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
    
    public class DettaglioRimborso : IEntity
    {
        [Column("ANNO_DOCUMENTO")]
        public String AnnoDocumento { get; set; }

        [Column("NUMERO_DOCUMENTO")]
        public String NumeroDocumento { get; set; }

        [Column("TIPO_RIMBORSO")]
        public String TipoRimborso { get; set; }

        [Column("TIPO_DOCUMENTO")]
        public String TipoDocumento { get; set; }

        [Column("IMPORTO")]
        public Decimal Importo { get; set; }

        [Column("NUMERO_BOLLETTA")]
        public String NumeroBolletta { get; set; }

        [Column("ANNO_RIMBORSO_INTEGRA")]
        public String AnnoRimborsoIntegra { get; set; }

        [Column("ID_RIMBORSO_INTEGRA")]
        public String IDRimborsoIntegra { get; set; }
        
        [Column("MANDATO")]
        public decimal Mandato { get; set; }

        [Column("DATA_CONTAB")]
        public DateTime? data_contab { get; set; }

        [Column("COD_BOLLETTA")]
        public String cod_bolletta {get; set; }
        
        public object EntityId
        {
            get { return this.NumeroDocumento; }
        }

        public string DisplayText
        {
            get { return string.Format("Rimborso num: {0}", this.NumeroDocumento); }
        }
    }
}
