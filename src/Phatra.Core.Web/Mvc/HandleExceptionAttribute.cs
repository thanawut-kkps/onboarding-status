using Phatra.Core.Exceptions;
using Phatra.Core.Logging;
using System;
using System.Web.Mvc;

namespace Phatra.Core.Web.Mvc
{
    public sealed class HandleExceptionAttribute : HandleErrorAttribute
    {
        private string _defaultUnHandlerErrorDetailViewName = "Error";
        private readonly ILogger _logger;

        public HandleExceptionAttribute() : this(string.Empty)
        {
        }

        public HandleExceptionAttribute(string unHandlerErrorDetailViewName)
        {
            _logger = Log4NetLogger.CreateLoggerFor(GetType());

            UnHandlerErrorDetailViewName = !string.IsNullOrWhiteSpace(unHandlerErrorDetailViewName) ? unHandlerErrorDetailViewName : _defaultUnHandlerErrorDetailViewName;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string errorMessage = ExtractExceptionToErrorMessage(filterContext.Exception);
                _logger.Debug(errorMessage);

                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];

                if (!IsAjaxRequest(filterContext))
                {
                    var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                    filterContext.ExceptionHandled = true;
                    filterContext.Result = GetProperView(filterContext, UnHandlerErrorDetailViewName, model);                    
                }                                         
            }
        }

        private string UnHandlerErrorDetailViewName { get; }

        private static bool IsAjaxRequest(ControllerContext filterContext)
        {
            return filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }

        private string ExtractExceptionToErrorMessage(Exception exception)
        {
            string errorMessage;
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

                //_logger.Error("Other Exception: " + errorMessage);
                _logger.Error("Exception.ToString(): " + exception);
            }

            return errorMessage;
        }

        private static bool IsBusinessException(object exception)
        {
            if (exception.GetType() == typeof(BaseBusinessException)) return true;
            if (exception.GetType().BaseType == typeof(BaseBusinessException)) return true;
            return exception is BaseBusinessException;
        }

        private static ActionResult GetProperView(ControllerContext exceptionContext, string viewName, dynamic model)
        {
            var viewData = new ViewDataDictionary {Model = model};

            if (exceptionContext.IsChildAction)
            {
                return new PartialViewResult() { ViewName = viewName, ViewData = viewData, TempData = exceptionContext.Controller.TempData  };
            }

            return new ViewResult() {  ViewName = viewName , ViewData = viewData };
        }
    }
}
