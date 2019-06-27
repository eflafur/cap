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
    [TableName("UT_RIMB_TEST")]
    public class TestataRimborso : IEntity
    {
        [Column("ID_RIMBORSO_INTEGRA")]
        public String IDRimborsoIntegra { get; set; }

        [Column("ANNO_RIMBORSO_INTEGRA")]
        public String AnnoRimborso { get; set; }

        [Column("NUMERO_DOCUMENTO")]
        public String NumeroDocumento { get; set; }

        [Column("TIPO_RIMBORSO")]
        public String TipoRimborso { get; set; }        

        [Column("DATA_CONFERMA")]
        public DateTime? DataConferma { get; set; }

        [Column("DATA_INSERIMENTO")]
        public DateTime? DataInserimento { get; set; }

        [Column("IMP_TOT_RIMB")]
        public Decimal ImportoTotaleRimborso { get; set; }

        [Column("STATO_DOCUMENTO")]
        public String StatoDocumento { get; set; }

        [Column("ANNO_DOCUMENTO")]
        public String AnnoDocumento { get; set; }

        [Column("TIPO_DOCUMENTO")]
        public String TipoDocumento { get; set; }

        [Column("UTENTE_INSERIMENTO")]
        public String UtenteInserimento { get; set; }

        [Column("DATA_ULT_VARIAZIONE")]
        public DateTime? DataUltimaVariazione { get; set; }

        [Column("UTENTE_ULT_VARIAZIONE")]
        public String UtenteUltimaVariazione { get; set; }

        [Column("UTENTE_CONFERMA")]
        public String UtenteConferma { get; set; }

        [Column("CODICE_CLIENTE")]
        public String CodiceCliente { get; set; }

        [Column("MANDATO")]
        public String Mandato { get; set; }

        [Column("PUNTO_FORNITURA")]
        public String CodicePuntoFornitura { get; set; }

        [Column("BENEFICIARIO")]
        public String Beneficiario { get; set; }

        [Column("INDIRIZZO_ASSEGNO")]
        public String IndirizzoAssegno { get; set; }

        [Column("CAP_ASSEGNO")]
        public String CapAssegno { get; set; }

        [Column("PROVINCIA_ASSEGNO")]
        public String ProvinciaAssegno { get; set; }

        [Column("LOCALITA_ASSEGNO")]
        public String LocalitaAssegno { get; set; }

        [Column("INTESTAZIONE_ALT")]
        public String Intestazione { get; set; }

        [Column("STRADA_ALT")]
        public String Strada { get; set; }

        [Column("CIVICO_ALT")]
        public String Civico { get; set; }

        [Column("CAP_ALT")]
        public String CAP { get; set; }

        [Column("PROVINCIA_ALT")]
        public String Provincia { get; set; }

        [Column("LOCALITA_ALT")]
        public String Localita { get; set; }

        [Column("ABI")]
        public String ABI { get; set; }

        [Column("CAB")]
        public String CAB { get; set; }

        [Column("CONTO_CORRENTE")]
        public String ContoCorrente { get; set; }

        [Column("CIN")]
        public String CIN { get; set; }

        [Column("NOTE")]
        public String Note { get; set; }

        [Column("USR_PROTOCOLLO")]
        public String UserProtocollo { get; set; }

        [Column("NUM_PROTOCOLLO")]
        public String NumeroProtocollo { get; set; }

        [Column("DATA_PROTOCOLLO")]
        public String DataProtocollo { get; set; }

        [Column("DATA_EMISSIONE")]
        public DateTime? DataEmissione { get; set; }

        [Column("NOME_FILE_GENERATO")]
        public String NomeFileGenerato { get; set; }

        [Column("FLAG_GENERATO")]
        public String FlagGenerazioneFile { get; set; }

        [Column("DATA_GENERAZIONE")]
        public DateTime? DataGenerazioneFile { get; set; }

        [Column("UTENTE_GENERAZIONE")]
        public String UtenteGenerazioneFile { get; set; }

        [Column("COD_AZIENDA")]
        public String CodiceAzienda { get; set; }        

        [Column("ID_SAFO")]
        public String IDSafo { get; set; }

        [Column("DATA_VALUTA")]
        public DateTime? DataValuta { get; set; }        

        [Ignore]
        public object EntityId
        {
            get { return string.Format("{0}-{1}", this.AnnoDocumento, this.NumeroDocumento); }
        }

        [Ignore]
        public string DisplayText
        {
            get { return string.Format("Rimborso num: {0}-{1}", this.AnnoDocumento, this.NumeroDocumento); }
        }
    }
}
