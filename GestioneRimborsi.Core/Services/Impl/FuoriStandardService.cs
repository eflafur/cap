using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestioneRimborsi.Core;
using PetaPoco;

namespace GestioneRimborsi.Core
{
    public class FuoriStandardService : IFuoriStandardService
    {
        IFuoriStandardRepo _fuoriStandardRepo = null;

        public FuoriStandardService(IFuoriStandardRepo FuoriStandardRepo)
        {
            _fuoriStandardRepo = FuoriStandardRepo;
        }

        public ISubCollection<FuoriStandard> GetIndennizziAperti(String Utente)
        {
            return _fuoriStandardRepo.GetIndennizziAperti(Utente);
        }

        public ISubCollection<FuoriStandard> GetFuoriStandardByTipologia(String CodGruppo, String Utente)
        {
            return _fuoriStandardRepo.GetFuoriStandardByTipologia(CodGruppo, Utente);
        }

        public Dictionary<string, int> GetCountFuoriStandard(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.GetCountFuoriStandard(CodiciGruppo);
        }

        public Dictionary<string, int> GetCountFuoriStandardDaValidare(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.GetCountFuoriStandardDaValidare(CodiciGruppo);
        }

        public Dictionary<string, int> GetCountStoricoFuoriStandard(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.GetCountStoricoFuoriStandard(CodiciGruppo);
        }

        public Dictionary<string, int> GetCountFuoriStandardNonIndennizzabili(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.GetCountFuoriStandardNonIndennizzabili(CodiciGruppo);
        }

        public Dictionary<string, int> GetCountFuoriStandardTutti(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.GetCountFuoriStandardTutti(CodiciGruppo);
        }

        public ISubCollection<FuoriStandard> GetStoricoByTipologia(String CodGruppo, String Utente)
        {
            return _fuoriStandardRepo.GetStoricoByTipologia(CodGruppo, Utente);
        }

        public List<String> GetTipologieByGrouping(List<String> Grouping)
        {
            return _fuoriStandardRepo.GetTipologieByGrouping(Grouping);
        }

        public ISubCollection<FuoriStandard> GetIndennizziPagabili(String Utente)
        {
            return _fuoriStandardRepo.GetIndennizziPagabili(Utente);
        }

        public FuoriStandard GetFuoriStandardByID(String FuoriStandardID)
        {
            return _fuoriStandardRepo.GetFuoriStandardByID(FuoriStandardID);
        }

        public FuoriStandard GetFuoriStandardByIdwh(String FuoriStandardID)
        {
            return _fuoriStandardRepo.GetFuoriStandardByIdwh(FuoriStandardID);
        }

        public FuoriStandard GetFuoriStandardStoricoByID(String FuoriStandardID)
        {
            return _fuoriStandardRepo.GetFuoriStandardStoricoByID(FuoriStandardID);
        }

        public FuoriStandard GetIndennizzoPagabileByID(String FuoriStandardID)
        {
            return _fuoriStandardRepo.GetIndennizzoPagabileByID(FuoriStandardID);
        }

        public ClienteFuoriStandard GetCliFuoriStandard(String CodiceCliente)
        {
            return _fuoriStandardRepo.GetCliFuoriStandard(CodiceCliente);
        }

        public ISubCollection<String> GetContrattoByCliente(String CodiceCliente)
        {
            return _fuoriStandardRepo.GetContrattoByCliente(CodiceCliente);
        }

        public String GetPUFByContratto(String CodiceContratto)
        {
            return _fuoriStandardRepo.GetPUFByContratto(CodiceContratto);
        }

        public ISubCollection<FuoriStandard> GetFuoriStandardStorico(String Tipologia)
        {
            return _fuoriStandardRepo.GetFuoriStandardStorico(Tipologia);
        }

        public ISubCollection<FuoriStandard> GetStoricoTutti(String Tipologia)
        {
            return _fuoriStandardRepo.GetStoricoTutti(Tipologia);
        }

        public ISubCollection<FuoriStandard> GetStoricoChiusi(String Tipologia)
        {
            return _fuoriStandardRepo.GetStoricoChiusi(Tipologia);
        }

        public ISubCollection<FuoriStandard> GetStoricoPendenti(String Tipologia)
        {
            return _fuoriStandardRepo.GetStoricoPendenti(Tipologia);
        }

        public TipologiaFuoriStandard GetTipologiaStandard(Int32 IdStandard)
        {
            return _fuoriStandardRepo.GetTipologiaStandard(IdStandard);
        }

        public ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard)
        {
            return _fuoriStandardRepo.GetTipologieDesc(CodStandard);
        }

        public ISubCollection<TipologiaFuoriStandard> GetIndicatoreByIds(List<String> idStandard)
        {
            return _fuoriStandardRepo.GetIndicatoreByIds(idStandard);
        }

        public Dictionary<string, int> GetIndicatoreByGroup(String CodGruppo)
        {
            return _fuoriStandardRepo.GetIndicatoreByGroup(CodGruppo);
        }

        public Dictionary<string, int> GetIndicatoreByGroups(List<String> CodGruppo)
        {
            return _fuoriStandardRepo.GetIndicatoreByGroups(CodGruppo);
        }

        public Dictionary<string, int> GetIndicatoriForRettifiche(List<String> CodGruppo)
        {
            return _fuoriStandardRepo.GetIndicatoriForRettifiche(CodGruppo);
        }

        public Dictionary<string, int> GetIndicatoriValidazione(List<String> CodGruppo)
        {
            return _fuoriStandardRepo.GetIndicatoriValidazione(CodGruppo);
        }

