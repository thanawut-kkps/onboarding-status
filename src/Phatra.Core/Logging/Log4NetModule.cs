using System;

namespace Phatra.Core.Logging
{
    public class Log4NetModule : BaseLoggingModule
    {
        protected override ILogger CreateLoggerFor(Type type)
        {
            return Log4NetLogger.CreateLoggerFor(type);
        }
    }
}
