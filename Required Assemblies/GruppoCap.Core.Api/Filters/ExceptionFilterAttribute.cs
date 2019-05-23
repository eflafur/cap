using GruppoCap.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterWithOrderAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            var _log = context.Request.GetDependencyScope().GetService(typeof(ILogger)) as ILogger;
            var content = context.Request.Content.ReadAsStringAsync().Result;

            _log.Append(string.Format("OnException - {0}/{1}",
                context.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                context.ActionContext.ActionDescriptor.ActionName),
                LogLevel.Error,
                context.Exception,
                context.Exception?.Message,
                new object[] {
                    "{0}: {1}".FormatWith("Method", context.Request.Method),
                    "{0}: {1}".FormatWith("RequestUri", context.Request.RequestUri),
                    "{0}: {1}".FormatWith("Authorization", context.Request.Headers.Authorization),
                    "{0}: {1}".FormatWith("ClientIpAddress", context.Request.GetClientIpAddress()),
                    "{0}: {1}".FormatWith("content", content)
                    }
                );

            if (context.Exception is NotImplementedException)
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, context.Exception);
            else
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, context.Exception);
        }
    }
}