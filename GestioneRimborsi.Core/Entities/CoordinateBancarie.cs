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
    public class CoordinateBancarie : IEntity
    {
        [Column("CODICE_CLIENTE")]
        public String CodiceCliente { get; set; }

        [Column("ABI")]
        public String ABI { get; set; }

        [Column("CAB")]
        public String CAB { get; set; }

        [Column("CONTO_CORRENTE")]
        public String ContoCorrente { get; set; }

        [Column("CIN")]
        public String CIN { get; set; }

        [Column("NAZIONE")]
        public String Nazione { get; set; }

        [Column("CHECK_DIGIT")]
        public String CheckDigit { get; set; }

        public object EntityId
        {
            get { return string.Format("Campi IBAN Cliente");}
        }

        public string DisplayText
        {
            get { return string.Format("Campi IBAN Cliente");}
        }
    }
}