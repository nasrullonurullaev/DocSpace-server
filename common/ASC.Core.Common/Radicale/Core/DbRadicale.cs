﻿// (c) Copyright Ascensio System SIA 2009-2024
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

namespace ASC.Common.Radicale.Core;

[Scope]
public class DbRadicale(IDbContextFactory<UserDbContext> dbContextFactory)
{
    public async Task SaveCardDavUserAsync(int tenant, Guid id)
    {
        await using var userDbContext = await dbContextFactory.CreateDbContextAsync();
        await userDbContext.AddOrUpdateAsync(q => q.UsersDav, new UserDav { TenantId = tenant, UserId = id });
        await userDbContext.SaveChangesWithValidateAsync();
    }

    public async Task RemoveCardDavUserAsync(int tenant, Guid id)
    {
        await using var userDbContext = await dbContextFactory.CreateDbContextAsync();
        var userDav = await Queries.UserDavAsync(userDbContext, tenant, id);
        if (userDav != null)
        {
            userDbContext.UsersDav.Remove(userDav);
            await userDbContext.SaveChangesWithValidateAsync();
        }
    }

    public async Task<bool> IsExistCardDavUserAsync(int tenant, Guid id)
    {
        await using var userDbContext = await dbContextFactory.CreateDbContextAsync();
        return await Queries.UserDavAnyAsync(userDbContext, tenant, id);
    }
}

static file class Queries
{
    public static readonly Func<UserDbContext, int, Guid, Task<UserDav>> UserDavAsync =
        EF.CompileAsyncQuery(
            (UserDbContext ctx, int tenantId, Guid userId) =>
                ctx.UsersDav.FirstOrDefault(r => r.TenantId == tenantId && r.UserId == userId));

    public static readonly Func<UserDbContext, int, Guid, Task<bool>> UserDavAnyAsync =
        EF.CompileAsyncQuery(
            (UserDbContext ctx, int tenantId, Guid userId) =>
                ctx.UsersDav.Any(r => r.TenantId == tenantId && r.UserId == userId));
}
