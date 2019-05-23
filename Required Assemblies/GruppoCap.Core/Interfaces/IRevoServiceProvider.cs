using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IRevoServiceProvider
    {
        

        // SERVICE STORE
        IDictionary<String, Object> ServiceStore { get; }


        void SetService(Type entityType, object service);

        // GET SERVICE DICTIONARY KEY
        String GetServiceDictionaryKey(Type entityType);

        // GET SERVICE FOR
        IRevoService GetServiceFor<E>() where E : class, IEntity;

        //// GET SERVICE
        //IRevoService GetService(Type serviceInterfaceType);

        //// GET SERVICE FOR
        //IRevoService GetServiceFor(Type entityType);
    }
}
