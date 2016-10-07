﻿using LightInject;
using Umbraco.Core.DI;

namespace Umbraco.Core.Strings
{
    public class UrlSegmentProviderCollectionBuilder : OrderedCollectionBuilderBase<UrlSegmentProviderCollectionBuilder, UrlSegmentProviderCollection, IUrlSegmentProvider>
    {
        public UrlSegmentProviderCollectionBuilder(IServiceContainer container)
            : base(container)
        { }

        protected override UrlSegmentProviderCollectionBuilder This => this;
    }
}
