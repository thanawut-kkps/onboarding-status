using log4net.Core;
using log4net.Layout.Pattern;
using System.Web;

namespace Phatra.Core.Web.Logging.log4net
{
    public class HttpContextUserPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            var name = "";
            var context = HttpContext.Current;
            if (context?.User != null && context.User.Identity.IsAuthenticated)
            {
                name = context.User.Identity.Name;
            }
            writer.Write(name);
        }
    }
}
