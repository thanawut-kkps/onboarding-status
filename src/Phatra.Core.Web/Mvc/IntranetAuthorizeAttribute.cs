using System.Web.Mvc;
using Phatra.Core.Managers;

namespace Phatra.Core.Web.Mvc
{
    public class IntranetAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!WebCtrlManager.Instance.HasPagePrivilage(filterContext.HttpContext.Request.Url.ToString()))
            {
                var noPrivilegePage = WebCtrlManager.Instance.GetNoPrivilegePage();
                filterContext.Result = new RedirectResult(noPrivilegePage);
            }
        }
    }
}
