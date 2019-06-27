using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
namespace GestioneRimborsi.Core
{

    [TableName("GRI_DATI_VALIDAZIONE_BI_V")]
    public class BIContratto
    {

        [Column("COD_CLIENTE_INTEGRA")]
        public String codClienteIntegra { get; set; }

        [Column("DES_RAGIONE_SOCIALE")]
        public String ragioneSocialeCliente { get; set; }

        [Column("COD_PARTITA_IVA")]
        public String partitaIva { get; set; }

        [Column("COD_CODICE_FISCALE")]
        public String codiceFiscale { get; set; }

        [Column("IS_DOMESTICORESIDENTE")]
        public bool iSdomestico { get; set; }

        [Column("IS_DOMESTICONONRESIDENTE")]
        public bool iSdomesticononres { get; set; }

        [Column("IS_CONDOMINIALE")]
        public bool iScondominiale { get; set; }

        [Column("INDIRIZZO_PUF")]
        public String Indirizzo { get; set; }

        [Column("TIPOLOGIA_ARERA")]
        public string TipologiaArera { get; set; }

        [Column("NUM_RESIDENTI")]
        public int NucleoComponentiFamiliari { get; set; }

        [Column("CIVICO_PUF")]
        public String NumeroCivico { get; set; }

        [Column("CAP_PUF")]
        public String Cap { get; set; }

        [Column("COD_ID_CONTRATTO")]
        public String idContratto { get; set; }

        [Column("COD_CLIENTE")]
        public String codCliente { get; set; }



        public object EntityId
        {
            get { return this.codiceFiscale; }
        }

        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.ragioneSocialeCliente, this.NumeroCivico); }
        }

    }
}
