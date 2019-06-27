using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    [TableName("COMUNI")]
    public class Comune 
    {
        [Column("ISTAT")]
        public string istat { get; set; }

        [Column("DENOMINAZIONE")]
        public string Descrizione { get; set; }

        public string Provincia { get; set; }

        public string Attivo { get; set; }

        public bool isActive { get { return Attivo == "S"; } }

        [Column("FISCALE")]
        public string codiceCatastale { get; set; }
    }
}
