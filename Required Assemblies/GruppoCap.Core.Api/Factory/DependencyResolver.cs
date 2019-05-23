using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace GruppoCap.Core.Api
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Castle.MicroKernel.IKernel kernel;

        public DependencyResolver(Castle.MicroKernel.IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyScope(kernel);
        }

        public object GetService(Type type)
        {
            return kernel.HasComponent(type) ? kernel.Resolve(type) : null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return kernel.ResolveAll(type).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}