// (c) Copyright Ascensio System SIA 2009-2024
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

namespace ASC.Web.Core.RemovePortal;

[Singleton]
public class RemovePortalWorker(
    IDistributedTaskQueueFactory queueFactory,
    IServiceProvider serviceProvider,
    IDistributedLockProvider distributedLockProvider)
{
    private readonly DistributedTaskQueue _queue = queueFactory.CreateQueue(CUSTOM_DISTRIBUTED_TASK_QUEUE_NAME);

    public const string CUSTOM_DISTRIBUTED_TASK_QUEUE_NAME = "removePortal";

    public async Task StartAsync(int tenantId)
    {
        await using (await distributedLockProvider.TryAcquireLockAsync($"lock_{CUSTOM_DISTRIBUTED_TASK_QUEUE_NAME}"))
        {
            var item = (await _queue.GetAllTasks<RemovePortalOperation>()).FirstOrDefault(t => t.TenantId == tenantId);

            if (item is { IsCompleted: true })
            {
                await _queue.DequeueTask(item.Id);
                item = null;
            }
            if (item == null)
            {
                item = serviceProvider.GetService<RemovePortalOperation>();

                item.Init(tenantId);

                await _queue.EnqueueTask(item);
            }

            await item.PublishChanges();
        }
    }

    public async Task Stop()
    {
        var tasks = await _queue.GetAllTasks(DistributedTaskQueue.INSTANCE_ID);

        foreach (var t in tasks)
        {
            await _queue.DequeueTask(t.Id);
        }
    }
}