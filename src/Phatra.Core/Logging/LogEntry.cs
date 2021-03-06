using System;

namespace Phatra.Core.Logging
{
    public enum LoggingEventType { Debug, Information, Warning, Error, Fatal }

    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }
}
