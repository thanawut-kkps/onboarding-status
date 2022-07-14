using System.Reflection;
using System.Web.WebPages;

namespace System.Web.Mvc
{
    public static partial class HtmlExtension
    {
        public static string GetAssemblyVersion(this HtmlHelper helper)
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetAssemblyVersion(this HtmlHelper helper, Type type)
        {
            return type.Assembly.GetName().Version.ToString();
        }

        public static string GetWindowsUserName(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.User.Identity.Name; 
        }

        public static string GetVersionFromEnvironment(this HtmlHelper helper)
        {
            return Environment.Version.ToString();
        }

        /* ----------- Example for used -----
        _Layout.cshtml:
        <body>
        ...
        @Html.RenderScripts()
        </body>

        And somewhere in some template:
        @Html.Script(
            @<script src="@Url.Content("~/Scripts/jquery-1.4.4.min.js")" type="text/javascript"></script>
        )
        */
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }


    }
}
