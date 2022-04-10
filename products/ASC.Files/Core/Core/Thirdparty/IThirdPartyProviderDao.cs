﻿// (c) Copyright Ascensio System SIA 2010-2022
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

namespace ASC.Files.Thirdparty;

internal abstract class ThirdPartyProviderDao
{
    #region FileDao

    public Task ReassignFilesAsync(string[] fileIds, Guid newOwnerId)
    {
        return Task.CompletedTask;
    }

    public Task<List<File<string>>> GetFilesAsync(IEnumerable<string> parentIds, FilterType filterType, bool subjectGroup, Guid subjectID, string searchText, bool searchInContent)
    {
        return Task.FromResult(new List<File<string>>());
    }

    public IAsyncEnumerable<File<string>> SearchAsync(string text, bool bunch)
    {
        return null;
    }

    public Task<bool> IsExistOnStorageAsync(File<string> file)
    {
        return Task.FromResult(true);
    }

    public Task SaveEditHistoryAsync(File<string> file, string changes, Stream differenceStream)
    {
        //Do nothing
        return Task.CompletedTask;
    }

    public Task<List<EditHistory>> GetEditHistoryAsync(DocumentServiceHelper documentServiceHelper, string fileId, int fileVersion)
    {
        return null;
    }

    public Task<Stream> GetDifferenceStreamAsync(File<string> file)
    {
        return null;
    }

    public Task<bool> ContainChangesAsync(string fileId, int fileVersion)
    {
        return Task.FromResult(false);
    }

    public Task SaveThumbnailAsync(File<string> file, Stream thumbnail)
    {
        //Do nothing
        return Task.CompletedTask;
    }

    public Task<Stream> GetThumbnailAsync(File<string> file)
    {
        return Task.FromResult<Stream>(null);
    }

    public virtual Task<Stream> GetFileStreamAsync(File<string> file)
    {
        return null;
    }

