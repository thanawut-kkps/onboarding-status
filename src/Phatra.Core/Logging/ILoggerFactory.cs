using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phatra.Core.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLoggerFor(string name);

        ILogger CreateLoggerFor(Type type);
    }
}
