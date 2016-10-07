﻿using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.DI;
using Umbraco.Core.Media;
using Umbraco.Web.Media.ImageUrlProviders;

namespace Umbraco.Web.Media
{
    internal class ImageUrlProviderCollection : BuilderCollectionBase<IImageUrlProvider>
    {
        public ImageUrlProviderCollection(IEnumerable<IImageUrlProvider> items) 
            : base(items)
        { }

        public IImageUrlProvider this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name)) name = ImageUrlProvider.DefaultName;
                var provider = this.FirstOrDefault(x => x.Name.InvariantEquals(name));
                if (provider == null) throw new InvalidOperationException($"No provider exists with name \"{name}\".");
                return provider;
            }
        }
    }
}
