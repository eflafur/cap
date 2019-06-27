using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using GruppoCap.DAL;

namespace GestioneRimborsi.Core
{
    public class FuoriStandard : IEntity
    {
        [Column("ID_INDENNIZZO")]
        public Int32 IDFS { get; set; }

        [Column("PROVENIENZA")]
        public String Provenienza { get; set; }

        [Column("ID_CASE")]
        public String CaseID { get; set; }

        [Column("COD_CLIENTE")]
        public String CodCliente { get; set; }

        [Column("COD_PUF")]
        public String CodPuf { get; set; }

        [Column("COD_CONTRATTO")]
        public String CodContratto { get; set; }

        [Column("COD_STANDARD")]
        public Int32 CodStandard { get; set; }

        [Column("DT_DECORRENZA_INDENNIZZO")]
        public DateTime? CensitoIl { get; set; }

        [Column("ID_STANDARD")]
        public Int32 IdStandard { get; set; }

        [Column("VAL_STANDARD")]
        public Int32 ValoreStandard { get; set; }

        [Column("TEMPO_LAVORAZIONE")]
        public Decimal EvasoIn { get; set; }

        [Column("IMP_INDENNIZZO")]
        public Int32 Importo { get; set; }

        [Column("STADIO_INDENNIZZO")]
        public String StadioIndennizzo { get; set; }

        [Column("STATO")]
        public String Stato { get; set; }

        [Column("NOTE")]
        public String Note { get; set; }

        [Column("DT_INS")]
        public DateTime? DataInserimento { get; set; }

        [Column("UTE_INS")]
        public String UtenteInserimento { get; set; }

        [Column("DT_VALIDAZIONE")]
        public DateTime? GestitoIl { get; set; }

        [Column("UTE_VALIDAZIONE")]
        public String GestitoDa { get; set; }

        [Column("DT_MIGRAZIONE")]
        public DateTime? DataMigrazione { get; set; }

        [Column("UTE_MIGRAZIONE")]
        public String UtenteMigrazione { get; set; }

        [Column("DT_CHIUSURA")]
        public DateTime? DataChiusura { get; set; }

        [Column("DT_EMISSIONE_BOLLETTA")]
        public DateTime? EmissioneBolletta { get; set; }

        [Column("COD_BOLLETTA")]
        public String CodBolletta { get; set; }

        [Column("DT_RIMBORSO")]
        public DateTime? DataRimborso { get; set; }

        [Column("DT_ANNULLAMENTO")]
        public DateTime? DataAnnullamento { get; set; }

        [Column("UTE_ANNULLAMENTO")]
        public String UtenteAnnullamento { get; set; }

        [Column("ID_INDENNIZZO_DWH")]
        public Int32 IndennizzoDWH { get; set; }

        [Column("NUMERO_PRESTAZIONE")]
        public String NumeroPrestazione { get; set; }

        [Column("DESCRIZIONE_PRESTAZIONE")]
        public String DescrizionePrestazione { get; set; }

        [Column("DATA_INIZIO")]
        public DateTime? DataInizio { get; set; }

        [Column("DATA_FINE")]
        public DateTime? DataFine { get; set; }

        [Column("TIPO")]
        public String Tipo { get; set; }

        [Column("ESITO")]
        public String Esito { get; set; }

        [Column("DETTAGLIO_ESITO")]
        public String DettaglioEsito { get; set; }

        [Column("NOTE_ESITO")]
        public String NoteEsito { get; set; }

        [Column("CODICE_CAUSA")]
        public String CodiceCausa { get; set; }

        [Column("CODICE_SOTTOCAUSA")]
        public String CodiceSottocausa { get; set; }

        [Column("TIPO_STANDARD")]
        public String TipoStandard { get; set; }

        [Column("CODICE_PRESTAZIONE")]
        public String CodicePrestazione { get; set; }

        [Column("COD_GRUPPO")]
        public String CodiceGruppo { get; set; }

        [Column("COD_RINTRACCIABILITA")]
        public String CodiceRintracciabilita { get; set; }

        [Column("ERR_DATA_INIZIO")]
        public DateTime? ErrDataInizio { get; set; }

        [Column("ERR_DATA_FINE")]
        public DateTime? ErrDataFine { get; set; }

        [Column("ERR_TEMPO_LAVORAZIONE")]
        public Decimal ErrTempoLavorazione { get; set; }

        [Column("ERR_SOSPENSIONE")]
        public int ErrSospensione { get; set; }

        [Column("ERR_FLAG_STANDARD")]
        public String ErrFlagStandard { get; set; }

        [Column("DT_VALERR")]
        public DateTime? ValidazioneErrore { get; set; }

        [Column("UTE_VALERR")]
        public String UtenteErrore { get; set; }

        [Column("FLG_ERRORE")]
        public String FlagErrore { get; set; }

        [Column("FLG_UTE_NON_INDENNIZZABILE")]
        public String NonIndennizzabile { get; set; }

        [Column("FLAG_ORIGINE")]
        public String FlagOrigine { get; set; }

        [Column("FLG_IS_RETTIFICA")]
        public String FlagRettifica { get; set; }

        [Column("FLG_STATO")]
        public String FlagStato { get; set; }


        EccezioniFuoriStandardRepo _calcolofs = new EccezioniFuoriStandardRepo();
        public string InFuoriStandard
        {
            get { return (_calcolofs.CheckException(this.CodStandard, this.TipoStandard) ? this.EvasoIn < this.ValoreStandard ? "FS" : "S" : this.EvasoIn > this.ValoreStandard ? "FS" : "S"); }
        }

        public bool EccezioneFS
        {
            get { return (_calcolofs.CheckException(this.CodStandard, this.TipoStandard) ? true : false); }
        }

        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.IDFS.ToString(), this.CensitoIl); }
        }

        public string DisplayText
        {
            get { return string.Format("FuoriStandard num: {0}-{1}", this.IDFS.ToString(), this.CensitoIl); }
        }

    }
}
