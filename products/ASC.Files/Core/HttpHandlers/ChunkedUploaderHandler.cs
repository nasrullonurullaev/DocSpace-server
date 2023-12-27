// (c) Copyright Ascensio System SIA 2010-2023
// 
// This program is a free software product.
// You can redistribute it and/or modify it under the terms
// of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
// Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
// to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of
// any third-party rights.
// 
// This program is distributed WITHOUT ANY WARRANTY, without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see
// the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
// 
// You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
// 
// The  interactive user interfaces in modified source and object code versions of the Program must
// display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
// 
// Pursuant to Section 7(b) of the License you must retain the original Product logo when
// distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under
// trademark law for use of our trademarks.
// 
// All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
// content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
// International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode

namespace ASC.Web.Files.HttpHandlers;

public class ChunkedUploaderHandler
{
    public ChunkedUploaderHandler(RequestDelegate _)
    {
    }

    public async Task Invoke(HttpContext context, ChunkedUploaderHandlerService chunkedUploaderHandlerService)
    {
        await chunkedUploaderHandlerService.Invoke(context);
    }
}

[Scope]
public class ChunkedUploaderHandlerService(ILogger<ChunkedUploaderHandlerService> logger,
    TenantManager tenantManager,
    FileUploader fileUploader,
    FilesMessageService filesMessageService,
    ChunkedUploadSessionHolder chunkedUploadSessionHolder,
    ChunkedUploadSessionHelper chunkedUploadSessionHelper,
    SocketManager socketManager,
    FileDtoHelper filesWrapperHelper,
    AuthContext authContext,
    IDaoFactory daoFactory,
    IServiceProvider serviceProvider)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await Invoke<int>(context);
        }
        catch (Exception)
        {
            await Invoke<string>(context);
        }
    }

    private async Task Invoke<T>(HttpContext context)
    {
        try
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 200;

                return;
            }

            var request = new ChunkedRequestHelper<T>(context.Request);

            if (!await TryAuthorizeAsync(request))
            {
                await WriteError(context, "Not authorized or session with specified upload id already expired");

                return;
            }

            if ((await tenantManager.GetCurrentTenantAsync()).Status != TenantStatus.Active)
            {
                await WriteError(context, "Can't perform upload for deleted or transferring portals");

                return;
            }

            switch (request.Type())
            {
                case ChunkedRequestType.Abort:
                    await fileUploader.AbortUploadAsync<T>(request.UploadId);
                    await WriteSuccess(context, null);

                    return;

                case ChunkedRequestType.Initiate:
                    var createdSession = await fileUploader.InitiateUploadAsync(request.FolderId, request.FileId, request.FileName, request.FileSize, request.Encrypted);
                    await WriteSuccess(context, await chunkedUploadSessionHelper.ToResponseObjectAsync(createdSession, true));

                    return;

                case ChunkedRequestType.Upload:
                    var resumedSession = await fileUploader.UploadChunkAsync<T>(request.UploadId, request.ChunkStream, request.ChunkSize);

                    if (resumedSession.BytesUploaded == resumedSession.BytesTotal)
                    {
                        await WriteSuccess(context, await ToResponseObject(resumedSession.File), (int)HttpStatusCode.Created);
                        await filesMessageService.SendAsync(MessageAction.FileUploaded, resumedSession.File, resumedSession.File.Title);

                        var fileExst = FileUtility.GetFileExtension(resumedSession.File.Title);
                        var fileType = FileUtility.GetFileTypeByExtention(fileExst);

                        await socketManager.CreateFileAsync(resumedSession.File);
                    }
                    else
                    {
                        await WriteSuccess(context, await chunkedUploadSessionHelper.ToResponseObjectAsync(resumedSession));
                    }

                    return;

                default:
                    await WriteError(context, "Unknown request type.");
                    return;
            }
        }
        catch (FileNotFoundException error)
        {
            logger.ErrorChunkedUploaderHandlerService(error);
            await WriteError(context, FilesCommonResource.ErrorMassage_FileNotFound);
        }
        catch (Exception error)
        {
            logger.ErrorChunkedUploaderHandlerService(error);
            await WriteError(context, error.Message);
        }
    }

    private async Task<bool> TryAuthorizeAsync<T>(ChunkedRequestHelper<T> request)
    {
        if (!authContext.IsAuthenticated)
        {
            return false;
        }

        if (request.Type() == ChunkedRequestType.Initiate)
        {
            return true;
        }

        if (!string.IsNullOrEmpty(request.UploadId))
        {
            var uploadSession = await chunkedUploadSessionHolder.GetSessionAsync<T>(request.UploadId);
            if (uploadSession != null && authContext.CurrentAccount.ID == uploadSession.UserId)
            {
                return true;
            }
        }

        return false;
    }

    private static Task WriteError(HttpContext context, string message)
    {
        return WriteResponse(context, false, null, message, (int)HttpStatusCode.OK);
    }

    private static Task WriteSuccess(HttpContext context, object data, int statusCode = (int)HttpStatusCode.OK)
    {
        return WriteResponse(context, true, data, string.Empty, statusCode);
    }

    private static Task WriteResponse(HttpContext context, bool success, object data, string message, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(new { success, data, message }, new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }

    private async Task<object> ToResponseObject<T>(File<T> file)
    {
        return new
        {
            id = file.Id,
            folderId = file.ParentId,
            version = file.Version,
            title = file.Title,
            provider_key = file.ProviderKey,
            uploaded = true,
            file = await filesWrapperHelper.GetAsync(file)
        };
    }
}

