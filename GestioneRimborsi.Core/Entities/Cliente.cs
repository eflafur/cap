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
    [TableName("COM_ANAGRAFICA_TERZI")]
    public class Cliente : IEntity
    {
        [Column("COD_CLIENTE")]
        public String codCliente { get; set; }

        [Column("COD_CLIENTE_INTEGRA")]
        public String codClienteIntegra { get; set; }

        //[Column("ROW_ID")]
        //public String rowID { get; set; }

        [Column("DES_RAGIONE_SOCIALE")]
        public String ragioneSocialeCliente { get; set; }

        [Column("COD_PARTITA_IVA")]
        public String partitaIva { get; set; }

        [Column("COD_CODICE_FISCALE")]
        public String codiceFiscale { get; set; }
        [Column("DES_COMUNE")]
        public String Comune { get; set; }
        [Column("DES_STRADA")]
        public String Strada { get; set; }
        [Column("DES_NUMERO_CIVICO")]
        public String NumeroCivico { get; set; }

        [Column("DES_CAP_SPEDIZIONE")]
        public String CAP { get; set; }
        [Column("DES_PROVINCIA")]
        public String Provincia { get; set; }
        [Column("COD_NAZIONE")]
        public String Nazione { get; set; }
        [Column("DES_EMAIL")]
        public String Email { get; set; }

        [Column("DES_TELEFONO")]
        public String Telefono { get; set; }

        [Column("DES_FAX")]
        public String Fax { get; set; }

        [Column("DTA_INS")]
        public DateTime? DataInserimento { get; set; }

        [Column("DTA_NASCITA")]
        public DateTime? DataNascita { get; set; }

        public object EntityId
        {
            get { return this.codCliente; }
        }

        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.ragioneSocialeCliente, this.codCliente); }
        }
    }
}
