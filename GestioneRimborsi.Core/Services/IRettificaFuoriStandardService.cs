using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IRettificaFuoriStandardService : IRevoService
    {
        FuoriStandard CercaStandardRettifica(String IdFS);
        RettificaFuoriStandard GetRettificaApprovataByID(String FuoriStandardID);
        ISubCollection<RettificaFuoriStandard> GetRettificheByID(String FuoriStandardID);
        RettificaFuoriStandard GetRettificaApertaByID(String FuoriStandardID);
        FuoriStandard GetFuoriStandardRettifica(String FuoriStandardID);
        ISubCollection<FuoriStandard> GetFuoriStandardSenzaRettifica(String CodGruppo);
        Dictionary<string, int> GetCountRettifiche(List<String> CodiciGruppo);
        Dictionary<string, int> GetCountRettificheDaApprovare(List<String> CodiciGruppo, bool isProcessOwner);
        Dictionary<string, int> GetCountNotCanceled(List<String> CodiciGruppo, bool isProcessOwner);
        Dictionary<string, int> GetCountRettificheDaAnnullare(List<String> CodiciGruppo);
        String ApprovaRettifica(String FuoriStandard, String CodiceCliente, String CodicePuf, String CodiceContratto, String Utente, String Note, String ErrDataInizio, String ErrDataFine, String ErrTempoLavorazione, String ErrSospensione, String ErrFlagStandard, String CodiceCausa, String CodiceSottocausa, String FlagErrore, String NonIndennizzabile, String NoteApprovatore, bool isProcessOwner);
        String RifiutaRettifica(String FuoriStandard, String Note, String NoteApprovatore, String Utente);
        ISubCollection<FuoriStandard> RicercaAvanzataApprovatore(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiAperti);
        ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovare(String CodGruppo);
        ISubCollection<FuoriStandard> CercaFuoriStandardRifiutati(String CodGruppo);
        ISubCollection<FuoriStandard> CercaTutteLeRettifiche(String CodGruppo);
        ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovareByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner);
        ISubCollection<FuoriStandard> CercaRettificheTutteByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner);
        String RettificaPrimoStep(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard, String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note, Int32 flgRettifica);
        String GetManagerInfo(String CodGruppo);
        //String AnnullaPrestazione(String idFuoriStandard, String codiceCliente, String codiceContratto, String codicePuf, String note, String utente);
        String AnnullaPrestazione(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
              String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note);
    }
}