        public Dictionary<string, int> GetIndicatoriForApprover(List<String> CodGruppo, bool isProcessOwner)
        {
            return _fuoriStandardRepo.GetIndicatoriForApprover(CodGruppo, isProcessOwner);
        }

        public Dictionary<string, int> GetIndicatoriNonIndennizzabili(List<String> CodGruppo)
        {
            return _fuoriStandardRepo.GetIndicatoriNonIndennizzabili(CodGruppo);
        }

        public List<String> GetTipologieFilter()
        {
            return _fuoriStandardRepo.GetTipologieFilter();
        }

        public List<String> GetTipologieStoricoFilter()
        {
            return _fuoriStandardRepo.GetTipologieStoricoFilter();
        }

        public ISubCollection<CausaRitardoFuoriStandard> GetListaCategorie()
        {
            return _fuoriStandardRepo.GetListaCategorie();
        }

        public CausaRitardoFuoriStandard GetCategoriaByCod(String CategoriaID)
        {
            return _fuoriStandardRepo.GetCategoriaByCod(CategoriaID);
        }

        public ISubCollection<SottoCausaRitardoFS> GetListaSottoCategorieByCategoria(String CategoriaId)
        {
            return _fuoriStandardRepo.GetListaSottoCategorieByCategoria(CategoriaId);
        }

        public SottoCausaRitardoFS GetSottoCategoriaByCod(String SottocategoriaID)
        {
            return _fuoriStandardRepo.GetSottoCategoriaByCod(SottocategoriaID);
        }

        public bool IsApproved(String CategoriaId)
        {
            return _fuoriStandardRepo.IsApproved(CategoriaId);
        }

        public String FirstValidation(String FuoriStandardDataCliente, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile)
        {
            return _fuoriStandardRepo.FirstValidation(FuoriStandardDataCliente, CodiceCausa, CodiceSottocausa, Utente, Note, NonIndennizzabile);
        }

        public String ValidazioneNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile)
        {
            return _fuoriStandardRepo.ValidazioneNuovoCliente(IdFS, CodiceCliente, CodiceContratto, CodicePuf, CodiceCausa, CodiceSottocausa, Utente, Note, NonIndennizzabile);
        }

        public String AssociaNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf)
        {
            return _fuoriStandardRepo.AssociaNuovoCliente(IdFS, CodiceCliente, CodiceContratto, CodicePuf);
        }

        public String RifiutaFuoriStandard(List<String> FuoriStandardDataCliente, String Utente, String Note)
        {
            return _fuoriStandardRepo.RifiutaFuoriStandard(FuoriStandardDataCliente, Utente, Note);
        }

        public String IndennizziPagabili(List<String> FuoriStandardDataCliente, String Utente, String Note)
        {
            return _fuoriStandardRepo.IndennizziPagabili(FuoriStandardDataCliente, Utente, Note);
        }

        public String RegistrazioneFuoriStandard(String CodiceCliente, String CodicePUF, String CodiceStandard, DateTime DecorrenzaIndennizzo, String FuoriStandard, String IdSAFO, String Utente, String Note, String contratti, String CodGruppo, String ValStandard)
        {
            return _fuoriStandardRepo.RegistrazioneFuoriStandard(CodiceCliente, CodicePUF, CodiceStandard, DecorrenzaIndennizzo, FuoriStandard, IdSAFO, Utente, Note, contratti, CodGruppo, ValStandard);
        }

        public ISubCollection<FuoriStandard> RicercaAvanzata(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            return _fuoriStandardRepo.RicercaAvanzata(DataInizio, DataFine, CodiceRintracciabilita, Cliente, CodGruppo, TuttiPendChiusi);
        }

        public ISubCollection<FuoriStandard> RicercaAvanzataStorico(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            return _fuoriStandardRepo.RicercaAvanzataStorico(DataInizio, DataFine, CodiceRintracciabilita, Cliente, CodGruppo, TuttiPendChiusi);
        }

        public String AggiungiFile(String IdFS, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente)
        {
            return _fuoriStandardRepo.AggiungiFile(IdFS, file, NomefileOriginale, Extension, ServerPath, Utente);
        }

        public ISubCollection<FuoriStandardAllegato> GetElencoAllegati(String IdFS)
        {
            return _fuoriStandardRepo.GetElencoAllegati(IdFS);
        }

        public bool DeleteAllegato(String NomeFile, String ServerPath, String TipoFile)
        {
            return _fuoriStandardRepo.DeleteAllegato(NomeFile, ServerPath, TipoFile);
        }

        public String ClienteCensitoInCom(String CodCliente)
        {
            return _fuoriStandardRepo.ClienteCensitoInCom(CodCliente);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner)
        {
            return _fuoriStandardRepo.CercaFuoriStandardByFilter(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard, stato, isProcessOwner);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardStoricoByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard)
        {
            return _fuoriStandardRepo.CercaFuoriStandardStoricoByFilter(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardNonIndennizzabili(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool checkCliente, bool checkIndennizzabile)
        {
            return _fuoriStandardRepo.CercaFuoriStandardNonIndennizzabili(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard, checkCliente, checkIndennizzabile);
        }

        public ISubCollection<ReportFuoriStandard> ExportReportPrestazioni(List<String> CodiciGruppo)
        {
            return _fuoriStandardRepo.ExportReportPrestazioni(CodiciGruppo);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovareByFilters(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner)
        {
            return _fuoriStandardRepo.CercaFuoriStandardDaApprovareByFilters(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard, stato, isProcessOwner);
        }

    }
}
