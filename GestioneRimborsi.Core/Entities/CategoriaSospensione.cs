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
    [TableName("GRI_CATEGORIE_SOSPENSIONE")]
    public class CategoriaSospensione : IEntity
    {
        [Column("ID_CATEGORIA_SOSPENSIONE")]
        public Int32 ID_CATEGORIA_SOSPENSIONE { get; set; }

        [Column("DESC_CATEGORIA_SOSPENSIONE")]
        public String DESC_CATEGORIA_SOSPENSIONE { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}", this.ID_CATEGORIA_SOSPENSIONE.ToString()); }
        }

        public string DisplayText
        {
            get { return string.Format("Categoria Sospensione: {0}-{1}", this.ID_CATEGORIA_SOSPENSIONE.ToString(), this.DESC_CATEGORIA_SOSPENSIONE); }
        }
    }
}