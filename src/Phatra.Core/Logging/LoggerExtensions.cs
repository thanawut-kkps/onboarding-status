using System;

namespace Phatra.Core.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Debug, msg));
        }

        public static void Info(this ILogger logger, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, msg));
        }

        public static void Warn(this ILogger logger, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Warning, msg));
        }

        public static void Error(this ILogger logger, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, msg));
        }

        public static void Error(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));
        }

        public static void Error(this ILogger logger, Exception exception, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, msg, exception));
        }

        public static void Fatal(this ILogger logger, Exception exception, string msg)
        {
            logger.Log(new LogEntry(LoggingEventType.Fatal, msg, exception));
        }
    }
}
