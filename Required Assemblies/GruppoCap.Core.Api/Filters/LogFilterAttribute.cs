using GruppoCap.Logging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Filters
{
    public class LogFilterAttribute : ActionFilterWithOrderAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var _log = actionContext.Request.GetDependencyScope().GetService(typeof(ILogger)) as ILogger;
            var content = actionContext.Request.Content.ReadAsStringAsync().Result;

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
                "LogFilterAttribute - OnActionExecuting",
                new object[] {
                    "{0}: {1}".FormatWith("Method", actionContext.Request.Method),
                    "{0}: {1}".FormatWith("RequestUri", actionContext.Request.RequestUri),
                    "{0}: {1}".FormatWith("Authorization", actionContext.Request.Headers.Authorization),
                    "{0}: {1}".FormatWith("ClientIpAddress", actionContext.Request.GetClientIpAddress()),
                    "{0}: {1}".FormatWith("content", content)
                }
            );
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var _log = actionExecutedContext.Request.GetDependencyScope().GetService(typeof(ILogger)) as ILogger;
            bool logContent = !(actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<NoOnExecutedContentLog>().Any()
                || actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<NoOnExecutedContentLog>().Any());

            var content = logContent ? actionExecutedContext.ActionContext.Response?.Content?.ReadAsStringAsync().Result : "Log Content excluded by attributes";
            _log.Append(string.Format("{0}/{1}",
            actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
            actionExecutedContext.ActionContext.ActionDescriptor.ActionName),
            LogLevel.Trace,
            null,
            "LogFilterAttribute - OnActionExecuted",
            new object[] {
                    "{0}: {1}".FormatWith("Method", actionExecutedContext.ActionContext.Request.Method),
                    "{0}: {1}".FormatWith("StatusCode", actionExecutedContext.ActionContext.Response?.StatusCode),
                    "{0}: {1}".FormatWith("RequestUri", actionExecutedContext.ActionContext.Request.RequestUri),
                    "{0}: {1}".FormatWith("ClientIpAddress", actionExecutedContext.ActionContext.Request.GetClientIpAddress()),
                    "{0}: {1}".FormatWith("content", content)
                }
            );
        }

    }
    public class NoOnExecutedContentLog : ActionFilterWithOrderAttribute
    {
    }
}