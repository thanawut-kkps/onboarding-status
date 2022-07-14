using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phatra.Core.Logging
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLoggerFor(Type type)
        {
            return new Log4NetLogger(LogManager.GetLogger(type));
        }

        public ILogger CreateLoggerFor(string name)
        {
            return new Log4NetLogger(LogManager.GetLogger(name));
        }
    }
}
