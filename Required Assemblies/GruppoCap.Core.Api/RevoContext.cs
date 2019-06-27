using GruppoCap.Core.Activity;
using GruppoCap.Core.Caching;
using GruppoCap.Core.Identity;
using GruppoCap.Core.Permission;
using GruppoCap.Logging;
using GruppoCap.Logging.Common;
using System;
using System.Collections.Generic;

namespace GruppoCap.Core.Api
{
    public class RevoContext : IRevoContext
    {
        // PRIVATE MEMBERs
        private IGenericTypeResolver _genericTypeResolver = null;
        private IRevoServiceProvider _serviceProvider = null;
        private IDictionary<String, Object> _extraData = new Dictionary<String, Object>();
        private ICache _cacheProvider = null;

        private ILoggerScope _Logger = null;

        // EXTRA DATA
        public IDictionary<String, Object> ExtraData
        {
            get { return _extraData; }
        }

        // GENERIC TYPE RESOLVER
        public IGenericTypeResolver GenericTypeResolver
        {
            get { return this._genericTypeResolver; }
        }

        public RevoContext(
            IGenericTypeResolver genericTypeResolver
            , IRevoServiceProvider serviceProvider
            , IIdentityManager identityManager
            //, IActivityManager activityManager
            //, IPermissionManager permissionManager
            , ILogger logger
            , ICache cacheProvider
        )
        {
            _genericTypeResolver = genericTypeResolver;
            
            _serviceProvider = serviceProvider;

            IdentityManager = identityManager;

            //ActivityManager = activityManager;

            //PermissionManager = permissionManager;

            _Logger = logger.CreateScope("REVO-API-CTX-{0}".FormatWith(Ambient.Current));

            _cacheProvider = cacheProvider;
        }

        // SERVICE PROVIDER
        public IRevoServiceProvider ServiceProvider { get { return this._serviceProvider; } }

        public ICache CacheProvider { get { return this._cacheProvider; } }

        //MANAGERS
        public IIdentityManager IdentityManager { get; protected set; }

        public IActivityManager ActivityManager { get; protected set; }

        public IPermissionManager PermissionManager { get; protected set; }

        // CONTEXT LOGGER
        public ILoggerScope ContextLogger
        {
            get
            {
                if (_Logger == null)
                {
                    try
                    {
                        // FIRST, TRY TO RESOLVE A LOGGER USING IOC
                        _Logger = GenericTypeResolver.ResolveInstance<ILogger>().CreateScope("REVO-API-CTX-{0}".FormatWith(Ambient.Current));
                    }
                    catch (Exception)
                    {
                        // USE THE DEFAULT SHARED ONE
                        _Logger = DummyLogger.SharedDefaultLogger.CreateScope("REVO-API-CTX-{0}".FormatWith(Ambient.Current));
                    }
                }

                return _Logger;
            }
        }

        
    }
}
