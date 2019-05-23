using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using GruppoCap.Core;


namespace GestioneRimborsi.Core 
{
    [TableName("GRI_BI_REQUEST_CAP")]
    [PrimaryKey("BI_REQ_CAP_ID", autoIncrement = false)]
    public  class BICapRequest:IEntity
    {
        [Column("BI_REQ_CAP_ID")]
        public int Id { get; set; }

        //    [ForeignKey("BI_LOT_ID")]
        [Column("BI_LOTCAP_ID")]
        public int lotId { get; set; }

        [Column("BI_PROT_RICHIESTA")]
        public string ProtRichiesta { get; set; }

        [Column("BI_TIPOLOGIA_RICHIESTA")]
        public string TipoRichiesta { get; set; }

        [Column("BI_TIPOLOGIA_UTENTE")]
        public int TipoUtente { get; set; }

        [Column("BI_DATAPRESENTAZIONE")]
        public DateTime DataPresentazione { get; set; }

        [Column("BI_SCAD_INVIOESITI")]
        public DateTime DataScadenzaInvioEsiti{ get; set; }

        [Column("BI_NOME")]
        public string Nome { get; set; }

        [Column("BI_COGNOME")]
        public string Cognome { get; set; }

        [Column("BI_DENOMOMINAZIONE")]
        public string Denominazione{ get; set; }

        [Column("BI_CF")]
        public string Cf{ get; set; }

        [Column("BI_INTEGRA")]
        public string Integra{ get; set; }

        [Column("BI_INDIRIZZO_FORNITURA")]
        public string IndFornitua{ get; set; }

        [Column("BI_NUMERO_FAMANA")]
        public int NumFamAnagrafica { get; set; }

        [Column("BI_USO")]
        public string Uso { get; set; }

        [Column("BI_STATO_UTENZA")]
        public int StatoUtenza{ get; set; }

        [Column("BI_ESITO_AUTOVAL")]
        public string EsitoAutoVal{ get; set; }

        [Column("BI_ESITO_MANVAL")]
        public string EsitoManVal { get; set; }

        [Column("BI_PROCESSATO")]
        public int Processato { get; set; }

        [Column("BI_CONFERMA")]
        public int ReqInterno { get; set; }

        [Column("BI_IMPORTO_BI")]
        public Double ImportoBi { get; set; }

        [Column("BI_IMPORTO_INTEGRATIVO")]
        public Double ImportoIntegrativo { get; set; }

        [Column("BI_ESITO")]
        public string Esito{ get; set; }

        [Column("BI_CLIENTE")]
        public string codCliente{ get; set; }

        [Column("BI_CLIENTE_NEW")]
        public string codClienteNew { get; set; }

        //[Column("row_number")]
        //public int rowNumber { get; set; }
        [Ignore]
        public object EntityId
        {
            get { return this.Id; }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Utente {1} - RagioneSociale : {0}", this.Id, this.ProtRichiesta); }
        }
    }
}
