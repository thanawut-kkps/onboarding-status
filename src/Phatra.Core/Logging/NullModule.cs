using System;

namespace Phatra.Core.Logging
{
    public class NullModule : BaseLoggingModule
    {
        protected override ILogger CreateLoggerFor(Type type)
        {
            return new NullLogger(type);
        }
    }
}
