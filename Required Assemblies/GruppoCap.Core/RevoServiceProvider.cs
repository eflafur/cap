using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public class RevoServiceProvider
        : IRevoServiceProvider
    {
        // PRIVATE MEMBERs
        private IGenericTypeResolver _genericTypeResolver = null;
        private IDictionary<String, Object> _serviceStore = new Dictionary<String, Object>();

        public String GetServiceDictionaryKey(Type entityType)
        {
            return "revo.serviceprovider." + entityType.FullName;
        }

        #region " CTORs "

        // CTOR
        public RevoServiceProvider(IGenericTypeResolver genericTypeResolver)
        {
            _genericTypeResolver = genericTypeResolver;
        }

        #endregion

        // SERVICE STORE
        public IDictionary<String, Object> ServiceStore
        {
            get { return _serviceStore; }
        }

        // SET SERVICE
        public void SetService(Type entityType, Object service)
        {
            String key;

            // GET KEY
            key = this.GetServiceDictionaryKey(entityType);

            this.ServiceStore[key] = service;
        }

        // GET SERVICE
        public IRevoService GetService(Type entityType)
        {

            String key;
            IRevoService s1;

            // GET KEY
            key = this.GetServiceDictionaryKey(entityType);

            if (this.ServiceStore.ContainsKey(key) == false)
                throw new KeyNotFoundException("ServiceStore doesn't contain the requested service. Did you call the \"RegisterService\" in \"Application_Start\" ??");

            // TRY TO GET THE SERVICE
            s1 = this.ServiceStore[key] as IRevoService;

            return s1;
        }

        // INDEXER
        public IRevoService this[Type entityType]
        {
            get { return this.GetService(entityType); }
        }

        // GET SERVICE FOR
        public IRevoService GetServiceFor<E>()
            where E : class, IEntity
        {
            return this.GetService(typeof(E)) as IRevoService;
        }

    }
}
