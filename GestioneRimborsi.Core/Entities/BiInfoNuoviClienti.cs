using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    //[TableName("GRI_BI_CLIENTI_NEW")]
    //public class BiInfoNuoviClienti : InfoNuoviClienti
    //{
    //    public BiInfoNuoviClienti()
    //    {
    //    }
    //}

    [TableName("GRI_BI_CLIENTI_NEW")]
    public class BiInfoNuoviClienti : IEntity
    {
        //public InfoNuoviClienti()
        //{
        //}

        //[Ignore]
        //public ISubCollection<BiInfoNuoviClienti> InfoCLienti { get; set; }

        [Column("ID_LOTTO")]
        public int ID_LOTTO { get; set; }
        [Column("CLIENTE_ORIGINALE")]
        public String CLIENTE_ORIGINALE { get; set; }
        [Column("COD CLIENTE RIMBORSATO")]
        public String COD_CLIENTE_RIMBORSATO { get; set; }
        [Column("TIPOLOGIA_RICHIESTA")]
        public String TIPOLOGIA_RICHIESTA { get; set; }
        [Column("IMPORTO_BONUS_INTEGRATIVO")]
        public double IMPORTO_BONUS_INTEGRATIVO { get; set; }
        [Column("IMPORTO_BONUS_SOCIALE")]
        public double IMPORTO_BONUS_SOCIALE { get; set; }
        [Column("IMPORTO_TOTALE_RIMBORSO")]
        public double IMPORTO_TOTALE_RIMBORSO { get; set; }       
        [Column("ID_RICHIESTA")]
        public int ID_RICHIESTA { get; set; }
        [Column("PROTOCOLLO_RICHIESTA")]
        public int PROTOCOLLO_RICHIESTA { get; set; }
        [Column("PROTOCOLLO_DOMANDA")]
        public int PROTOCOLLO_DOMANDA { get; set; }
        [Column("RAGIONE_SOCIALE")]
        public String RAGIONE_SOCIALE { get; set; }
        [Column("CODICE_FISCALE")]
        public String CODICE_FISCALE { get; set; }
        [Column("COMUNE_FORNITURA")]
        public String COMUNE_FORNITURA { get; set; }
        [Column("INDIRIZZO_FORNITURA")]
        public String INDIRIZZO_FORNITURA { get; set; }
        [Column("CAP_FORNITURA")]
        public String CAP_FORNITURA { get; set; }
        [Column("DATA_RICHIESTA")]
        public DateTime DATA_RICHIESTA { get; set; }
        [Column("DATA_SCADENZA_RICHIESTA")]
        public DateTime DATA_SCADENZA_RICHIESTA { get; set; }


        public object EntityId
        {
            get { return this.ID_LOTTO; }
        }

        public string DisplayText
        {
            get { return string.Format("Codice Cliente {1} - LotId : {0}", this.CLIENTE_ORIGINALE, this.ID_LOTTO.ToString()); }
        }
    }
}
