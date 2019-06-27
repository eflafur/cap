using GestioneRimborsi.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class SchedaFuoriStandardModel
    {
        public string IDFS { get; set; }        
        public bool SolaLettura { get; set; }

        public String FuoriStandard { get; set; }
    }
}