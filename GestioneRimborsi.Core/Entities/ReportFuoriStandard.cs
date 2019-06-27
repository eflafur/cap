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
    public class ReportFuoriStandard : IEntity
    {
        [Column("ANNO")]
        public String Anno { get; set; }
        [Column("NUMERO")]
        public String Numero { get; set; }
        [Column("DATA")]
        public String Data { get; set; }
        [Column("IMPORTO")]
        public Decimal Importo { get; set; }
        [Column("CLIENTE")]
        public String Cliente { get; set; }
        [Column("PUF")]
        public String Puf { get; set; }
        [Column("CONTRATTO")]
        public String Contratto { get; set; }
        [Column("COD_RINTRACCIABILITA")]
        public String Rintracciabilita { get; set; }
        [Column("CODICE_PRESTAZIONE")]
        public String Prestazione { get; set; }
        [Column("DESCRIZIONE_PRESTAZIONE")]
        public String DescPrestazione { get; set; }
        [Column("ESITO")]
        public String Esito { get; set; }
        [Column("DETTAGLIO_ESITO")]
        public String DettaglioEsito { get; set; }
        [Column("NOTE_ESITO")]
        public String NoteEsito { get; set; }
        [Column("TIPOLOGIA_CASE")]
        public String TipologiaCase { get; set; }
        [Column("DESCRIZIONE_STANDARD")]
        public String DescStandard { get; set; }
        [Column("TIPO_STANDARD")]
        public String TipoStandard { get; set; }
        [Column("CODICE_CAUSA")]
        public String CodiceCausa { get; set; }
        [Column("DESCRIZIONE_SOTTOCAUSA")]
        public String DescSottoCausa { get; set; }
        [Column("RETTIFICATO")]
        public String Rettificato { get; set; }
        [Column("IN_STANDARD_SE_RETTIFICATO")]
        public String InStandardSeRettificato { get; set; }
        [Column("DATA_INIZIO")]
        public String DataInizio { get; set; }
        [Column("DATA_FINE")]
        public String DataFine { get; set; }
        [Column("VAL_STANDARD")]
        public Decimal ValStandard { get; set; }
        [Column("TEMPO_LAVORAZIONE")]
        public Decimal TempoLavorazione { get; set; }
        [Column("RET_DATA_INIZIO")]
        public String RetDataInizio { get; set; }
        [Column("RET_DATA_FINE")]
        public String RetDataFine { get; set; }
        [Column("RET_TEMPO_LAVORAZIONE")]
        public Decimal RetTempoLavorazione { get; set; }
        [Column("MOLT_X_IMP_INDENNIZZO")]
        public Decimal ImportoMoltX { get; set; }
        [Column("DESC_STADIO_INDENNIZZO")]
        public String StadioIndennizzo { get; set; }
        [Column("DICHIARATO_NON_INDENNIZABILE")]
        public String DichNonIndennizzabile { get; set; }
        [Column("NOTE")]
        public String Note { get; set; }
        [Column("DESC_STATO_FUORI_STANDARD")]
        public String StatoFuoriStandard { get; set; }
        [Column("DATA_PAGAMENTO")]
        public String DataPagamento { get; set; }
        [Column("DATA_EMISSIONE_BOLLETTA")]
        public String DataEmissioneBolletta { get; set; }
        [Column("BOLLETTA")]
        public String Bolletta { get; set; }
        [Column("UTENTE_INSERIMENTO")]
        public String UtenteInserimento { get; set; }
        [Column("DATA_INSERIMENTO")]
        public String DataInserimento { get; set; }
        [Column("UTENTE_VALIDAZIONE")]
        public String UtenteValidazione { get; set; }
        [Column("DATA_VALIDAZIONE")]
        public String DataValidazione { get; set; }
        [Column("UTENTE_RETTIFICA")]
        public String UtenteRettifica { get; set; }
        [Column("DATA_RETTIFICA")]
        public String DataRettifica { get; set; }
        [Column("UTENTE_BENESTARE_PAGAMENTO")]
        public String UtenteBenestarePagamento { get; set; }
        [Column("DATA_BENESTARE_PAGAMENTO")]
        public String DataBenestarePagamento { get; set; }
        [Column("UTENTE_LIQUIDAZIONE")]
        public String UtenteLiquidazione { get; set; }
        [Column("UTENTE_ANNULLAMENTO")]
        public String UtenteAnnullamento { get; set; }
        [Column("DATA_ANNULLAMENTO")]
        public String DataAnnullamento { get; set; }
        [Column("COD_GRUPPO")]
        public String CodGruppo { get; set; }

        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.Rintracciabilita.ToString(), this.Data); }
        }

        public string DisplayText
        {
            get { return string.Format("FuoriStandard num: {0}-{1}", this.Rintracciabilita.ToString(), this.Data); }
        }

    }
}
