using System;

namespace Phatra.Core.Logging
{
    public class NullLogger : ILogger
    {
        private Type _type;
        public NullLogger(Type type)
        {
            _type = type;
        }

        public void Log(LogEntry entry)
        {
            // Do nothing
        }

        public static ILogger CreateLoggerFor(Type type)
        {
            return new NullLogger(type);
        }
    }
}
