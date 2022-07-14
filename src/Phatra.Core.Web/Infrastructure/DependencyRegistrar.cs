using System.Linq;
using System.Web;
using Phatra.Core.Infrastructure.DependencyManagement;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Phatra.Core.Infrastructure;
using Phatra.Core.Web.Caching;
using Phatra.Core.Web.Fakes;
using Phatra.Core.Web.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace Phatra.Core.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {

            // Register Web Abstractions(HttpContextBase, HttpRequestBase, HttpResponseBase,
            //        HttpServerUtilityBase, HttpSessionStateBase, HttpApplicationStateBase, HttpBrowserCapabilitiesBase,
            //        HttpFileCollectionBase, RequestContext, HttpCachePolicyBase, VirtualPathProvider, UrlHelper
            builder.RegisterModule<AutofacWebTypesModule>();

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            builder.RegisterType<HandleExceptionAttribute>().PropertiesAutowired();
            builder.RegisterFilterProvider();

            builder.RegisterType<PerUserCacheManager>().As<IPerUserCacheManager>().InstancePerLifetimeScope();
            builder.RegisterType<PerRequestCacheManager>().As<IPerRequestCacheManager>().InstancePerLifetimeScope();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            // api controller
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());

        }
        public void SetDependencyResolver(IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
