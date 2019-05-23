using GestioneRimborsi.Core;
using GruppoCap;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestioneRimborsi.Web.Models
{
    public class SospensioneModel
    {
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ROW_ID")]
        public long ROW_ID { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ID_SOSPENSIONE")]
        public long ID_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ID_INDENNIZZO")]
        public Int32 ID_INDENNIZZO { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "NUMERO_PRESTAZIONE")]
        public String NUMERO_PRESTAZIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ID_STANDARD")]
        public Int32 ID_STANDARD { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "STATO_SOSPENSIONE")]
        public String STATO_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DATA_INIZIO_SOSPENSIONE")]
        public DateTime DATA_INIZIO_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DATA_FINE_SOSPENSIONE")]
        public DateTime DATA_FINE_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DATA_COMUNICAZIONE")]
        public DateTime DATA_COMUNICAZIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "CATEGORIA_SOSPENSIONE")]
        public String CATEGORIA_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "TIPO_SOSPENSIONE")]
        public String TIPO_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DURATA_SOSPENSIONE")]
        public decimal DURATA_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DATA_INS")]
        public DateTime DATA_INS { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "UTE_INS")]
        public String UTE_INS { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "DATA_VALERR")]
        public DateTime DATA_VALERR { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "UTE_VALERR")]
        public String UTE_VALERR { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "NOTE")]
        public String NOTE { get; set; }

        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ID_SOSPENSIONE_DWH")]
        public Int32 ID_SOSPENSIONE_DWH { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_DATA_INIZIO_SOSPENSIONE")]
        public DateTime ERR_DATA_INIZIO_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_DATA_FINE_SOSPENSIONE")]
        public DateTime ERR_DATA_FINE_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_DATA_COMUNICAZIONE")]
        public DateTime ERR_DATA_COMUNICAZIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_CATEGORIA_SOSPENSIONE")]
        public String ERR_CATEGORIA_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_TIPO_SOSPENSIONE")]
        public String ERR_TIPO_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "ERR_DURATA_SOSPENSIONE")]
        public decimal ERR_DURATA_SOSPENSIONE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "FLG_ERRORE")]
        public string FLG_ERRORE { get; set; }
        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "FLAG_ELIMINATO")]
        public int FLAG_ELIMINATO { get; set; }

        [MappedOn("GestioneRimborsi.Core.RettificaSospensione", "STATUS")]
        public int STATUS { get; set; }
         
        //public bool EDIT { get; set; }
        //public ISubCollection<RettificaSospensione> sospensioni { get; set; }
    }
}