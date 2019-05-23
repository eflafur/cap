using Castle.Windsor;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace GruppoCap.Core.Api
{
    public abstract class RevoApplication : System.Web.HttpApplication
    {
        // TRY REGISTER CONFIG FILE
        protected Boolean TryRegisterConfigFile(IWindsorContainer container, String configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                container.Install(Castle.Windsor.Installer.Configuration.FromXmlFile(configFilePath));
                return true;
            }

            return false;
        }

        // TRY REGISTER CONFIG FILEs
        protected Boolean TryRegisterConfigFiles(IWindsorContainer container, String baseConfigFilePath, params String[] variants)
        {
            Boolean res, tempRes;

            String variantConfigFilePath;

            res = false;

            foreach (String variant in variants)
            {
                if (String.IsNullOrEmpty(variant) == false)
                {
                    variantConfigFilePath = baseConfigFilePath.Replace(".config", String.Format(".{0}.config", variant));

                    tempRes = TryRegisterConfigFile(container, variantConfigFilePath);

                    res = res || tempRes;
                }
            }

            tempRes = TryRegisterConfigFile(container, baseConfigFilePath);

            res = res || tempRes;

            return res;
        }

        // TRY REGISTER CONFIG FILEs WITH AMBIENT
        protected Boolean TryRegisterConfigFilesWithAmbient(IWindsorContainer container, String baseConfigFilePath)
        {
            return TryRegisterConfigFiles(container, baseConfigFilePath, Ambient.Current);
        }

        // TRY REGISTER CORE CONFIG FILEs WITH AMBIENT
        protected Boolean TryRegisterCoreConfigFilesWithAmbient(IWindsorContainer container, String coreFileInnerName)
        {
            String filePath;

            filePath = Server.MapPath(String.Format("~/Configuration/castle.{0}.config", coreFileInnerName));

            return TryRegisterConfigFiles(container, filePath, Ambient.Current);
        }


        // GET GENERIC TYPE RESOLVER
        public IGenericTypeResolver GetGenericTypeResolver()
        {
            return Activator.CreateInstance(Type.GetType(GenericTypeResolverComponentName)) as IGenericTypeResolver;
        }

        // GENERIC TYPE RESOLVER COMPONENT NAME
        public static String GenericTypeResolverComponentName
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("Revo.IoC.GenericTypeResolverComponentName") == false)
                    throw new ConfigurationErrorsException("Revolution error: the IoC Generic Type Resolver Component Name is not present");

                if (ConfigurationManager.AppSettings["Revo.IoC.GenericTypeResolverComponentName"].IsNullOrWhiteSpace())
                    throw new ConfigurationErrorsException("Revolution error: the IoC Generic Type Resolver Component Name cannot be empty");

                return ConfigurationManager.AppSettings["Revo.IoC.GenericTypeResolverComponentName"];
            }
        }
    }
}
