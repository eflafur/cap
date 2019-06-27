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
    public class RecapitoClienteRimborso : IEntity
    {
        [Column("COD_CLIENTE")]
        public String CodCliente { get; set; }

        [Column("COD_PUF")]
        public String CodPuf { get; set; }

        [Column("ID_INCASSO")]
        public String IdIncasso { get; set; }

        [Column("DES_RAGIONE_SOCIALE")]
        public String RagioneSociale { get; set; }

        [Column("DES_STRADA")]
        public String Strada { get; set; }

        [Column("DES_NUMERO_CIVICO")]
        public String NumeroCivico { get; set; }

        [Column("DES_CAP_SPEDIZIONE")]
        public String CapSpedizione { get; set; }

        [Column("DES_COMUNE")]
        public String Comune { get; set; }

        [Column("DES_PROVINCIA")]
        public String Provincia { get; set; }

        public object EntityId
        {
            get { return string.Format("Beneficiario Assegno: {0}", RagioneSociale); }
        }

        public string DisplayText
        {
            get { return string.Format("Beneficiario Assegno: {0}", RagioneSociale); }
        }
    }
}
