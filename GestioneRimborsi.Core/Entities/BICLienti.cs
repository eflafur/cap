using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
//using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestioneRimborsi.Core
{
    public class BICLienti:Cliente
    {
        [Column("DES_NOME")]
        public String Nome { get; set; }

        [Column("DES_COGNOME")]
        public String Cognome { get; set; }

    }
}
