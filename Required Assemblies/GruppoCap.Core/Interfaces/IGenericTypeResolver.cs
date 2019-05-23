using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IGenericTypeResolver
        : IDisposable
    {

        // RESOLVE INSTANCE
        T ResolveInstance<T>() where T : class;

        // RESOLVE INSTANCE
        T ResolveInstance<T>(Type instanceType) where T : class;

        // RESOLVE ALL INSTANCES
        List<T> ResolveAllInstances<T>() where T : class;

        // RESOLVE ALL INSTANCES
        List<T> ResolveAllInstances<T>(Type instanceType) where T : class;

    }
}
