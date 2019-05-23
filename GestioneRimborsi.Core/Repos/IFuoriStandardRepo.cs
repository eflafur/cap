using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IFuoriStandardRepo : IRepository<FuoriStandard>
    {
        ISubCollection<FuoriStandard> GetIndennizziAperti(String Utente);
        ISubCollection<FuoriStandard> GetFuoriStandardByTipologia(String CodGruppo, String Utente);
        Dictionary<string, int> GetCountFuoriStandard(List<String> CodiciGruppo);
        Dictionary<string, int> GetCountFuoriStandardDaValidare(List<String> CodiciGruppo);
        Dictionary<string, int> GetCountStoricoFuoriStandard(List<String> CodiciGruppo);
        Dictionary<string, int> GetCountFuoriStandardNonIndennizzabili(List<String> CodiciGruppo);
        Dictionary<string, int> GetCountFuoriStandardTutti(List<String> CodiciGruppo);
        ISubCollection<FuoriStandard> GetStoricoByTipologia(String CodGruppo, String Utente);
        List<String> GetTipologieByGrouping(List<String> Grouping);
        ISubCollection<FuoriStandard> GetIndennizziPagabili(String Utente);
        FuoriStandard GetFuoriStandardByID(String FuoriStandardID);
        FuoriStandard GetFuoriStandardByIdwh(String FuoriStandardID);
        FuoriStandard GetFuoriStandardStoricoByID(String FuoriStandardID);
        FuoriStandard GetIndennizzoPagabileByID(String FuoriStandardID);
        ClienteFuoriStandard GetCliFuoriStandard(String CodiceCliente);
        ISubCollection<String> GetContrattoByCliente(String CodiceCliente);
        String GetPUFByContratto(String CodiceContratto);
        ISubCollection<FuoriStandard> GetFuoriStandardStorico(String Tipologia);
        ISubCollection<FuoriStandard> GetStoricoTutti(String Tipologia);
        ISubCollection<FuoriStandard> GetStoricoChiusi(String Tipologia);
        ISubCollection<FuoriStandard> GetStoricoPendenti(String Tipologia);
        TipologiaFuoriStandard GetTipologiaStandard(Int32 IdStandard);
        ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard);
        ISubCollection<TipologiaFuoriStandard> GetIndicatoreByIds(List<String> idStandard);
        Dictionary<string, int> GetIndicatoreByGroup(String CodGruppo);
        Dictionary<string, int> GetIndicatoreByGroups(List<String> CodGruppo);
        Dictionary<string, int> GetIndicatoriForApprover(List<String> CodGruppo, bool isProcessOwner);
        Dictionary<string, int> GetIndicatoriForRettifiche(List<String> CodGruppo);
        Dictionary<string, int> GetIndicatoriValidazione(List<String> CodGruppo);
        Dictionary<string, int> GetIndicatoriNonIndennizzabili(List<String> CodGruppo);
        List<String> GetTipologieFilter();
        List<String> GetTipologieStoricoFilter();
        ISubCollection<CausaRitardoFuoriStandard> GetListaCategorie();
        CausaRitardoFuoriStandard GetCategoriaByCod(String CategoriaID);
        SottoCausaRitardoFS GetSottoCategoriaByCod(String SottocategoriaID);
        ISubCollection<SottoCausaRitardoFS> GetListaSottoCategorieByCategoria(String CategoriaId);
        bool IsApproved(String CategoriaId);
        String FirstValidation(String FuoriStandardDataCliente, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile);
        String ValidazioneNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile);
        String AssociaNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf);
        String RifiutaFuoriStandard(List<String> FuoriStandardDataCliente, String Utente, String Note);
        String IndennizziPagabili(List<String> FuoriStandardDataCliente, String Utente, String Note);
        String RegistrazioneFuoriStandard(String CodiceCliente, String CodicePUF, String CodiceStandard, DateTime DecorrenzaIndennizzo, String FuoriStandard, String IdSAFO, String Utente, String Note, String contratti, String CodGruppo, String ValStandard);
        ISubCollection<FuoriStandard> RicercaAvanzata(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi);
        ISubCollection<FuoriStandard> RicercaAvanzataStorico(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi);
        String AggiungiFile(String IdFS, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente);
        ISubCollection<FuoriStandardAllegato> GetElencoAllegati(String IdFS);
        bool DeleteAllegato(String NomeFile, String ServerPath, String TipoFile);
        String ClienteCensitoInCom(String CodCliente);
        ISubCollection<FuoriStandard> CercaFuoriStandardByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner);
        ISubCollection<FuoriStandard> CercaFuoriStandardStoricoByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard);
        ISubCollection<FuoriStandard> CercaFuoriStandardNonIndennizzabili(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool checkCliente, bool checkIndennizzabile);
        ISubCollection<ReportFuoriStandard> ExportReportPrestazioni(List<String> CodiciGruppo);
        ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovareByFilters(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner);
    }
}
