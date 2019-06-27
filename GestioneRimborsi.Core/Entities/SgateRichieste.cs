using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using PetaPoco;
//using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace GestioneRimborsi.Core
{
    [TableName("GRI_BI_REQUEST")]
    [PrimaryKey("BI_REQ_ID", autoIncrement = true)]
    public class SgateRichieste : IEntity
    {
        [Column("BI_REQ_ID") ]
        public int Id { get; set; }  

        [Column("BI_LOTCAP_ID")]
        public int lotCapId { get; set; }

        [Column("BI_PROTDOMANDA") ]
        public int ProtDomanda { get; set; }

        [Column("BI_PROTRICHIESTA")]
        public int ProtRichiesta { get; set; }

        [Column("BI_TIPODOMANDA")]
        public string TipoDomanda { get; set; }

        [Column("BI_REQ_NOME")]
        public string ReqNome { get; set; }

        [Column("BI_REQ_COGNOME")]
        public string ReqCognome { get; set; }

        [Column("BI_REQ_CF")]
        public string ReqCf { get; set; }

        [Column("BI_REQ_TIPODOC")]
        public string ReqTipoDoc { get; set; }

        [Column("BI_REQ_NUMERODOC")]
        public string ReqNumeroDoc { get; set; }

        [Column("BI_REQ_DATADOC")]
        public DateTime ReqDataDoc { get; set; }

        [Column("BI_REQ_ENTERILDOC")]
        public string ReqEnteRilsascioDoc { get; set; }

        [Column("BI_REQ_ISTATCOMUNE")]
        public string ReqIstatComune { get; set; }

        [Column("BI_REQ_AREACIR")]
        public string ReqEnteAreaCir { get; set; }

        [Column("BI_REQ_CIVICO")]
        public string ReqCivico { get; set; }

        [Column("BI_REQ_EDIFICIO")]
        public string ReqEdificio { get; set; }

        [Column("BI_REQ_SCALA")]
        public string ReqScala { get; set; }

        [Column("BI_REQ_INTERNO")]
        public string ReqInterno { get; set; }

        [Column("BI_REQ_CAP")]
        public string ReqCap { get; set; }

        [Column("BI_COD_UTENTEIND")]
        public string CodUtenteInd { get; set; }

        [Column("BI_IND_NOME")]
        public string IndNome { get; set; }

        [Column("BI_IND_COGNOME")]
        public string IndCognome { get; set; }

        [Column("BI_IND_CF")]
        public string IndCf { get; set; }

        [Column("BI_IND_TIPODOC")]
        public string IndTipoDoc { get; set; }

        [Column("BI_IND_NUMERODOC")]
        public string IndNumeroDoc { get; set; }

        [Column("BI_IND_DATADOC")]
        public DateTime IndDataDoc { get; set; }

        [Column("BI_IND_ENTERILDOC")]
        public string IndEnteRilDoc { get; set; }

        [Column("BI_IND_ISTATCOMUNE")]
        public string IndIstatComune { get; set; }

        [Column("BI_IND_AREACIR")]
        public string IndAreaCirc { get; set; }

        [Column("BI_IND_CIVICO")]
        public string IndCivico { get; set; }

        [Column("BI_IND_EDIFICIO")]
        public string IndEdificio { get; set; }

        [Column("BI_IND_SCALA")]
        public string IndScala { get; set; }

        [Column("BI_IND_INTERNO")]
        public string IndInterno { get; set; }

        [Column("BI_IND_CAP")]
        public string IndCap { get; set; }

        [Column("BI_COD_UTENTECENTR")]
        public string CodUtenteCentr { get; set; }

        [Column("BI_CENTR_IBAN")]
        public string CentrIban { get; set; }

        [Column("BI_CENTR_DENCOND")]
        public string CentrDenCondominio { get; set; }

        [Column("BI_CENTR_PLURIFAM")]
        public string CentrEdificioPlurifam { get; set; }

        [Column("BI_CENTR_ISTATCOMUNE")]
        public string CentrIstatComune { get; set; }

        [Column("BI_CENTR_AREACIR")]
        public string CentrAreaCircolazione { get; set; }

        [Column("BI_CENTR_CIVICO")]
        public string CentrCivico { get; set; }

        [Column("BI_CENTR_EDIFICIO")]
        public string CentrEdificio { get; set; }

        [Column("BI_CENTR_SCALA")]
        public string CentrScala { get; set; }

        [Column("BI_CENTR_INTERNO")]
        public string CentrInterno { get; set; }

        [Column("BI_CENTR_CAP")]
        public string CentrCap { get; set; }

        [Column("BI_CENTR_ISTATCOMUNE2")]
        public string CentrIstatComune2 { get; set; }

        [Column("BI_CENTR_AREACIR2")]
        public string CentrAreaCir2 { get; set; }

        [Column("BI_CENTR_CIVICO2")]
        public string CentrCivico2 { get; set; }

        [Column("BI_CENTR_EDIFICIO2")]
        public string CentrEdificio2 { get; set; }

        [Column("BI_CENTR_SCALA2")]
        public string CentrScala2 { get; set; }

        [Column("BI_CENTR_INTERNO2")]
        public string CentrInterno2 { get; set; }

        [Column("BI_CENTR_CAP2")]
        public string CentrCap2 { get; set; }

        [Column("BI_CENTR_ISTATCOMUNE3")]
        public string CentrIstatComune3 { get; set; }

        [Column("BI_CENTR_AREACIRC3")]
        public string CentrAreaCirc3 { get; set; }

        [Column("BI_CENTR_CIVICO3")]
        public string CentrCivico3 { get; set; }

        [Column("BI_CENTR_EDIFICIO3")]
        public string CentrEdificio3 { get; set; }

        [Column("BI_CENTR_SCALA3")]
        public string CentrScalae3 { get; set; }

        [Column("BI_CENTR_INTERNO3")]
        public string CentrInterno3 { get; set; }

        [Column("BI_CENTR_CAP3")]
        public string CentrCap3 { get; set; }

        [Column("BI_CAPOPFAM_ANAG")]
        public string CompFamigliaAnag { get; set; }

        [Column("BI_DATA_DISPONIBILITA")]
        public DateTime DataDisponibilita { get; set; }

        [Column("BI_DATA_AMMISSIONE")]
        public DateTime DataAmmissione { get; set; }

        [Column("BI_DATA_PRESENTAZIONE")]
        public DateTime DataPresentazione { get; set; }

        [Column("BI_DATAINIZIO_AGEV")]
        public DateTime DataInizioAgev{ get; set; }

        [Column("BI_DATAFINE_AGEV")]
        public DateTime DataFineAgev { get; set; }

        [Column("BI_ALLINEAMENTO"),Required ]
        public bool Allineamento { get; set; }

        [Column("BI_FRUIZIONECONT"),Required]
        public bool FruizioneCont { get; set; }

        [Column("BI_DATA_ACQUISIZIONE")]
        public DateTime DataAcquisizione { get; set; }

        [Column("BI_DATA_PRESAINCARICO")]
        public DateTime DataPresaIncarico { get; set; }

        [Column("BI_FORZATO"),Required]
        public bool Forzato { get; set; }

        [Column("BI_ESITOD")]
        public string EsitoD { get; set; }

        [Column("BI_DATAINVIO_ESITOD")]
        public DateTime DataInvioEsitoD { get; set; }

        [Column("BI_ESITOS")]
        public string EsitoS { get; set; }

        [Column("BI_CODERRORES")]
        public int CodErroreS { get; set; }

        [Column("BI_DETTAGLIO_ERRORES")]
        public string DettaglioErrores { get; set; }

        [Column("BI_DATA_CHIUSURAS")]
        public DateTime DataChiusuras { get; set; }

        //[Column("BI_CODICEFORNITURA")]
        //public string  CodiceFornitura { get; set; }

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
