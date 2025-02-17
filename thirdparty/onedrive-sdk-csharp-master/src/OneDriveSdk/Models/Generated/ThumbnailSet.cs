// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.Graph;

using Newtonsoft.Json;

// **NOTE** This file was generated by a tool and any changes will be overwritten.


namespace Microsoft.OneDrive.Sdk
{
    /// <summary>
    /// The type Thumbnail Set.
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(DerivedTypeConverter))]
    public partial class ThumbnailSet
    {
    
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false, IsRequired = false)]
        public string Id { get; set; }
    
        /// <summary>
        /// Gets or sets large.
        /// </summary>
        [DataMember(Name = "large", EmitDefaultValue = false, IsRequired = false)]
        public Thumbnail Large { get; set; }
    
        /// <summary>
        /// Gets or sets medium.
        /// </summary>
        [DataMember(Name = "medium", EmitDefaultValue = false, IsRequired = false)]
        public Thumbnail Medium { get; set; }
    
        /// <summary>
        /// Gets or sets small.
        /// </summary>
        [DataMember(Name = "small", EmitDefaultValue = false, IsRequired = false)]
        public Thumbnail Small { get; set; }
    
        /// <summary>
        /// Gets or sets source.
        /// </summary>
        [DataMember(Name = "source", EmitDefaultValue = false, IsRequired = false)]
        public Thumbnail Source { get; set; }
    
        /// <summary>
        /// Gets or sets @odata.type.
        /// </summary>
        [DataMember(Name = "@odata.type", EmitDefaultValue = false, IsRequired = false)]
        public string ODataType { get; set; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }
    
    }
}

