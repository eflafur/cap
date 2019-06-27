using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class CalcoloDurataSospensioniResponse
    {
        public OutputData OutputData { get; set; }
    }
    public class OutputData
    {
        public List<SospensioneOut> sospensioni { get; set; }
        public bool result { get; set; }
        public string numeroPrestazioneOut { get; set; }
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public decimal durataPrestazione { get; set; }
        public decimal durataTotaleSospensioni { get; set; }
    }

    public class SospensioneOut
    {
        public string codiceSospensioneOut { get; set; }
        public decimal durataSospensione { get; set; }
    }
}