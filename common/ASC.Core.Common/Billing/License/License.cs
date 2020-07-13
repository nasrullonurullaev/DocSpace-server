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


using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASC.Core.Billing
{
    [Serializable]
    [DebuggerDisplay("{DueDate}")]
    public class License
    {
        public string OriginalLicense { get; set; }

        [JsonPropertyName("affiliate_id")]
        public string AffiliateId { get; set; }

        //[Obsolete]
        public bool WhiteLabel { get; set; }

        public bool Customization { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime DueDate { get; set; }

        [JsonPropertyName("portal_count")]
        public int PortalCount { get; set; }

        public bool Trial { get; set; }

        [JsonPropertyName("user_quota")]
        public int ActiveUsers { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; }

        public string Signature { get; set; }


        public static License Parse(string licenseString)
        {
            if (string.IsNullOrEmpty(licenseString)) throw new BillingNotFoundException("License file is empty");

            try
            {
                var license = JsonSerializer.Deserialize<License>(licenseString);
                if (license == null) throw new BillingNotFoundException("Can't parse license");

                license.OriginalLicense = licenseString;

                return license;
            }
            catch (Exception)
            {
                throw new BillingNotFoundException("Can't parse license");
            }
        }
    }
}