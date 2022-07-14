using System.IO;
using System.Web.UI;
using Phatra.Core.Managers;
using Phatra.Core.Utilities;
using System.Text;
using Phatra.Core.Web.Infrastructure;
using Phatra.Core.Logging;

namespace System.Web.Mvc
{
    /// <summary>
    /// Controller extension class that adds controller methods
    /// to render a partial view and return the result as string.
    /// 
    /// Based on http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
    /// </summary>
    public static class ControllerExtension
    {

        /// <summary>
        /// Renders a (partial) view to string.
        /// </summary>
        /// <param name="controller">Controller to extend</param>
        /// <param name="viewName">(Partial) view to render</param>
        /// <returns>Rendered (partial) view as string</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName)
        {
            return controller.RenderPartialViewToString(viewName, null);
        }

        /// <summary>
        /// Renders a (partial) view to string.
        /// </summary>
        /// <param name="controller">Controller to extend</param>
        /// <param name="viewName">(Partial) view to render</param>
        /// <param name="model">Model</param>
        /// <returns>Rendered (partial) view as string</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

    public static class ViewExtensions
    {
        private static readonly ILogger Log = WebContext.Instance.Current.Resolve<ILogger>();

        public static string RenderToString(this PartialViewResult partialView)
        {

            Log.Debug("Starting RenderToString ...");

            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");
            }

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            Log.Debug("Before (ControllerBase)ControllerBuilder.Current.GetControllerFactory()...");
            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(httpContext.Request.RequestContext, controllerName);

            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);

            Log.Debug("Before ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName)...");
            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    Log.Debug("Before view.Render ...");
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
                }
            }

            Log.Debug("Before vsb.ToString() ...");
            return sb.ToString();
        }
    }


    public static partial class HtmlExtension
    {
        public static string GetDatabaseConfiguraion(this HtmlHelper helper)
        {
            return ConfigurationHelper.GetAppSettingValue("DATABASE");
        }

        public static string GetWebCtrlConfiguraion(this HtmlHelper helper)
        {
            return ConfigurationHelper.GetAppSettingValue("WebCtrlDB");
        }
        public static string GetCISUIConfiguraion(this HtmlHelper helper)
        {
            return ConfigurationHelper.GetAppSettingValue("CISUIDB");
        }

        public static string GetMenuStep(this HtmlHelper helper)
        {
            return helper.ViewContext.RequestContext.HttpContext.Request.Url == null ? string.Empty : WebCtrlManager.Instance.GetMenuStep(helper.ViewContext.RequestContext.HttpContext.Request.Url.ToString());
        }

        public static string GetPageLabel(this HtmlHelper helper)
        {
            return helper.ViewContext.RequestContext.HttpContext.Request.Url == null ? string.Empty : WebCtrlManager.Instance.GetPageLabel(helper.ViewContext.RequestContext.HttpContext.Request.Url.ToString());
        }

        public static string GetUserLogin(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.User.Identity.Name;
        }

        public static string GetCurrentUserFullName(this HtmlHelper helper)
        {
            return WebCtrlManager.Instance.GetCurrentUserFullName();
        }

        public static bool IsProduction(this HtmlHelper helper)
        {
            return WebCtrlManager.Instance.IsProduction();
        }

        public static bool IsUat(this HtmlHelper helper)
        {
            return WebCtrlManager.Instance.IsUat();
        }
    }
}
