// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.Graph;

// **NOTE** This file was generated by a tool and any changes will be overwritten.

namespace Microsoft.OneDrive.Sdk
{
    /// <summary>
    /// The type ItemThumbnailsCollectionRequestBuilder.
    /// </summary>
    public partial class ItemThumbnailsCollectionRequestBuilder : BaseRequestBuilder, IItemThumbnailsCollectionRequestBuilder
    {
        /// <summary>
        /// Constructs a new ItemThumbnailsCollectionRequestBuilder.
        /// </summary>
        /// <param name="requestUrl">The URL for the built request.</param>
        /// <param name="client">The <see cref="IBaseClient"/> for handling requests.</param>
        public ItemThumbnailsCollectionRequestBuilder(
            string requestUrl,
            IBaseClient client)
            : base(requestUrl, client)
        {
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <returns>The built request.</returns>
        public IItemThumbnailsCollectionRequest Request()
        {
            return this.Request(null);
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <param name="options">The query and header options for the request.</param>
        /// <returns>The built request.</returns>
        public IItemThumbnailsCollectionRequest Request(IEnumerable<Option> options)
        {
            return new ItemThumbnailsCollectionRequest(this.RequestUrl, this.Client, options);
        }

        /// <summary>
        /// Gets an <see cref="IThumbnailSetRequestBuilder"/> for the specified ItemThumbnailSet.
        /// </summary>
        /// <param name="id">The ID for the ItemThumbnailSet.</param>
        /// <returns>The <see cref="IThumbnailSetRequestBuilder"/>.</returns>
        public IThumbnailSetRequestBuilder this[string id]
        {
            get
            {
                return new ThumbnailSetRequestBuilder(this.AppendSegmentToRequestUrl(id), this.Client);
            }
        }
    }
}
