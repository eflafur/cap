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
    [TableName("GRI_TIPI_SOSPENSIONE")]
    public class TipoSospensione : IEntity
    {
        [Column("ID_TIPO_SOSPENSIONE")]
        public Int32 ID_TIPO_SOSPENSIONE { get; set; }

        [Column("DESC_TIPO_SOSPENSIONE")]
        public String DESC_TIPO_SOSPENSIONE { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.ID_TIPO_SOSPENSIONE.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Tipo Sospensione: {0}-{1}", this.ID_TIPO_SOSPENSIONE.ToString(), this.DESC_TIPO_SOSPENSIONE); }
        }
    }
}