﻿using System;
using LightInject;
using Umbraco.Core.Dictionary;
using Umbraco.Core.DI;
using Umbraco.Core.Macros;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence.Mappers;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Sync;
using Umbraco.Web.Editors;
using Umbraco.Web.HealthCheck;
using Umbraco.Web.Media;
using Umbraco.Web.Media.ThumbnailProviders;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.Routing;
using Umbraco.Web.Strategies.Migrations;
using Umbraco.Web._Legacy.Actions;

// the namespace here is intentional -  although defined in Umbraco.Web assembly,
// this class should be visible when using Umbraco.Core.Components, alongside
// Umbraco.Core's own CompositionExtensions class

// ReSharper disable once CheckNamespace
namespace Umbraco.Core.Components
{
    /// <summary>
    /// Provides extension methods to the <see cref="Composition"/> class.
    /// </summary>
    public static class WebCompositionExtensions
    {
        #region Collection Builders

        /// <summary>
        /// Gets the actions collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <returns></returns>
        internal static ActionCollectionBuilder Actions(this Composition composition)
            => composition.Container.GetInstance<ActionCollectionBuilder>();

        /// <summary>
        /// Gets the content finders collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <returns></returns>
        internal static XsltExtensionCollectionBuilder XsltExtensions(this Composition composition)
            => composition.Container.GetInstance<XsltExtensionCollectionBuilder>();

        /// <summary>
        /// Gets the content finders collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <returns></returns>
        public static ContentFinderCollectionBuilder ContentFinders(this Composition composition)
            => composition.Container.GetInstance<ContentFinderCollectionBuilder>();

        /// <summary>
        /// Gets the editor validators collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <returns></returns>
        internal static EditorValidatorCollectionBuilder EditorValidators(this Composition composition)
            => composition.Container.GetInstance<EditorValidatorCollectionBuilder>();

        /// <summary>
        /// Gets the filtered controller factories collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <returns></returns>
        public static FilteredControllerFactoryCollectionBuilder FilderedControllerFactory(this Composition composition)
            => composition.Container.GetInstance<FilteredControllerFactoryCollectionBuilder>();

        /// <summary>
        /// Gets the health checks collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        public static HealthCheckCollectionBuilder HealthChecks(this Composition composition)
            => composition.Container.GetInstance<HealthCheckCollectionBuilder>();

        /// <summary>
        /// Gets the image url providers collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        internal static ImageUrlProviderCollectionBuilder ImageUrlProviders(this Composition composition)
            => composition.Container.GetInstance<ImageUrlProviderCollectionBuilder>();

        /// <summary>
        /// Gets the thumbnail providers collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        internal static ThumbnailProviderCollectionBuilder ThumbnailProviders(this Composition composition)
            => composition.Container.GetInstance<ThumbnailProviderCollectionBuilder>();

        /// <summary>
        /// Gets the url providers collection builder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        internal static UrlProviderCollectionBuilder UrlProviders(this Composition composition)
            => composition.Container.GetInstance<UrlProviderCollectionBuilder>();

        #endregion

        #region Singletons

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <typeparam name="T">The type of the content last chance finder.</typeparam>
        /// <param name="composition">The composition.</param>
        public static void SetContentLastChanceFinder<T>(this Composition composition)
            where T : IContentLastChanceFinder
        {
            composition.Container.RegisterSingleton<IContentLastChanceFinder, T>();
        }

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="factory">A function creating a last chance finder.</param>
        public static void SetContentLastChanceFinder(this Composition composition, Func<IServiceFactory, IContentLastChanceFinder> factory)
        {
            composition.Container.RegisterSingleton(factory);
        }

        /// <summary>
        /// Sets the content last chance finder.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="finder">A last chance finder.</param>
        public static void SetContentLastChanceFinder(this Composition composition, IContentLastChanceFinder finder)
        {
            composition.Container.RegisterSingleton(_ => finder);
        }

        /// <summary>
        /// Sets the type of the default rendering controller.
        /// </summary>
        /// <typeparam name="T">The type of the default rendering controller.</typeparam>
        /// <param name="composition">The composition.</param>
        public static void SetDefaultRenderMvcControllerType<T>(this Composition composition)
        {
            Web.Current.DefaultRenderMvcControllerType = typeof(T);
        }

        /// <summary>
        /// Sets the facade service.
        /// </summary>
        /// <typeparam name="T">The type of the facade service.</typeparam>
        /// <param name="composition">The composition.</param>
        public static void SetFacadeService<T>(this Composition composition)
            where T : IFacadeService
        {
            composition.Container.RegisterSingleton<IFacadeService, T>();
        }

        /// <summary>
        /// Sets the facade service.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="factory">A function creating a service facade.</param>
        public static void SetFacadeService(this Composition composition, Func<IServiceFactory, IFacadeService> factory)
        {
            composition.Container.RegisterSingleton(factory);
        }

        /// <summary>
        /// Sets the facade service.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="service">A facade service.</param>
        public static void SetFacadeService(this Composition composition, IFacadeService service)
        {
            composition.Container.RegisterSingleton(_ => service);
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <typeparam name="T">The type of the site domain helper.</typeparam>
        /// <param name="composition"></param>
        public static void SetSiteDomainHelper<T>(this Composition composition)
            where T : ISiteDomainHelper
        {
            composition.Container.RegisterSingleton<ISiteDomainHelper, T>();
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="factory">A function creating a helper.</param>
        public static void SetSiteDomainHelper(this Composition composition, Func<IServiceFactory, ISiteDomainHelper> factory)
        {
            composition.Container.RegisterSingleton(factory);
        }

        /// <summary>
        /// Sets the site domain helper.
        /// </summary>
        /// <param name="composition">The composition.</param>
        /// <param name="helper">A helper.</param>
        public static void SetSiteDomainHelper(this Composition composition, ISiteDomainHelper helper)
        {
            composition.Container.RegisterSingleton(_ => helper);
        }

        #endregion
    }
}
