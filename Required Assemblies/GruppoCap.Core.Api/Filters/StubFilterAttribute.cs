using GruppoCap;
using GruppoCap.Core.Api;
using GruppoCap.Core.Api.Filters;
using GruppoCap.Logging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace GruppoCap.Core.Api.Filters
{
    public class StubFilterAttribute : ActionFilterWithOrderAttribute
    {
        private HttpStatusCode StatusCode { get; set; }
        private string Message { get; set; }

        public StubFilterAttribute(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public StubFilterAttribute(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<NoStubFilterAttribute>().Any()
                && !actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<NoStubFilterAttribute>().Any())
            {
                //stub
                if (string.IsNullOrEmpty(Message))
                    actionContext.Response = actionContext.Request.CreateResponse(StatusCode, new BaseApiResponse() { Result = true });
                else
                    actionContext.Response = actionContext.Request.CreateResponse(StatusCode, new BaseApiResponse() { Result = StatusCode == HttpStatusCode.OK ? true : false, ErrorCode = "Stub", ErrorMessage = Message });

                var _log = actionContext.Request.GetDependencyScope().GetService(typeof(ILogger)) as ILogger;
                var content = actionContext.Response.Content?.ReadAsStringAsync().Result;

                try
                {
                    if (content.IsNullOrWhiteSpace())
                    {
                        Stream reqStream = actionContext.Request.Content.ReadAsStreamAsync().Result;
                        if (reqStream.CanSeek)
                            reqStream.Position = 0;

                        StreamReader reader = new StreamReader(reqStream);
                        content = reader.ReadToEnd();
                    }
                }
                catch
                {
                    // EMPTY BY DEFAULT... 
                }

                _log.Append(string.Format("{0}/{1}",
                    actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    actionContext.ActionDescriptor.ActionName),
                    LogLevel.Trace,
                    null,
                    "StubFilterAttribute - OnActionExecuting",
                    new object[] {
                        "{0}: {1}".FormatWith("Method", actionContext.Request.Method),
                        "{0}: {1}".FormatWith("StatusCode", actionContext.Response.StatusCode),
                        "{0}: {1}".FormatWith("RequestUri", actionContext.Request.RequestUri),
                        "{0}: {1}".FormatWith("ClientIpAddress", actionContext.Request.GetClientIpAddress()),
                        "{0}: {1}".FormatWith("content", content)
                    }
                );
            }
        }
    }
    public class NoStubFilterAttribute : ActionFilterWithOrderAttribute
    {
    }
}