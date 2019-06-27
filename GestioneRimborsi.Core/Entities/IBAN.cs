using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
using System.ComponentModel.DataAnnotations;

namespace GestioneRimborsi.Core
{
    [TableName("GRI_IBAN")]
    public class IBAN : IEntity
    {
        [Column("CODICE_CLIENTE")]
        public string CodCliente {get; set;}

        [Column("IBAN")]
        public string CodiceIBAN {get;set;}

        [Column("DATA_INSERIMENTO")]
        public DateTime? DataInserimento { get; set; }

        [Column("UTENTE_INSERIMENTO")]
        public String UtenteInserimento { get; set; }

        [Column("DATA_ULT_VARIAZIONE")]
        public DateTime? DataUltimaVariazione { get; set; }

        [Column("UTENTE_ULT_VARIAZIONE")]
        public String UtenteUltimaVariazione { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.CodCliente, this.CodiceIBAN); }
        }

        public string DisplayText
        {
            get { return string.Format("{0}-{1}", this.CodCliente, this.CodiceIBAN); }
        }
    }
}
