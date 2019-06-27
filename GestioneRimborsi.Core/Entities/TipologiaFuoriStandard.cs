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
    [TableName("GIN_STANDARD_V")]
    public class TipologiaFuoriStandard : IEntity
    {
        [Column("ID_STANDARD")]
        public Int32 IDStandard { get; set; }

        [Column("COD_STANDARD")]
        public Int32 CodStandard { get; set; }

        [Column("DESC_STANDARD")]
        public String DescStandard { get; set; }
        
        [Column("TIPO_STANDARD")]
        public String TipoStandard { get; set; }

        [Column("IMPORTO_INDENN")]
        public String ImportoIndennizzo { get; set; }

        [Column("FL_RIMB_PROG")]
        public String FlagRimbProg { get; set; }

        [Column("UNITA_MISURA")]
        public String UnitaMisura { get; set; }

        [Column("TIPO")]
        public String Tipo { get; set; }

        [Column("CODICE_PRESTAZIONE")]
        public String CodicePrestazione { get; set; }

        [Column("VAL_STANDARD")]
        public String ValStandard { get; set; }
       

        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.IDStandard.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Standard num: {0}-{1}", this.IDStandard.ToString(), this.DescStandard); }
        }

    }
}
