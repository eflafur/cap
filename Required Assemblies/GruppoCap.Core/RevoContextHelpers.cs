using GruppoCap.Logging;
using GruppoCap.Logging.Common;
using System;
using System.Web;

namespace GruppoCap.Core
{
    public static class RevoContextHelpers
    {
        // CONSTANTs
        public const String Const_RevoContext_AppKey = "$revolution.revo-context";
        public const String Const_RevoWebRequest_AppKey = "$revolution.revo-web-request";

        #region " REVO CONTEXT STUFF "

        // GET REVO CONTEXT
        public static IRevoContext GetRevoContext(HttpApplicationState appState)
        {
            try
            {
                return appState.Get(Const_RevoContext_AppKey) as IRevoContext;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // SET REVO CONTEXT
        public static void SetRevoContext(HttpApplicationState appState, IRevoContext revoContext)
        {
            appState.Set(Const_RevoContext_AppKey, revoContext);
        }

        // GET CURRENT REVO CONTEXT
        public static IRevoContext GetCurrentRevoContext()
        {
            if (HttpContext.Current == null || HttpContext.Current.Application == null)
                return null;

            return GetRevoContext(System.Web.HttpContext.Current.Application);
        }

        // SET CURRENT REVO CONTEXT
        public static void SetCurrentRevoContext(IRevoContext revoContext)
        {
            SetRevoContext(System.Web.HttpContext.Current.Application, revoContext);
        }

        // GET EXTRA DATA AS
        public static T GetExtraDataAs<T>(this IRevoContext ctx, String extendedDataId)
        {
            return (T)ctx.ExtraData[extendedDataId];
        }

        // REGISTER SERVICE
        public static void RegisterService<E, S>(this IRevoContext ctx)
            where E : class, IEntity
            where S : class, IRevoService
        {
            ctx.ServiceProvider.SetService(typeof(E), ctx.GenericTypeResolver.ResolveInstance<S>());
        }

        // REGISTER PERMISSION FOR
        public static void RegisterPermissionFor<E>(this IRevoContext ctx)
            where E : class, IEntity
        {
            ctx.PermissionManager.EnsureEntityPermissionSetExistence(typeof(E));
        }

        // REGISTER PERMISSION FOR
        public static void RegisterPermissionFor<E>(this IRevoContext ctx, Boolean defaultGrant, Boolean isPrivileged)
            where E : class, IEntity
        {
            ctx.PermissionManager.EnsureEntityPermissionSetExistence(typeof(E), defaultGrant, isPrivileged);
        }

        // REGISTER PERMISSION
        public static void RegisterPermission(this IRevoContext ctx, String permissionCode, String categoryName = "", Boolean defaultGrant = false, Boolean isPriviledge = false)
        {
            ctx.PermissionManager.EnsurePermissionExistence(permissionCode, categoryName, defaultGrant, isPriviledge);
        }

        #endregion

        #region " REVO WEB REQUEST STUFF "

        // GET REVO WEB REQUEST
        public static IRevoWebRequest GetRevoWebRequest(HttpContext context)
        {
            try
            {
                return context.Items[Const_RevoWebRequest_AppKey] as IRevoWebRequest;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // SET REVO WEB REQUEST
        public static void SetRevoWebRequest(HttpContext context, IRevoWebRequest revoWebRequest)
        {
            if (revoWebRequest == null)
            {
                context.Items.Remove(Const_RevoWebRequest_AppKey);
                return;
            }

            context.Items[Const_RevoWebRequest_AppKey] = revoWebRequest;
        }

        // GET CURRENT REVO WEB REQUEST
        public static IRevoWebRequest GetCurrentRevoWebRequest()
        {
            return GetRevoWebRequest(System.Web.HttpContext.Current);
        }

        // SET CURRENT REVO WEB REQUEST
        public static void SetCurrentRevoWebRequest(IRevoWebRequest revoWebRequest)
        {
            SetRevoWebRequest(System.Web.HttpContext.Current, revoWebRequest);
        }

        // GET EXTRA DATA AS
        public static T GetExtraDataAs<T>(this IRevoWebRequest revoWebRequest, String extendedDataId)
        {
            return (T)revoWebRequest.ExtraData[extendedDataId];
        }

        #endregion

        #region LOGGING

        // CREATE SCOPE
        public static ILoggerScope CreateScope(this ILogger logger, String scope)
        {
            return new LoggerScope(logger, scope);
        }

        // CREATE SCOPE FOR TYPE OF INSTANCE
        public static ILoggerScope CreateScopeForTypeOfInstance(this ILogger logger, Object o)
        {
            return new LoggerScope(logger, o == null ? "Ooouch! No type infos" : o.GetType().FullName);
        }

        // TRACE
        public static void Trace(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.TraceEx(null, message, parameters);
        }

        // DEBUG
        public static void Debug(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.DebugEx(null, message, parameters);
        }

        // INFO
        public static void Info(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.InfoEx(null, message, parameters);
        }

        // WARN
        public static void Warn(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.WarnEx(null, message, parameters);
        }

        // ERROR
        public static void Error(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.ErrorEx(null, message, parameters);
        }

        // PANIC
        public static void Panic(this ILoggerScope loggerScope, String message, params Object[] parameters)
        {
            loggerScope.PanicEx(null, message, parameters);
        }

        // TRACE EX
        public static void TraceEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Trace, exceptionOrNull, message, parameters);
        }

        // DEBUG EX
        public static void DebugEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Debug, exceptionOrNull, message, parameters);
        }

        // INFO EX
        public static void InfoEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Info, exceptionOrNull, message, parameters);
        }

        // WARN EX
        public static void WarnEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Warn, exceptionOrNull, message, parameters);
        }

        // ERROR EX
        public static void ErrorEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Error, exceptionOrNull, message, parameters);
        }

        // PANIC EX
        public static void PanicEx(this ILoggerScope loggerScope, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            loggerScope.Append(LogLevel.Panic, exceptionOrNull, message, parameters);
        }

        // IS TRACE ENABLED
        public static Boolean IsTraceEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Trace);
        }

        // IS DEBUG ENABLED
        public static Boolean IsDebugEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Debug);
        }

        // IS INFO ENABLED
        public static Boolean IsInfoEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Info);
        }

        // IS WARN ENABLED
        public static Boolean IsWarnEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Warn);
        }

        // IS ERROR ENABLED
        public static Boolean IsErrorEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Error);
        }

        // IS PANIC ENABLED
        public static Boolean IsPanicEnabled(this ILoggerScope loggerScope)
        {
            return loggerScope.IsLogLevelEnabled(LogLevel.Panic);
        }

        #endregion
    }
}
