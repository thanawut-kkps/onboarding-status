using System;
using System.Web.Mvc;
using System.Linq;
using Phatra.Core.Exceptions;
using Phatra.Core.Logging;

namespace Phatra.Core.Web.Mvc
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger = Log4NetLogger.CreateLoggerFor(typeof(BaseController));

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAjaxRequest() && !filterContext.IsChildAction)
            {
                var actionName = (string)filterContext.RouteData.Values["Action"];
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                _logger.Debug($"[{User.Identity.Name}] is executing action {controllerName}.{actionName}");
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Debug($"************* Start OnException ({User.Identity.Name}) ***************************");

            var errorMessage = ExtractExceptionToErrorMessage(filterContext.Exception);

            if (Request.IsAjaxRequest() && !filterContext.IsChildAction)
            {
                string message;
                if (IsBusinessException(filterContext.Exception))
                {
                    _logger.Debug("Error is Business Exception.");
                    //message = this.RenderPartialViewToString(this.ModelStateInvalidViewName);
                    message = errorMessage;
                    _logger.Debug($"HTML String view render to browser is::{message}");
                }
                else
                {
                    _logger.Debug("Error is Unhandle or Technical Exception .");
                    //message = this.RenderPartialViewToString(this.UnHandlerErrorDetailViewName);
                    message = errorMessage;
                    _logger.Debug($"HTML String view render to browser is::{message}");
                }

                //filterContext.Result= Json(new { valid = false, message = message });
                filterContext.Result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { valid = false, message } };
            }
            else
            {
                string viewName = ViewBag.ViewName;
                var model = ViewBag.Model;
                if (IsBusinessException(filterContext.Exception))
                {
                    _logger.Debug("No Ajax Request Error is Business Exception.");
                    if (viewName == null || model == null)
                    {
                        filterContext.Result = BusinessErrorDetailView(filterContext.Controller, filterContext.Exception);
                    }
                    else
                    {
                        filterContext.Result = View(viewName, model);
                    }
                }
                else
                {
                    var e = new UnhandleErrorViewModel()
                    {
                        UserName = filterContext.HttpContext.User.Identity.Name,
                        OriginalErrorMessage = filterContext.Exception.Message,
                        DetailError = filterContext.Exception.ToString()
                    };

                    _logger.Debug("No Ajax Request Error is Unhandle or Technical Exception .");
                    _logger.Debug($"ErrorMessage is: {errorMessage}");

                    filterContext.Result = GetProperView(filterContext, UnHandlerErrorDetailViewName, e);      
                }
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            _logger.Debug("************* End OnException ***************************");

            base.OnException(filterContext);

        }

        //private ActionResult GetProperView(ControllerContext filterContext, string viewName)
        //{
        //    return GetProperView(filterContext, viewName, null);
        //}

        private ActionResult GetProperView(ControllerContext filterContext, string viewName, dynamic model)
        {
            return filterContext.IsChildAction ? (ActionResult) PartialView(viewName, model) : (ActionResult) View(viewName, model);
        }

        protected bool IsBusinessException(object exception)
        {
            if (exception.GetType() == typeof(BaseBusinessException)) return true;
            if (exception.GetType().BaseType == typeof(BaseBusinessException)) return true;
            return exception is BaseBusinessException;
        }

        protected ActionResult ModelStateInValid(string viewName, object model)
        {
            if (Request.IsAjaxRequest())
            {
                AddModelStateError();
                return Json(new { valid = false, message = this.RenderPartialViewToString("_ModelStateInvalid", model) });
            }
            return View(viewName, model);
        }

        protected ActionResult ModelStateInValid(object model)
        {
            return ModelStateInValid(RouteData.Values["action"].ToString(), model);
        }

        protected virtual void AddModelStateError()
        {
            var modelError = (from m in ModelState where m.Value.Errors.Count > 0 select m).ToList();
            foreach (var model in modelError)
            {
                if (model.Key == string.Empty) continue;
                var error = ModelState[model.Key].Errors;
                var countError = error.Count;
                _logger.Debug("The invalid data are the following:");
                _logger.Debug($"{model.Key} Error is ");
                for (int i = 0; i < countError; i++)
                {
                    _logger.Debug(error[i].ErrorMessage);
                    ModelState.AddModelError(string.Empty, error[i].ErrorMessage);
                }
            }
        }

        protected string ExtractExceptionToErrorMessage(Exception exception)
        {
            string errorMessage;
            if (IsBusinessException(exception))
            {
                var buEx = (BaseBusinessException)exception;
                errorMessage = buEx.ErrorMessage;
                _logger.Debug("BusinessException: " + buEx.ErrorMessage);
                _logger.Debug("Exception.ToString(): " + exception);
            }
            else
            {
                //Other error or Technical error
                errorMessage = exception.Message;

                //Logger.Error("Other Exception: " + errorMessage);
                _logger.Error($"Exception.ToString(): {exception.ToString()}");
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            AddModelStateError();

            return errorMessage;
        }

        protected abstract ActionResult BusinessErrorDetailView(ControllerBase controller, Exception exception);

        protected abstract string UnHandlerErrorDetailViewName
        {
            get;
        }

        protected abstract string ModelStateInvalidViewName
        {
            get;
        }

        //private void CompresssResponse(ActionExecutedContext filterContext)
        //{
        //    HttpRequestBase request = filterContext.HttpContext.Request;

        //    string acceptEncoding = request.Headers["Accept-Encoding"];

        //    if (string.IsNullOrEmpty(acceptEncoding)) return;

        //    acceptEncoding = acceptEncoding.ToUpperInvariant();

        //    HttpResponseBase response = filterContext.HttpContext.Response;

        //    if (acceptEncoding.Contains("GZIP"))
        //    {
        //        response.AppendHeader("Content-encoding", "gzip");
        //        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
        //    }
        //    else if (acceptEncoding.Contains("DEFLATE"))
        //    {
        //        response.AppendHeader("Content-encoding", "deflate");
        //        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
        //    }
        //}

    }
}
