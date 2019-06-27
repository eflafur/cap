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
    [TableName("V_TEST_SEPA")]
    [PrimaryKey("PKOPERAZIONE")]
  public class Rimborso :IEntity
    {
        [Column("PKOPERAZIONE")]
        public string IdBolletta { get; set; }

        [Column("IBANDEST")]
        public string IbanDestinatario { get; set; }

        [Column("ABIORD")]
        public string AbiOrdinante { get; set; }

        [Column("CABORD")]
        public string CabOrdinante { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column("IMPORTO")]
        public decimal TotaleEuro { get; set; }

        [Column("DESCRDEST")]
        public string DescrizioneDestinatario { get; set; }

        [Column("UTENTE_CONFERMA")]
        public string UtenteRimborso { get; set; }

        [Column("CODICE_CLIENTE")]
        public string CodiceClienteDestinatario { get; set; }


        [Column("BANCA_ORDINANTE")]
        public string DescrizioneBancaOrdinante { get; set; }

        [Column("IDRIMBORSO")]
        public string IdRimborso { get; set; }


        [Ignore]
        public List<string> UsersOfLottiRimborsi { get; set; }


        public object EntityId
        {
            get { return this.IdBolletta; }
        }

        public string DisplayText
        {
            get { return string.Format("Utente {0} - IdBolletta : {1}", this.UtenteRimborso, this.IdRimborso); }
        }
    }
}
