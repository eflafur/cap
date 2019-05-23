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
    public class InsolutoBolletta : IEntity
    {
        [Column("GESTITO")]
        public int Gestito { get; set; }

        [Column("COD_BOLLETTA")]
        public String CodiceBolletta { get; set; }

        [Column("NUMERO_DOCUMENTO")]
        public String NumeroDocumento { get; set; }

        [Column("ANNO_DOCUMENTO")]
        public String AnnoDocumento { get; set; }

        [Column("DATA_SCAD")]
        public DateTime Scadenza { get; set; }

        [Column("DATA_CONTAB")]
        public DateTime DataContab { get; set; }

        [Column("IMPO_BOLLETTA")]
        public String ImportoBolletta { get; set; }

        [Column("IMPO_PAGATO")]
        public decimal ImportoPagato { get; set; }

        [Column("IMPO_RIMB_NAC")]
        public decimal ImpRimbNac { get; set; }

        [Column("IMPO_RIMB_PAG_ECC")]
        public decimal ImpRimbPagEcc { get; set; }

        [Column("IMPO_RIMB_BNEG")]
        public decimal ImpRimbBneg { get; set; }

        [Column("IMPO_DA_COMPENSARE")]
        public decimal ImportoDaCompensare { get; set; }

        [Column("RESIDUO_BOLLETTA")]
        public decimal ResiduoBolletta { get; set; }

        public object EntityId
        {
            get { return this.CodiceBolletta; }
        }

        public string DisplayText
        {
            get { return string.Format("Bolletta {1} - Importo : {0}", this.ImportoBolletta, this.CodiceBolletta); }
        }
    }
}
