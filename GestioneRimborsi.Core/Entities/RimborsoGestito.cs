using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Core
{
    public class RimborsoGestito
    {
        public string AnnoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public string UtenteInserimento { get; set; }
       
        public string TipoDocumento { get; set; }

        public string TipoRimborso { get; set; }
        public string CodiceCliente { get; set; }

        public string CodicePuntoFornitura { get; set; }

        public string IBAN { get; set; }        
        public string IntestazioneAlt { get; set; }
        public string IndirizzoAlt { get; set; }

        public string LocalitaAlt { get; set; }
        public string CivicoAlt { get; set; }
        public string ProvinciaAlt { get; set; }
        public string CAPAlt { get; set; }
        public string Beneficiario { get; set; }

        public string IndirizzoAssegno { get; set; }
        public string LocalitaAssegno { get; set; }
        public string CAPAssegno { get; set; }
        public string ProvinciaAssegno { get; set; }

        public string CodiceAzienda { get; set; }

        public List<RigaRimborsoGestito> RigheRimborso { get; set; }      

    }

    public class RigaRimborsoGestito
    {
        public string TipoRimborso { get; set; }
        public string CodiceBolletta { get; set; }
        public decimal Importo { get; set; }
    }
}