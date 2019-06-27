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
    public class RettificaFuoriStandard : IEntity
    {
        [Column("ID_FUORI_STANDARD")]
        public Int32 IDFuoriStandard { get; set; }

        [Column("DATA_INIZIO_ATTIVITA")]
        public DateTime DataInizioAttivita { get; set; }

        [Column("DATA_FINE_ATTIVITA")]
        public DateTime DataFineAttivita { get; set; }

        [Column("QUANTITA")]
        public Decimal Quantita { get; set; }

        [Column("QUANTITA_SOSPENSIONE")]
        public int QuantitaSospensione { get; set; }

        [Column("FLAG_STANDARD")]
        public String FuoriStandard { get; set; }

        [Column("CAUSALE")]
        public String Causale { get; set; }

        [Column("SOTTOCAUSALE")]
        public String SottoCausale { get; set; }

        [Column("ESITO")]
        public Int32 Esito { get; set; }

        [Column("CREATO_IL")]
        public DateTime CreatoIl { get; set; }

        [Column("AUTORE")]
        public String Autore { get; set; }

        [Column("STORICO")]
        public Int32 Storico { get; set; }

        [Column("COD_GRUPPO")]
        public String CodiceGruppo { get; set; }

        [Column("DATA_INIZIO_ORIGINALE")]
        public DateTime DataInizioOriginale { get; set; }

        [Column("DATA_FINE_ORIGINALE")]
        public DateTime DataFineOriginale { get; set; }

        [Column("QUANTITA_ORIGINALE")]
        public String QuantitaOriginale { get; set; }

        [Column("CODICE_CLIENTE")]
        public String CodiceCliente { get; set; }

        [Column("CODICE_PUF")]
        public String CodicePuf { get; set; }

        [Column("CODICE_CONTRATTO")]
        public String CodiceContratto { get; set; }

        [Column("INDENNIZZABILE")]
        public String NonIndennizzabile { get; set; }

        [Column("NOTE")]
        public String Note { get; set; }

        [Column("NOTE_APPROVATORE")]
        public String NoteApprovatore { get; set; }

        [Column("FLG_IS_RETTIFICA")]
        public Int32 FlagIsRettifica { get; set; }

        [Column("FLG_STATO")]
        public Int32 FlagStato { get; set; }

        [Column("PROCESS_OWNER")]
        public String ProcessOwner { get; set; }

        [Column("MANAGER")]
        public String Manager { get; set; }

        [Column("DATA_APPR_MANAGER")]
        public DateTime DataManager { get; set; }        

        [Column("DATA_APPROVAZIONE_PO")]
        public DateTime DataApprovazionePO { get; set; }

        [Column("NOTE_PROCESS_OWNER")]
        public String NoteProcessOwner { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.IDFuoriStandard.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Rettifica: {0}-{1}", this.Causale.ToString(), this.SottoCausale); }
        }
    }
}