using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SepaManager.Base.Entity.Insoluti
{
    public class InsolutoObject
    {
        public string COD_BOLLETTA { get; set; }
        public string COD_CLIENTE_INTEGRA { get; set; }
        public string DES_RAGIONE_SOCIALE { get; set; }
        public DateTime DATA_EMISSIONE_BOLLETTA { get; set; }
        public decimal IMPO_BOLLETTA { get; set; }
        public string CAUSALE { get; set; }
        public string FLG_STORNO { get; set; }
        public DateTime DTA_INS { get; set; }
        public string COD_UTENTE_INS { get; set; }
        public string DES_NOTE { get; set; }
        public DateTime DTA_REGISTRAZIONE { get; set; }
        public long COD_ID_ACQUISIZIONE { get; set; }
        public DateTime DATA_SCADENZA_BOLLETTA { get; set; }


    }
}
