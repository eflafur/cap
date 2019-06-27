using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GruppoCap.Core
{
    public class CastleWindsorGenericTypeResolver
        : IGenericTypeResolver
    {

        // PRIVATE MEMBERs
        private IWindsorContainer _Container = null;

        #region " CTORs "

        // CTOR
        public CastleWindsorGenericTypeResolver()
        {
            // IOC CONTAINER - SETUP
            try
            {
                _Container = new WindsorContainer(new Castle.Windsor.Configuration.Interpreters.XmlInterpreter());
            }
            catch (ConfigurationErrorsException)
            {
                _Container = new WindsorContainer();
            }
            catch (Exception)
            {
                throw;
            }

            // IOC CONTAINER REGISTRATION
            _Container.Register(
                Component
                .For<IWindsorContainer>()
                .Named("__IOC__")
                .Instance(_Container)
            );

            _Container.Register(
                Component
                .For<IGenericTypeResolver>()
                .Named("generictyperesolver")
                .Instance(this)
            );
        }

        #endregion

        #region IGenericTypeResolver Members

        // RESOLVE INSTANCE
        public T ResolveInstance<T>() where T : class
        {
            return this._Container.Resolve<T>();
        }

        // RESOLVE INSTANCE
        public T ResolveInstance<T>(Type instanceType) where T : class
        {
            return this._Container.Resolve(instanceType) as T;
        }

        // RESOLVE ALL INSTANCES
        public List<T> ResolveAllInstances<T>() where T : class
        {
            return this._Container.ResolveAll<T>().ToList<T>();
        }

        // RESOLVE ALL INSTANCES
        public List<T> ResolveAllInstances<T>(Type instanceType) where T : class
        {
            return this._Container.ResolveAll(instanceType).Cast<T>().ToList<T>();
        }

        #endregion

        #region IDisposable Members

        // DISPOSE
        public void Dispose()
        {
            this._Container.Dispose();
        }

        #endregion

        // WINDSOR CONTAINER
        public IWindsorContainer WindsorContainer
        {
            get { return _Container; }
        }

    }
}
