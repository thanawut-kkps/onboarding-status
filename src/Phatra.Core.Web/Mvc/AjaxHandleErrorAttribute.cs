using Phatra.Core.Exceptions;
using Phatra.Core.Logging;
using System;
using System.Net;
using System.Web.Mvc;

namespace Phatra.Core.Web.Mvc
{
    public class AjaxHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly ILogger _logger;

        public AjaxHandleErrorAttribute()
        {
            _logger = Log4NetLogger.CreateLoggerFor(GetType());
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                //var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                _logger.Debug($"OnException at {controllerName}.{actionName}");

                var errorMessage = ExtractExceptionToErrorMessage(filterContext.Exception);
                if (IsAjaxRequest(filterContext))
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    filterContext.ExceptionHandled = true;

                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new
                        {
                            filterContext.Exception.Message,
                            filterContext.Exception.StackTrace
                        }
                    };

                    filterContext.Result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { valid = false, message = errorMessage } };
                }
            }
        }


        private bool IsAjaxRequest(ExceptionContext filterContext)
        {
            return filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }

        private bool IsBusinessException(object exception)
        {
            if (exception.GetType() == typeof(BaseBusinessException)) return true;
            if (exception.GetType().BaseType == typeof(BaseBusinessException)) return true;
            if (exception is BaseBusinessException) return true;
            return false;
        }

        private string ExtractExceptionToErrorMessage(Exception exception)
        {
            string errorMessage;
            //var message = string.Empty;
            if (IsBusinessException(exception))
            {
                BaseBusinessException buEx = (BaseBusinessException)exception;
                errorMessage = buEx.ErrorMessage;
                _logger.Debug("BusinessException: " + buEx.ErrorMessage);
                _logger.Debug("Exception.ToString(): " + exception);
            }
            else
            {
                //Other error or Technical error
                errorMessage = exception.Message;

                //Logger.Error("Other Exception: " + errorMessage);
                _logger.Error("Exception.ToString(): " + exception);
            }


            return errorMessage;
        }

    }
}
