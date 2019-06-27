using GestioneRimborsi.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class SospensioniViewModel
    {
        public ISubCollection<RettificaSospensione> sospensioni { get; set; }
        public bool result { get; set; }
        public string numeroPrestazioneOut { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public string durataPrestazione { get; set; }
        public string durataTotaleSospensioni { get; set; }
    }
}