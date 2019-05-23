using GruppoCap;
using System;
using System.Linq;

namespace GestioneRimborsi.Web
{
    public static class WebApiSettings
    {
        private static T GetSettingFromAppSettings<T>(string key)
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
                return System.Configuration.ConfigurationManager.AppSettings[key].CoerceTo<T>();
            else
                throw new ApplicationException(string.Format("Impossibile trovare il setting '{0}' nella configurazione del sistema", key));
        }
        private static T GetSettingFromAppSettings<T>(string key, T defaultVal)
        {
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key))
                return System.Configuration.ConfigurationManager.AppSettings[key].CoerceToOrDefault<T>(defaultVal);
            else
                return defaultVal;
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class RevoInfo : Attribute
    {
        public RevoInfo()
        {
        }
        public RevoInfo(string description)
        {
            Description = description;
        }
        public string Description { get; }
    }
}