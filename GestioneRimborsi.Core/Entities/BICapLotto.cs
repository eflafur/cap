using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using GruppoCap.Core;

namespace GestioneRimborsi.Core
{

    [TableName("GRI_BI_LOTS_CAP")]
    [PrimaryKey("BI_CAP_ID", autoIncrement =true)]    
    public class BICapLotto : IEntity
    {
        [Column("BI_CAP_ID")]
        public int Id { get; set; }

        [Column("BI_DESC")]
        public string Desc { get; set; }

        [Column("BI_DATA_ACQUISIZIONE")]
        public DateTime DataAcquisizione { get; set; }

        [Column("BI_DATACARICO")]
        public DateTime DataCarico { get; set; }

        [Column("BI_DATASCADENZA_ESITI")]
        public DateTime DataScadenza { get; set; }

        [Column("BI_RICHIESTETOT")]
        public int RichiesteTotali { get; set; }

        [Column("BI_RICHIESTE_AUTOVAL")]
        public int RichiesteAutoVal { get; set; }

        [Column("BI_RICHIESTE_VAL")]
        public int RichiesteVal { get; set; }

        [Column("BI_DATAINVIOESITI")]
        public DateTime DataInvioEsiti { get; set; }

        [Column("BI_DATARISCONTROESITI")]
        public DateTime FDataRiscontroEsiti { get; set; }

        [Column("BI_DATACONFERMASGATE")]
        public DateTime DataConfermaSgate { get; set; }

        [Column("BI_STATUS")]
        public int Status { get; set; }

        [Ignore]
        public object EntityId
        {
            get { return this.Id; }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.Id, this.Desc); }
        }

    }
}
