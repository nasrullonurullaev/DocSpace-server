// (c) Copyright Ascensio System SIA 2010-2022
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

namespace ASC.Web.Studio.Core.Backup;

public class BackupFileUploadHandler
{
    public BackupFileUploadHandler(RequestDelegate next)
    {

    }

    public async Task Invoke(HttpContext context,
        PermissionContext permissionContext,
        BackupAjaxHandler backupAjaxHandler,
        ICache cache,
        TenantManager tenantManager)
    {
        BackupFileUploadResult result = null;
        try
        {
            if (!await permissionContext.CheckPermissionsAsync(SecutiryConstants.EditPortalSettings))
            {
                throw new ArgumentException("Access denied.");
            }
            var tenantId = tenantManager.GetCurrentTenant().Id;
            var path = await backupAjaxHandler.GetTmpFilePathAsync(tenantId);
            if (context.Request.Query["Init"].ToString() == "true")
            {
                long.TryParse(context.Request.Query["totalSize"], out var size);
                if (size <= 0)
                {
                    throw new ArgumentException("Total size must be greater than 0.");
                }

                var maxSize = (await tenantManager.GetCurrentTenantQuotaAsync()).MaxTotalSize;
                if (size > maxSize)
                {
                    throw new ArgumentException(BackupResource.LargeBackup);
                }

                try
                {
                    if (File.Exists(path + "tar.gz"))
                    {
                        File.Delete(path + "tar.gz");
                    }
                    if (File.Exists(path + "tar"))
                    {
                        File.Delete(path + "tar");
                    }
                    var info = new UploadInfo
                    {
                        Ext = context.Request.Query["extension"].ToString() == "tar" ? "tar" : "tar.gz",
                        Size = size
                    };
                    cache.Insert($"{tenantId} backupTotalSize", info, TimeSpan.FromMinutes(20));

                    result = Success();
                }
                catch
                {
                    throw new ArgumentException("Can't start upload.");
                }
            }
            else
            {
                var info = cache.Get<UploadInfo>($"{tenantId} backupTotalSize");
                if (info != null)
                {
                    throw new ArgumentException("Need init upload.");
                }

                var file = context.Request.Form.Files[0];
                using var stream = file.OpenReadStream();

                var fs = File.Open(path + info.Ext, FileMode.Append);
                await stream.CopyToAsync(fs);

                if (fs.Length >= info.Size)
                {
                    cache.Remove($"{tenantId} backupTotalSize");
                    result = Success(endUpload: true);
                }
                else
                {
                    result = Success();
                }
            }
        }
        catch (Exception error)
        {
            result = Error(error.Message);
        }

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result, new System.Text.Json.JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }));
    }

    private BackupFileUploadResult Success(bool endUpload = false)
    {
        return new BackupFileUploadResult
        {
            Success = true,
            EndUpload = endUpload
        };
    }

    private BackupFileUploadResult Error(string messageFormat, params object[] args)
    {
        return new BackupFileUploadResult
        {
            Success = false,
            Message = string.Format(messageFormat, args)
        };
    }
}

internal class BackupFileUploadResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int ChunkSize { get; set; }
    public bool EndUpload { get; set; }
}

internal class UploadInfo
{
    public long Size { get; set; }
    public string Ext { get; set; }
}

public static class BackupFileUploadHandlerExtensions
{
    public static IApplicationBuilder UseBackupFileUploadHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BackupFileUploadHandler>();
    }
}
