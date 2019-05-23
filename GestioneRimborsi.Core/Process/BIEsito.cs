using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core.Process
{
   public class BIEsito
    {
        public string esito { get; set; }
        public string esitosgate { get; set; }
        public int error { get; set; }
        public int errorana { get; set; }
        public string cliente { get; set; }

        public BIEsito()
        {
            //this.esito = esito;
            //this.error = error;
        }

    }
}
