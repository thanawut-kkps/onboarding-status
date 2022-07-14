using Phatra.Core.Exceptions;
using Phatra.Core.Logging;
using Phatra.Core.Web.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;

namespace Phatra.Core.Web.Mvc
{
    public abstract class BaseHttpApplication : HttpApplication
    {
        readonly ILogger _logger = Log4NetLogger.CreateLoggerFor(typeof(BaseHttpApplication));

        protected virtual void Application_Start()
        {
            _logger.Info("Start Application...");

            WebContext.Instance.Initialize(true);
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                _logger.Info($"BaseHttpApplication.Application_Error: {exception.ToString()}");
            }
            //Server.ClearError();
        }

        protected void HandleError(Exception exception)
        {
            var errorMessage = ExtractExceptionToErrorMessage(exception);
            var requestWrapper = new HttpRequestWrapper(HttpContext.Current.Request);
            var isAjaxRequest = requestWrapper.IsAjaxRequest();

        }

        protected bool IsBusinessException(object exception)
        {
            if (exception.GetType() == typeof(BaseBusinessException)) return true;
            if (exception.GetType().BaseType == typeof(BaseBusinessException)) return true;
            if (exception is BaseBusinessException) return true;
            return false;
        }

        protected string ExtractExceptionToErrorMessage(Exception exception)
        {
            string errorMessage;
            if (IsBusinessException(exception))
            {
                BaseBusinessException buEx = (BaseBusinessException)exception;
                errorMessage = buEx.ErrorMessage;
                _logger.Debug($"BusinessException: {buEx.ErrorMessage}");
                _logger.Debug($"Exception.ToString(): {exception}");
            }
            else
            {
                //Other error or Technical error
                errorMessage = exception.Message;

                //Logger.Error("Other Exception: " + errorMessage);
                _logger.Error($"Exception.ToString(): {exception}");
            }
            return errorMessage;
        }


    }
}