public enum ChunkedRequestType
{
    None,
    Initiate,
    Abort,
    Upload
}

[DebuggerDisplay("{Type} ({UploadId})")]
public class ChunkedRequestHelper<T>(HttpRequest request)
{
    private readonly HttpRequest _request = request ?? throw new ArgumentNullException(nameof(request));
    private IFormFile _file;
    private int? _tenantId;
    private long? _fileContentLength;

    public ChunkedRequestType Type()
    {
        if (_request.Query["initiate"] == "true" && IsFileDataSet())
        {
            return ChunkedRequestType.Initiate;
        }

        if (_request.Query["abort"] == "true" && !string.IsNullOrEmpty(UploadId))
        {
            return ChunkedRequestType.Abort;
        }

        return !string.IsNullOrEmpty(UploadId)
                    ? ChunkedRequestType.Upload
                    : ChunkedRequestType.None;
    }

    public string UploadId => _request.Query["uid"];

    public int TenantId
    {
        get
        {
            if (!_tenantId.HasValue)
            {
                if (int.TryParse(_request.Query["tid"], out var v))
                {
                    _tenantId = v;
                }
                else
                {
                    _tenantId = -1;
                }
            }

            return _tenantId.Value;
        }
    }

    public T FolderId
    {
        get
        {
            var queryValue = _request.Query[FilesLinkUtility.FolderId];

            if (queryValue.Count == 0)
            {
                return default;
            }

            return IdConverter.Convert<T>(queryValue[0]);
        }
    }

    public T FileId
    {
        get
        {
            var queryValue = _request.Query[FilesLinkUtility.FileId];

            if (queryValue.Count == 0)
            {
                return default;
            }

            return IdConverter.Convert<T>(queryValue[0]);
        }
    }

    public string FileName => _request.Query[FilesLinkUtility.FileTitle];

    public long FileSize
    {
        get
        {
            if (!_fileContentLength.HasValue)
            {
                long.TryParse(_request.Query["fileSize"], out var v);
                _fileContentLength = v;
            }

            return _fileContentLength.Value;
        }
    }

    public long ChunkSize => File.Length;

    public Stream ChunkStream => File.OpenReadStream();

    public bool Encrypted => _request.Query["encrypted"] == "true";

    private IFormFile File
    {
        get
        {
            if (_file != null)
            {
                return _file;
            }

            if (_request.Form.Files.Count > 0)
            {
                return _file = _request.Form.Files[0];
            }

            throw new Exception("HttpRequest.Files is empty");
        }
    }

    private bool IsFileDataSet()
    {
        return !string.IsNullOrEmpty(FileName) && !EqualityComparer<T>.Default.Equals(FolderId, default);
    }
}

public static class ChunkedUploaderHandlerExtension
{
    public static IApplicationBuilder UseChunkedUploaderHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChunkedUploaderHandler>();
    }
}
