using System;
using log4net;
using log4net.Config;

namespace Phatra.Core.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _adaptee;

        static Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        public Log4NetLogger(ILog adaptee)
        {
            _adaptee = adaptee;
        }

        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Debug)
                _adaptee.Debug(entry.Message);
            else if (entry.Severity == LoggingEventType.Information)
                _adaptee.Info(entry.Message);
            else if (entry.Severity == LoggingEventType.Warning)
                _adaptee.Warn(entry.Message);
            else if (entry.Severity == LoggingEventType.Error)
                _adaptee.Error(entry.Message, entry.Exception);
            else
                _adaptee.Fatal(entry.Message, entry.Exception);
        }

        public static ILogger CreateLoggerFor(Type type)
        {
            return new Log4NetLogger(LogManager.GetLogger(type));
        }

        public static ILogger CreateLoggerFor(string name)
        {
            return new Log4NetLogger(LogManager.GetLogger(name));
        }
    }
}
