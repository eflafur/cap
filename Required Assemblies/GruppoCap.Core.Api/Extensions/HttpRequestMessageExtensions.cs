using System.Net.Http;
using System.Web;

namespace GruppoCap.Core.Api
{
    public static class Extensions
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            return null;
        }
    }
}
