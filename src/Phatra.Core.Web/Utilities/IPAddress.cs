using System.Web;
using System;
namespace Phatra.Core.Web.Utilities
{
    public class IPAddress
    {
        public static string GetClientIPAddress()
        {
            string IPAddress = string.Empty;
            IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!String.IsNullOrEmpty(IPAddress))
                return IPAddress.Split(',')[0];


            //While it can't get the Client IP, it will return proxy IP.
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
