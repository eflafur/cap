using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestioneRimborsi.Core;

namespace GestioneRimborsi.Web.Models
{
    public class SgateCapModel
    {
        public SgateRichieste sgate { get; set; }
        public Double ImportoBi { get; set; }
        public string integra { get; set; }
        public Double ImportoIntegrativo { get; set; }
        public Int32 status { get; set; }
    }
}