using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaManager.Base.Entity.Sdd
{
    public class SedaCbiSddTableRow
    {
        /// <summary>
        /// Identificativo del messaggio logico. Deve essere univoco a parità di azienda mittente e data di creazione
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// Data e ora di creazione del messaggio logico
        /// </summary>
        public string CreDtTm { get; set; }
        /// <summary>
        /// Numero di transazioni DD incluse nel messaggio logico
        /// </summary>
        public int NbOfTxs { get; set; }
        /// <summary>
        /// Somma di controllo. Deve  coincidere con la somma degli importi delle disposizioni di incasso contenute nella richiesta
        /// </summary>
        public decimal CtrlSum { get; set; }
        /// <summary>
        /// Mittente della richiesta di incasso
        /// </summary>
        public string IdGrpHdr { get; set; }
        /// <summary>
        /// Valore fisso CBI
        /// </summary>
        public string IssrGrpHdr { get; set; }
        /// <summary>
        /// Identificativo informazioni di accredito
        /// </summary>
        public string PmtInfId { get; set; }
        /// <summary>
        /// Metodo di pagamento
        /// </summary>
        public string PmtMtd { get; set; }
        /// <summary>
        /// Codifica servizio
        /// </summary>
        public string CdSvcLvl { get; set; }
        /// <summary>
        /// Tipo SDD
        /// </summary>
        public string CdLclInstrm { get; set; }
        /// <summary>
        /// Tipo sequenza incasso
        /// </summary>
        public string SeqTp { get; set; }
        /// <summary>
        /// Data scadenza richiesta dal mittente
        /// </summary>
        public string ReqdColltnDt { get; set; }
        /// <summary>
        /// Descrizione creditore
        /// </summary>
        public string NmCdtr { get; set; }
        /// <summary>
        /// Iban creditore
        /// </summary>
        public string IbanCdtrAcct { get; set; }
        /// <summary>
        /// Banca Passiva sulla quale risiede il c/c di accredito
        /// </summary>
        public string MmbIdClrSysMmbId { get; set; }
        /// <summary>
        /// Identificativo schema creditore
        /// </summary>
        public string IdCdtrSchmeIdOthr { get; set; }
        /// <summary>
        /// Identificativo univoco assegnato all'istruzione dal Mittente nei confronti della sua Banca
        /// </summary>
        public long InstrID { get; set; }
        /// <summary>
        /// Identificativo URI assegnato dal Mittente e che identifica la singola disposizione di incasso per tutta la catena fino al Debitore.
        /// </summary>
        public long EndToEndID { get; set; }
        /// <summary>
        /// Importo della singola transazione
        /// </summary>
        public decimal InstdAmt { get; set; }
        /// <summary>
        /// Identificativo mandato
        /// </summary>
        public string MndtId { get; set; }
        /// <summary>
        /// Data di sottoscrizione
        /// </summary>
        public string DtOfSgntr { get; set; }
        /// <summary>
        /// Titolare c/c addebito
        /// </summary>
        public string NmDbtr { get; set; }
        /// <summary>
        /// Coordinate bancarie di addebito
        /// </summary>
        public string IbanDbtrAcct { get; set; }
        /// <summary>
        /// Causale della transazione
        /// </summary>
        public string CdPurp { get; set; }
        /// <summary>
        /// Campo predisposto ma non utilizzato
        /// </summary>
        public string AmdmntInd { get; set; }
        /// <summary>
        /// Campo predsposto ma non utilizzato
        /// </summary>
        public string OrgnlMndtId { get; set; }
        /// <summary>
        /// Descrizione addebito
        /// </summary>
        public string RmtInfUstrd { get; set; }
        /// <summary>
        /// Batch di fatturazione di riferimento
        /// </summary>
        public int BatchFatturazione { get; set; }
        /// <summary>
        /// Data batch fatturazione
        /// </summary>
        public DateTime DataBatchFatturazione { get; set; }
        public string NMORGNLDBTR { get; set; }
        public string IBANORGNLDBTRACCT { get; set; }

        /// <summary>
        /// Nome originale del Creditore con AT24
        /// </summary>
        public string OrgnlCdtrNm { get; set; }

        /// <summary>
        /// Id originale del Creditore con AT24 
        /// </summary>
        public string OrgnlCdtrId { get; set; }


    }
}
