﻿using Umbraco.Core;
using Umbraco.Core.Components;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Umbraco.Web.PublishedCache.NuCache
{
    public class NuCacheComponent : UmbracoComponentBase, IUmbracoCoreComponent
    {
        public override void Compose(Composition composition)
        {
            base.Compose(composition);

            // register the NuCache facade service
            composition.SetFacadeService(factory => new FacadeService(
                new FacadeService.Options { FacadeCacheIsApplicationRequestCache = true },
                factory.GetInstance<MainDom>(),
                factory.GetInstance<IRuntimeState>(),
                factory.GetInstance<ServiceContext>(),
                factory.GetInstance<IDatabaseUnitOfWorkProvider>(),
                factory.GetInstance<IFacadeAccessor>(),
                factory.GetInstance<ILogger>()));

            // add the NuCache health check (hidden from type finder)
            // todo - no NuCache health check yet
            //composition.HealthChecks().Add<NuCacheIntegrityHealthCheck>();
        }

        public void Initialize(IFacadeService service)
        {
            // nothing - this just ensures that the service is created at boot time
        }
    }
}
