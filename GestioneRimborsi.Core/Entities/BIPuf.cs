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

    public class BIPuf:IEntity
    {

        [Column("COD_CLIENTE")]
        public string codCliente{ get; set; }

        [Column("COMUNE_PUF")]
        public string Comune { get; set; }

        [Column("INDIRIZZO_PUF")]
        public string Strada { get; set; }

        [Column("CIVICO_PUF")]
        public string Civico{ get; set; }

        [Column("CAP_PUF")]
        public string Cap{ get; set; }

        [Column("DES_RAGIONE_SOCIALE")]
        public string Den { get; set; }

        [Ignore]
        public object EntityId
        {
            get { return this.Cap; }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.Cap, this.Comune); }
        }
    }
}
