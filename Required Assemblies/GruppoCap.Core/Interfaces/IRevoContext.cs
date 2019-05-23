using GruppoCap.Core.Activity;
using GruppoCap.Core.Caching;
using GruppoCap.Core.Identity;
using GruppoCap.Core.Permission;
using GruppoCap.Logging;
using System;
using System.Collections.Generic;

namespace GruppoCap.Core
{
    public interface IRevoContext
    {
        IGenericTypeResolver GenericTypeResolver { get; }

        IDictionary<String, Object> ExtraData { get; }

        IRevoServiceProvider ServiceProvider { get; }

        IIdentityManager IdentityManager { get; }
        
        IActivityManager ActivityManager { get; }
        
        IPermissionManager PermissionManager { get; }
        
        ILoggerScope ContextLogger { get; }

        ICache CacheProvider { get; }
    }
}
