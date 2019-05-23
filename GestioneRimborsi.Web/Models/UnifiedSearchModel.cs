using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public enum UnifiedSearchOptions
    {
        Tipologia, IndicatoreStd, CaseId, CodCliente, DataFineAttivita, InFuoriStd, SoloFuoriStd, FuoriStandardStorico, FlagCodCliente, FlagIndennizzabile
    }
    public class UnifiedSearchModel
    {
        Dictionary<UnifiedSearchOptions, Boolean> internalSettings = new Dictionary<UnifiedSearchOptions, bool>();
        Dictionary<string, string> internalStringSettings = new Dictionary<string, string>();

        public UnifiedSearchModel() { }

        public UnifiedSearchModel(string FormName, List<UnifiedSearchOptions> Arguments)
        {
            internalStringSettings.Add("FormName", FormName);
            Arguments.ForEach(ee => AddNewSetting(ee));
        }

        /// <summary>
        /// Add a new setting item to the internal collection
        /// </summary>
        /// <param name="settingKey">is the name of the setting that will be used by the software</param>
        /// <param name="enabled">if true setting is enabled, otherwise not.</param>
        public void AddNewSetting(UnifiedSearchOptions settingKey, bool enabled)
        {
            internalSettings.Add(settingKey, enabled);
        }

        /// <summary>
        /// Add a new setting item to the internal collection
        /// </summary>
        /// <param name="settingKey">is the name of the setting that will be used by the software</param>
        /// <remarks>Using this overload automatically add the setting as enabled.</remarks>
        public void AddNewSetting(UnifiedSearchOptions settingKey)
        {
            AddNewSetting(settingKey, true);
        }

        public void AddNewSetting(string settingKey, string settingValue)
        {

            internalStringSettings.Add(settingKey, settingValue);
        }

        public object this[string index]
        {
            get
            {
                if (internalStringSettings.Keys.Contains(index))
                    return internalStringSettings[index];
                return string.Empty;
            }
        }
        public bool this[UnifiedSearchOptions index]
        {
            get
            {
                if (internalSettings.Keys.Contains(index))
                    return internalSettings[index];
                return false;
            }
        }
    }



}