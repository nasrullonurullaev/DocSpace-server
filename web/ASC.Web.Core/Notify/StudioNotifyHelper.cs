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

using Constants = ASC.Core.Users.Constants;

namespace ASC.Web.Studio.Core.Notify
{
    [Scope]
    public class StudioNotifyHelper
    {
        public readonly string Helplink;

        public readonly StudioNotifySource NotifySource;

        public readonly ISubscriptionProvider SubscriptionProvider;

        public readonly IRecipientProvider RecipientsProvider;

        private readonly int CountMailsToNotActivated;

        private readonly string NotificationImagePath;

        private UserManager UserManager { get; }
        private SettingsManager SettingsManager { get; }
        private CommonLinkUtility CommonLinkUtility { get; }
        private SetupInfo SetupInfo { get; }
        private TenantManager TenantManager { get; }
        private TenantExtra TenantExtra { get; }
        private CoreBaseSettings CoreBaseSettings { get; }
        private WebImageSupplier WebImageSupplier { get; }
        private ILog Log { get; }

        public StudioNotifyHelper(
            StudioNotifySource studioNotifySource,
            UserManager userManager,
            SettingsManager settingsManager,
            AdditionalWhiteLabelSettingsHelper additionalWhiteLabelSettingsHelper,
            CommonLinkUtility commonLinkUtility,
            SetupInfo setupInfo,
            TenantManager tenantManager,
            TenantExtra tenantExtra,
            CoreBaseSettings coreBaseSettings,
            WebImageSupplier webImageSupplier,
            IConfiguration configuration,
            IOptionsMonitor<ILog> option)
        {
            Helplink = commonLinkUtility.GetHelpLink(settingsManager, additionalWhiteLabelSettingsHelper, false);
            NotifySource = studioNotifySource;
            UserManager = userManager;
            SettingsManager = settingsManager;
            CommonLinkUtility = commonLinkUtility;
            SetupInfo = setupInfo;
            TenantManager = tenantManager;
            TenantExtra = tenantExtra;
            CoreBaseSettings = coreBaseSettings;
            WebImageSupplier = webImageSupplier;
            SubscriptionProvider = NotifySource.GetSubscriptionProvider();
            RecipientsProvider = NotifySource.GetRecipientsProvider();
            Log = option.CurrentValue;

            int.TryParse(configuration["core:notify:countspam"], out CountMailsToNotActivated);
            NotificationImagePath = configuration["web:notification:image:path"];
        }


        public IEnumerable<UserInfo> GetRecipients(bool toadmins, bool tousers, bool toguests)
        {
            if (toadmins)
            {
                if (tousers)
                {
                    if (toguests)
                        return UserManager.GetUsers();

                    return UserManager.GetUsers(EmployeeStatus.Default, EmployeeType.User);
                }

                if (toguests)
                    return
                        UserManager.GetUsersByGroup(Constants.GroupAdmin.ID)
                                   .Concat(UserManager.GetUsers(EmployeeStatus.Default, EmployeeType.Visitor));

                return UserManager.GetUsersByGroup(Constants.GroupAdmin.ID);
            }

            if (tousers)
            {
                if (toguests)
                    return UserManager.GetUsers()
                                      .Where(u => !UserManager.IsUserInGroup(u.Id, Constants.GroupAdmin.ID));

                return UserManager.GetUsers(EmployeeStatus.Default, EmployeeType.User)
                                  .Where(u => !UserManager.IsUserInGroup(u.Id, Constants.GroupAdmin.ID));
            }

            if (toguests)
                return UserManager.GetUsers(EmployeeStatus.Default, EmployeeType.Visitor);

            return new List<UserInfo>();
        }

        public IRecipient ToRecipient(Guid userId)
        {
            return RecipientsProvider.GetRecipient(userId.ToString());
        }

        public IRecipient[] RecipientFromEmail(string email, bool checkActivation)
        {
            return RecipientFromEmail(new List<string> { email }, checkActivation);
        }

        public IRecipient[] RecipientFromEmail(List<string> emails, bool checkActivation)
        {
            var res = new List<IRecipient>();

            if (emails == null) return res.ToArray();

            res.AddRange(emails.
                             Select(email => email.ToLower()).
                             Select(e => new DirectRecipient(e, null, new[] { e }, checkActivation)));

            if (!checkActivation
                && CountMailsToNotActivated > 0
                && TenantExtra.Saas && !CoreBaseSettings.Personal)
            {
                var tenant = TenantManager.GetCurrentTenant();
                var tariff = TenantManager.GetTenantQuota(tenant.Id);
                if (tariff.Free || tariff.Trial)
                {
                    var spamEmailSettings = SettingsManager.Load<SpamEmailSettings>();
                    var sended = spamEmailSettings.MailsSended;

                    var mayTake = Math.Max(0, CountMailsToNotActivated - sended);
                    var tryCount = res.Count;
                    if (mayTake < tryCount)
                    {
                        res = res.Take(mayTake).ToList();

                        Log.Warn(string.Format("Free tenant {0} for today is trying to send {1} more letters without checking activation. Sent {2}", tenant.Id, tryCount, mayTake));
                    }
                    spamEmailSettings.MailsSended = sended + tryCount;
                    SettingsManager.Save(spamEmailSettings);
                }
            }

            return res.ToArray();
        }

        public string GetNotificationImageUrl(string imageFileName)
        {
            if (string.IsNullOrEmpty(NotificationImagePath))
            {
                return
                    CommonLinkUtility.GetFullAbsolutePath(
                        WebImageSupplier.GetAbsoluteWebPath("notification/" + imageFileName));
            }

            return NotificationImagePath.TrimEnd('/') + "/" + imageFileName;
        }


        public bool IsSubscribedToNotify(Guid userId, INotifyAction notifyAction)
        {
            return IsSubscribedToNotify(ToRecipient(userId), notifyAction);
        }

        public bool IsSubscribedToNotify(IRecipient recipient, INotifyAction notifyAction)
        {
            return recipient != null && SubscriptionProvider.IsSubscribed(Log, notifyAction, recipient, null);
        }

        public void SubscribeToNotify(Guid userId, INotifyAction notifyAction, bool subscribe)
        {
            SubscribeToNotify(ToRecipient(userId), notifyAction, subscribe);
        }

        public void SubscribeToNotify(IRecipient recipient, INotifyAction notifyAction, bool subscribe)
        {
            if (recipient == null) return;

            if (subscribe)
            {
                SubscriptionProvider.Subscribe(notifyAction, null, recipient);
            }
            else
            {
                SubscriptionProvider.UnSubscribe(notifyAction, null, recipient);
            }
        }
    }
}