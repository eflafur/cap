using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public class UserOfRimborso
    {

        [Column("UTENTE_CONFERMA")]
        public string UtenteRimborso { get; set; }
       
    }
}
