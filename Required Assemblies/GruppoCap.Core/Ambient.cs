using System;
using System.Configuration;
using System.Web.Configuration;

namespace GruppoCap.Core
{
    public static class Ambient
    {
        // CURRENT AMBIENT
        public static String Current
        {
            get { return ConfigurationManager.AppSettings["Revo.Ambient"] ?? "dev"; }
        }

        // CURRENT APPLICATION ID
        public static String CurrentApplicationId
        {
            get { return ConfigurationManager.AppSettings["Revo.Application.Id"] ?? "generic"; }
        }

        // CURRENT APPLICATION VERSION
        public static String CurrentApplicationVersion
        {
            get { return ConfigurationManager.AppSettings["Revo.Application.Version"] ?? "1.0"; }
        }

        // CURRENT APPLICATION NAME
        public static String CurrentApplicationName
        {
            get { return ConfigurationManager.AppSettings["Revo.Application.Name"] ?? "Gruppo CAP Application"; }
        }


        // CURRENT APPLICATION IS IN BETA
        public static Boolean CurrentApplicationIsInBeta
        {
            get
            {
                try { return ConfigurationManager.AppSettings["Revo.Application.IsBetaVersion"].CoerceTo<Boolean>(); }
                catch { return false; }
            }
        }

        // CURRENT APPLICATION CONNECTION STRING NAME
        public static String CurrentApplicationConnectionStringName(String ambient = null, String application = null)
        {
            String _ambient = ambient.IsNullOrWhiteSpace() ? Current : ambient;
            String _application = application.IsNullOrWhiteSpace() ? "application" : application;

            return "{0}.{1}.db".FormatWith(_ambient, _application);
        }

        public static String CurrentApplicationConnectionString(String application = null)
        {
            if (ConnectionStringSection.SectionInformation.ConfigSource.IsNullOrWhiteSpace()
                && ConnectionStringSection.ConnectionStrings.Count == 0)
            {
                throw new ConfigurationErrorsException("ConnectionStringSection into the local web.config is missing...");
            }

            return ConfigurationManager.ConnectionStrings[CurrentApplicationConnectionStringName(Current, application)].ConnectionString
                    ?? ConfigurationManager.ConnectionStrings["application.db"].ConnectionString;

        }

        // APPLICATION CONNECTION STRING
        public static String ApplicationConnectionString(String Ambient)
        {
            if (ConnectionStringSection.SectionInformation.ConfigSource.IsNullOrWhiteSpace()
                && ConnectionStringSection.ConnectionStrings.Count == 0)
            {
                throw new ConfigurationErrorsException("ConnectionStringSection into the local web.config is missing...");
            }

            return ConfigurationManager.ConnectionStrings["{0}.application.db".FormatWith(Ambient)].ConnectionString;
        }


        // CONNECTION STRING SECTION
        private static ConnectionStringsSection ConnectionStringSection
        {
            get
            {
                return (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
            }
        }

        private static AuthenticationSection AuthenticationSection
        {
            get
            {
                return (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            }
        }

        public static AuthenticationMode CurrentAuthenticationMode
        {
            get { return AuthenticationSection.Mode; }
        }


        public static String CurrentApplicationBootstrapVersion
        {
            get
            {
                return ConfigurationManager.AppSettings["Revo.Bootstrap.Version"] ?? "3"; ;
            }
        }

        public static Boolean IsCurrentApplicationBootstrapVersionNewerThan4
        {
            get
            {
                try { return CurrentApplicationBootstrapVersion.StartsWith("3") == false; }
                catch { return false; }
            }
        }

    }
}
