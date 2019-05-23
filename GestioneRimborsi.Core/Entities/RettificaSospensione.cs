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
    [TableName("GRI_GIN_SOSPENSIONI_V")]
    public class RettificaSospensione : IEntity
    {
        [Column("ROW_ID")]
        public long ROW_ID { get; set; }

        [Column("ID_SOSPENSIONE")]
        public long ID_SOSPENSIONE { get; set; }

        [Column("ID_INDENNIZZO")]
        public Int32 ID_INDENNIZZO { get; set; }

        [Column("NUMERO_PRESTAZIONE")]
        public String NUMERO_PRESTAZIONE { get; set; }

        [Column("ID_STANDARD")]
        public Int32 ID_STANDARD { get; set; }

        [Column("STATO_SOSPENSIONE")]
        public String STATO_SOSPENSIONE { get; set; }

        [Column("DATA_INIZIO_SOSPENSIONE")]
        public DateTime DATA_INIZIO_SOSPENSIONE { get; set; }

        [Column("DATA_FINE_SOSPENSIONE")]
        public DateTime DATA_FINE_SOSPENSIONE { get; set; }

        [Column("DATA_COMUNICAZIONE")]
        public DateTime DATA_COMUNICAZIONE { get; set; }

        [Column("CATEGORIA_SOSPENSIONE")]
        public String CATEGORIA_SOSPENSIONE { get; set; }

        [Column("TIPO_SOSPENSIONE")]
        public String TIPO_SOSPENSIONE { get; set; }

        [Column("DURATA_SOSPENSIONE")]
        public decimal DURATA_SOSPENSIONE { get; set; }

        [Column("DATA_INS")]
        public DateTime DATA_INS { get; set; }

        [Column("UTE_INS")]
        public String UTE_INS { get; set; }

        [Column("DATA_VALERR")]
        public DateTime DATA_VALERR { get; set; }

        [Column("UTE_VALERR")]
        public String UTE_VALERR { get; set; }

        [Column("NOTE")]
        public String NOTE { get; set; }

        [Column("ID_SOSPENSIONE_DWH")]
        public Int32 ID_SOSPENSIONE_DWH { get; set; }

        [Column("ERR_DATA_INIZIO_SOSPENSIONE")]
        public DateTime ERR_DATA_INIZIO_SOSPENSIONE { get; set; }

        [Column("ERR_DATA_FINE_SOSPENSIONE")]
        public DateTime ERR_DATA_FINE_SOSPENSIONE { get; set; }

        [Column("ERR_DATA_COMUNICAZIONE")]
        public DateTime ERR_DATA_COMUNICAZIONE { get; set; }

        [Column("ERR_CATEGORIA_SOSPENSIONE")]
        public String ERR_CATEGORIA_SOSPENSIONE { get; set; }

        [Column("ERR_TIPO_SOSPENSIONE")]
        public String ERR_TIPO_SOSPENSIONE { get; set; }

        [Column("ERR_DURATA_SOSPENSIONE")]
        public decimal ERR_DURATA_SOSPENSIONE { get; set; }

        [Column("FLG_ERRORE")]
        public string FLG_ERRORE { get; set; }

        [Column("FLAG_ELIMINATO")]
        public int FLAG_ELIMINATO { get; set; }

        [Column("STATUS")]
        public int STATUS { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.ID_SOSPENSIONE.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Sospensione: {0}-{1}", this.ID_SOSPENSIONE.ToString(), this.DATA_INS); }
        }
    }
}