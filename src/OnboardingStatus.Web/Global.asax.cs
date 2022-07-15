using Phatra.Core.Logging;
using Phatra.Core.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnboardingStatus_Web
{

    public class MvcApplication : HttpApplication
    {
        private ILogger _log;

        protected void Application_Start()
        {
            _log = Log4NetLogger.CreateLoggerFor(this.GetType());
            _log.Debug("Application Started ...");
            WebContext.Instance.Initialize(true);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DevExtremeBundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
