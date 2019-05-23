using GruppoCap.Core;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public class EccezioneFuoriStandard : IEntity
    {
        [Column("CODICE_STANDARD")]
        public int CodiceStandard { get; set; }

        [Column("TIPO_STANDARD")]
        public String TipoStandard { get; set; }
                

        public object EntityId
        {
            get { return this.CodiceStandard; }
        }

        public string DisplayText
        {
            get { return string.Format("{0}-{1}", this.CodiceStandard.ToString(), this.TipoStandard); }
        }
    }
}
