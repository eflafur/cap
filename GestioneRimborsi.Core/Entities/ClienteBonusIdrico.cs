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
    [TableName("BONUSIDRICO_RIMBORSI")]
    public class ClienteBonusIdrico : IEntity
    {
        [Column("COD_CLIENTE")]
        public String codCliente { get; set; }


        [Column("DATA_EMISSIONE")]
        public DateTime DataEmissione { get; set; }

        [Column("CODICEBONUS")]
        public String CodiceBonus { get; set; }

        [Column("IMP_BONUS")]
        public decimal ImportoBonus { get; set; }

        [Column("UTE_INS")]
        public String UtenteInserimento { get; set; }

        public object EntityId
        {
            get { return this.CodiceBonus; }
        }

        public string DisplayText
        {
            get { return string.Format("Utente {1} - Codice Bonus : {0}", this.UtenteInserimento, this.CodiceBonus); }
        }
    }
}
