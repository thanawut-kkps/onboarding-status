using log4net.Layout.Pattern;
using log4net.Core;
using System.Web;
using System.IO;

namespace Phatra.Core.Utilities.Log4net
{
    public class HttpContextUserPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            string name = "";
            HttpContext httpContext = HttpContext.Current;


            if (httpContext == null)
            {
                var request = new HttpRequest("/", "http://localhost:19509/", "");
                var response = new HttpResponse(new StringWriter());
                httpContext = new HttpContext(request, response);
            }

            if (httpContext != null && httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                name = httpContext.User.Identity.Name;
            }
            writer.Write(name);
        }
    }
}