    public string GetUniqFilePath(File<string> file, string fileTitle)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileWithShare>> GetFeedsAsync(int tenant, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<int>> GetTenantsWithFeedsAsync(DateTime fromTime)
    {
        throw new NotImplementedException();
    }

    #endregion
    #region FolderDao

    public Task ReassignFoldersAsync(string[] folderIds, Guid newOwnerId)
    {
        return Task.CompletedTask;
    }

    public IAsyncEnumerable<Folder<string>> SearchFoldersAsync(string text, bool bunch)
    {
        return null;
    }


    public Task<string> GetFolderIDAsync(string module, string bunch, string data, bool createIfNotExists)
    {
        return null;
    }

    public Task<IEnumerable<string>> GetFolderIDsAsync(string module, string bunch, IEnumerable<string> data, bool createIfNotExists)
    {
        return Task.FromResult((IEnumerable<string>)new List<string>());
    }

    public Task<string> GetFolderIDCommonAsync(bool createIfNotExists)
    {
        return null;
    }


    public Task<string> GetFolderIDUserAsync(bool createIfNotExists, Guid? userId)
    {
        return null;
    }

    public Task<string> GetFolderIDShareAsync(bool createIfNotExists)
    {
        return null;
    }


    public Task<string> GetFolderIDRecentAsync(bool createIfNotExists)
    {
        return null;
    }

    public Task<string> GetFolderIDFavoritesAsync(bool createIfNotExists)
    {
        return null;
    }

    public Task<string> GetFolderIDTemplatesAsync(bool createIfNotExists)
    {
        return null;
    }

    public Task<string> GetFolderIDPrivacyAsync(bool createIfNotExists, Guid? userId)
    {
        return null;
    }

    public Task<string> GetFolderIDTrashAsync(bool createIfNotExists, Guid? userId)
    {
        return null;
    }

    public string GetFolderIDPhotos(bool createIfNotExists)
    {
        return null;
    }


    public Task<string> GetFolderIDProjectsAsync(bool createIfNotExists)
    {
        return null;
    }

    public Task<string> GetFolderIDVirtualRooms(bool createIfNotExists)
    {
        return null;
    }

    public Task<string> GetBunchObjectIDAsync(string folderID)
    {
        return null;
    }

    public Task<Dictionary<string, string>> GetBunchObjectIDsAsync(List<string> folderIDs)
    {
        return null;
    }

    public Task<IEnumerable<FolderWithShare>> GetFeedsForFoldersAsync(int tenant, DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetTenantsWithFeedsForFoldersAsync(DateTime fromTime)
    {
        throw new NotImplementedException();
    }

    #endregion
}

internal abstract class ThirdPartyProviderDao<T> : ThirdPartyProviderDao, IDisposable where T : class, IProviderInfo
{
    public int TenantID { get; private set; }
    protected readonly IServiceProvider ServiceProvider;
    protected readonly UserManager UserManager;
    protected readonly TenantUtil TenantUtil;
    private readonly Lazy<FilesDbContext> _lazyFilesDbContext;
    protected FilesDbContext FilesDbContext => _lazyFilesDbContext.Value;
    protected readonly SetupInfo SetupInfo;
    protected readonly ILog Logger;
    protected readonly FileUtility FileUtility;
    protected readonly TempPath TempPath;
    protected RegexDaoSelectorBase<T> DaoSelector { get; set; }
    protected T ProviderInfo { get; set; }
    protected string PathPrefix { get; private set; }

    protected abstract string Id { get; }

    protected ThirdPartyProviderDao(
        IServiceProvider serviceProvider,
        UserManager userManager,
        TenantManager tenantManager,
        TenantUtil tenantUtil,
        DbContextManager<FilesDbContext> dbContextManager,
        SetupInfo setupInfo,
        IOptionsMonitor<ILog> monitor,
        FileUtility fileUtility,
        TempPath tempPath)
    {
        ServiceProvider = serviceProvider;
        UserManager = userManager;
        TenantUtil = tenantUtil;
        _lazyFilesDbContext = new Lazy<FilesDbContext>(() => dbContextManager.Get(FileConstant.DatabaseId));
        SetupInfo = setupInfo;
        Logger = monitor.CurrentValue;
        FileUtility = fileUtility;
        TempPath = tempPath;
        TenantID = tenantManager.GetCurrentTenant().Id;
    }

    public void Init(BaseProviderInfo<T> providerInfo, RegexDaoSelectorBase<T> selectorBase)
    {
        ProviderInfo = providerInfo.ProviderInfo;
        PathPrefix = providerInfo.PathPrefix;
        DaoSelector = selectorBase;
    }

    protected IQueryable<TSet> Query<TSet>(DbSet<TSet> set) where TSet : class, IDbFile
    {
        return set.AsQueryable().Where(r => r.TenantId == TenantID);
    }

    protected Task<string> MappingIDAsync(string id, bool saveIfNotExist = false)
    {
        if (id == null)
        {
            return null;
        }

        return InternalMappingIDAsync(id, saveIfNotExist);
    }

    private async Task<string> InternalMappingIDAsync(string id, bool saveIfNotExist = false)
    {
        string result;
        if (id.StartsWith(Id))
        {
            result = Regex.Replace(BitConverter.ToString(Hasher.Hash(id, HashAlg.MD5)), "-", "").ToLower();
        }
        else
        {
            result = await FilesDbContext.ThirdpartyIdMapping
                    .AsQueryable()
                    .Where(r => r.HashId == id)
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
        }
        if (saveIfNotExist)
        {
            var newMapping = new DbFilesThirdpartyIdMapping
            {
                Id = id,
                HashId = result,
                TenantId = TenantID
            };

            await FilesDbContext.ThirdpartyIdMapping.AddAsync(newMapping).ConfigureAwait(false);
            await FilesDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        return result;
    }

    protected Folder<string> GetFolder()
    {
        var folder = ServiceProvider.GetService<Folder<string>>();

        InitFileEntry(folder);

        folder.FolderType = FolderType.DEFAULT;
        folder.Shareable = false;
        folder.FilesCount = 0;
        folder.FoldersCount = 0;

        return folder;
    }

    protected Folder<string> GetErrorFolder(ErrorEntry entry)
    {
        var folder = GetFolder();

        InitFileEntryError(folder, entry);

        folder.ParentId = null;

        return folder;
    }

    protected File<string> GetFile()
    {
        var file = ServiceProvider.GetService<File<string>>();

        InitFileEntry(file);

        file.Access = FileShare.None;
        file.Shared = false;
        file.Version = 1;

        return file;
    }

    protected File<string> GetErrorFile(ErrorEntry entry)
    {
        var file = GetFile();
        InitFileEntryError(file, entry);

        return file;
    }

    protected void InitFileEntry(FileEntry<string> fileEntry)
    {
        fileEntry.CreateBy = ProviderInfo.Owner;
        fileEntry.ModifiedBy = ProviderInfo.Owner;
        fileEntry.ProviderId = ProviderInfo.ID;
        fileEntry.ProviderKey = ProviderInfo.ProviderKey;
        fileEntry.RootCreateBy = ProviderInfo.Owner;
        fileEntry.RootFolderType = ProviderInfo.RootFolderType;
        fileEntry.RootId = MakeId();
    }

    protected void InitFileEntryError(FileEntry<string> fileEntry, ErrorEntry entry)
    {
        fileEntry.Id = MakeId(entry.ErrorId);
        fileEntry.CreateOn = TenantUtil.DateTimeNow();
        fileEntry.ModifiedOn = TenantUtil.DateTimeNow();
        fileEntry.Error = entry.Error;
    }


    protected abstract string MakeId(string path = null);


    #region SecurityDao
    public Task SetShareAsync(FileShareRecord r)
    {
        return Task.CompletedTask;
    }

    public ValueTask<List<FileShareRecord>> GetSharesAsync(IEnumerable<Guid> subjects)
    {
        List<FileShareRecord> result = null;

        return new ValueTask<List<FileShareRecord>>(result);
    }

    public Task<IEnumerable<FileShareRecord>> GetSharesAsync(IEnumerable<FileEntry<string>> entry)
    {
        return null;
    }

    public Task<IEnumerable<FileShareRecord>> GetSharesAsync(FileEntry<string> entry)
    {
        return null;
    }

    public Task RemoveSubjectAsync(Guid subject)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<FileShareRecord>> GetPureShareRecordsAsync(IEnumerable<FileEntry<string>> entries)
    {
        return null;
    }

    public Task<IEnumerable<FileShareRecord>> GetPureShareRecordsAsync(FileEntry<string> entry)
    {
        return null;
    }

    public Task DeleteShareRecordsAsync(IEnumerable<FileShareRecord> records)
    {
        return Task.CompletedTask;
    }

    public ValueTask<bool> IsSharedAsync(object entryId, FileEntryType type)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region TagDao

    public IAsyncEnumerable<Tag> GetTagsAsync(Guid subject, TagType tagType, IEnumerable<FileEntry<string>> fileEntries)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IAsyncEnumerable<Tag> GetTagsAsync(TagType tagType, IEnumerable<FileEntry<string>> fileEntries)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IAsyncEnumerable<Tag> GetTagsAsync(Guid owner, TagType tagType)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IAsyncEnumerable<Tag> GetTagsAsync(string name, TagType tagType)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IAsyncEnumerable<Tag> GetTagsAsync(string[] names, TagType tagType)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public Task<IDictionary<object, IEnumerable<Tag>>> GetTagsAsync(Guid subject, IEnumerable<TagType> tagType, IEnumerable<FileEntry<string>> fileEntries)
    {
        return Task.FromResult((IDictionary<object, IEnumerable<Tag>>)new Dictionary<object, IEnumerable<Tag>>());
    }

    public IAsyncEnumerable<Tag> GetNewTagsAsync(Guid subject, IEnumerable<FileEntry<string>> fileEntries)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IAsyncEnumerable<Tag> GetNewTagsAsync(Guid subject, FileEntry<string> fileEntry)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public IEnumerable<Tag> SaveTags(IEnumerable<Tag> tag)
    {
        return new List<Tag>();
    }

    public IEnumerable<Tag> SaveTags(Tag tag)
    {
        return new List<Tag>();
    }

    public void UpdateNewTags(IEnumerable<Tag> tag)
    {
    }

    public void UpdateNewTags(Tag tag)
    {
    }

    public void RemoveTags(IEnumerable<Tag> tag)
    {
    }

    public void RemoveTags(Tag tag)
    {
    }

    public IAsyncEnumerable<Tag> GetTagsAsync(string entryID, FileEntryType entryType, TagType tagType)
    {
        return AsyncEnumerable.Empty<Tag>();
    }

    public void MarkAsNew(Guid subject, FileEntry<string> fileEntry)
    {
    }

    public async IAsyncEnumerable<Tag> GetNewTagsAsync(Guid subject, Folder<string> parentFolder, bool deepSearch)
    {
        var folderId = DaoSelector.ConvertId(parentFolder.Id);

        var entryIDs = await FilesDbContext.ThirdpartyIdMapping
                   .AsQueryable()
                   .Where(r => r.Id.StartsWith(parentFolder.Id))
                   .Select(r => r.HashId)
                   .ToListAsync()
                   .ConfigureAwait(false);

        if (!entryIDs.Any())
        {
            yield break;
        }

        var q = from r in FilesDbContext.Tag
                from l in FilesDbContext.TagLink.AsQueryable().Where(a => a.TenantId == r.TenantId && a.TagId == r.Id).DefaultIfEmpty()
                where r.TenantId == TenantID && l.TenantId == TenantID && r.Type == TagType.New && entryIDs.Contains(l.EntryId)
                select new { tag = r, tagLink = l };

        if (subject != Guid.Empty)
        {
            q = q.Where(r => r.tag.Owner == subject);
        }

        var qList = q
            .Distinct()
            .AsAsyncEnumerable();

        var tags = qList
            .SelectAwait(async r => new Tag
            {
                Name = r.tag.Name,
                Type = r.tag.Type,
                Owner = r.tag.Owner,
                EntryId = await MappingIDAsync(r.tagLink.EntryId).ConfigureAwait(false),
                EntryType = r.tagLink.EntryType,
                Count = r.tagLink.Count,
                Id = r.tag.Id
            });


        if (deepSearch)
        {
            await foreach (var e in tags.ConfigureAwait(false))
            {
                yield return e;
            }

            yield break;
        }

        var folderFileIds = new[] { parentFolder.Id }
            .Concat(await GetChildrenAsync(folderId).ConfigureAwait(false));

        await foreach (var e in tags.Where(tag => folderFileIds.Contains(tag.EntryId.ToString())).ConfigureAwait(false))
        {
            yield return e;
        }
    }

    protected abstract Task<IEnumerable<string>> GetChildrenAsync(string folderId);

    #endregion

    public void Dispose()
    {
        if (ProviderInfo != null)
        {
            ProviderInfo.Dispose();
            ProviderInfo = null;
        }
    }
}

internal class ErrorEntry
{
    public string Error { get; set; }
    public string ErrorId { get; set; }

    public ErrorEntry(string error, string errorId)
    {
        Error = error;
        ErrorId = errorId;
    }
}

public class TagLink
{
    public int TenantId { get; set; }
    public int Id { get; set; }
}

public class TagLinkComparer : IEqualityComparer<TagLink>
{
    public bool Equals([AllowNull] TagLink x, [AllowNull] TagLink y)
    {
        return x.Id == y.Id && x.TenantId == y.TenantId;
    }

    public int GetHashCode([DisallowNull] TagLink obj)
    {
        return obj.Id.GetHashCode() + obj.TenantId.GetHashCode();
    }
}
