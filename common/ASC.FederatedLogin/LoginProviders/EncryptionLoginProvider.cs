/*
 *
 * (c) Copyright Ascensio System Limited 2010-2018
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/


using SecurityContext = ASC.Core.SecurityContext;

namespace ASC.Web.Studio.Core;

[Scope]
public class EncryptionLoginProvider
{
    private readonly SecurityContext _securityContext;
    private readonly Signature _signature;
    private readonly InstanceCrypto _instanceCrypto;
    private readonly IOptionsSnapshot<AccountLinker> _snapshot;

    public EncryptionLoginProvider(
        SecurityContext securityContext,
        Signature signature,
        InstanceCrypto instanceCrypto,
        IOptionsSnapshot<AccountLinker> snapshot)
    {
        _securityContext = securityContext;
        _signature = signature;
        _instanceCrypto = instanceCrypto;
        _snapshot = snapshot;
    }


    public void SetKeys(Guid userId, string keys)
    {
        if (string.IsNullOrEmpty(keys))
        {
            return;
        }

        var loginProfile = new LoginProfile(_signature, _instanceCrypto)
        {
            Provider = ProviderConstants.Encryption,
            Name = _instanceCrypto.Encrypt(keys)
        };

        var linker = _snapshot.Get("webstudio");
        linker.AddLink(userId.ToString(), loginProfile);
    }

    public string GetKeys()
    {
        return GetKeys(_securityContext.CurrentAccount.ID);
    }

    public string GetKeys(Guid userId)
    {
        var linker = _snapshot.Get("webstudio");
        var profile = linker.GetLinkedProfiles(userId.ToString(), ProviderConstants.Encryption).FirstOrDefault();
        if (profile == null)
        {
            return null;
        }

        var keys = _instanceCrypto.Decrypt(profile.Name);

        return keys;
    }
}
