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
    public class RettificaFuoriStandardService : IRettificaFuoriStandardService
    {
        IRettificaFuoriStandardRepo _rettificaFuoriStandardRepo = null;

        public FuoriStandard CercaStandardRettifica(String IdFS)
        {
            return _rettificaFuoriStandardRepo.CercaStandardRettifica(IdFS);
        }
        public RettificaFuoriStandard GetRettificaApprovataByID(String FuoriStandardID)
        {
            return _rettificaFuoriStandardRepo.GetRettificaApprovataByID(FuoriStandardID);
        }
        public ISubCollection<RettificaFuoriStandard> GetRettificheByID(String FuoriStandardID)
        {
            return _rettificaFuoriStandardRepo.GetRettificheByID(FuoriStandardID);
        }
        public RettificaFuoriStandardService(IRettificaFuoriStandardRepo RettificaFuoriStandardRepo)
        {
            _rettificaFuoriStandardRepo = RettificaFuoriStandardRepo;
        }
        public RettificaFuoriStandard GetRettificaApertaByID(String FuoriStandardID)
        {
            return _rettificaFuoriStandardRepo.GetRettificaApertaByID(FuoriStandardID);
        }
        public FuoriStandard GetFuoriStandardRettifica(String FuoriStandardID)
        {
            return _rettificaFuoriStandardRepo.GetFuoriStandardRettifica(FuoriStandardID);
        }
        public ISubCollection<FuoriStandard> GetFuoriStandardSenzaRettifica(String CodGruppo)
        {
            return _rettificaFuoriStandardRepo.GetFuoriStandardSenzaRettifica(CodGruppo);
        }
        public Dictionary<string, int> GetCountRettifiche(List<String> CodiciGruppo)
        {
            return _rettificaFuoriStandardRepo.GetCountRettifiche(CodiciGruppo);
        }
        public Dictionary<string, int> GetCountRettificheDaApprovare(List<String> CodiciGruppo, bool isProcessOwner)
        {
            return _rettificaFuoriStandardRepo.GetCountRettificheDaApprovare(CodiciGruppo, isProcessOwner);
        }
        public Dictionary<string, int> GetCountNotCanceled(List<String> CodiciGruppo, bool isProcessOwner)
        {
            return _rettificaFuoriStandardRepo.GetCountNotCanceled(CodiciGruppo, isProcessOwner);
        }
        public Dictionary<string, int> GetCountRettificheDaAnnullare(List<String> CodiciGruppo)
        {
            return _rettificaFuoriStandardRepo.GetCountRettificheDaAnnullare(CodiciGruppo);
        }
        public String ApprovaRettifica(String FuoriStandard, String CodiceCliente, String CodicePuf, String CodiceContratto, String Utente, String Note, String ErrDataInizio, String ErrDataFine, String ErrTempoLavorazione, String ErrSospensione, String ErrFlagStandard, String CodiceCausa, String CodiceSottocausa, String FlagErrore, String NonIndennizzabile, String NoteApprovatore, bool isProcessOwner)
        {
            return _rettificaFuoriStandardRepo.ApprovaRettifica(FuoriStandard, CodiceCliente, CodicePuf, CodiceContratto, Utente, Note, ErrDataInizio, ErrDataFine, ErrTempoLavorazione, ErrSospensione, ErrFlagStandard, CodiceCausa, CodiceSottocausa, FlagErrore, NonIndennizzabile, NoteApprovatore, isProcessOwner);
        }
        public String RifiutaRettifica(String FuoriStandard, String Note, String NoteApprovatore, String Utente)
        {
            return _rettificaFuoriStandardRepo.RifiutaRettifica(FuoriStandard, Note, NoteApprovatore, Utente);
        }
        public ISubCollection<FuoriStandard> RicercaAvanzataApprovatore(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiAperti)
        {
            return _rettificaFuoriStandardRepo.RicercaAvanzataApprovatore(DataInizio, DataFine, CodiceRintracciabilita, Cliente, CodGruppo, TuttiAperti);
        }
        public ISubCollection<RettificaFuoriStandard> CercaRettificheDaApprovare(String CodGruppo)
        {
            return _rettificaFuoriStandardRepo.CercaRettificheDaApprovare(CodGruppo);
        }
        public ISubCollection<RettificaFuoriStandard> CercaRettificheRifiutate(String CodGruppo)
        {
            return _rettificaFuoriStandardRepo.CercaRettificheRifiutate(CodGruppo);
        }
        public ISubCollection<FuoriStandard> CercaTutteLeRettifiche(String CodGruppo)
        {
            return _rettificaFuoriStandardRepo.CercaTutteLeRettifiche(CodGruppo);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovare(String CodGruppo)
        {
            ISubCollection<RettificaFuoriStandard> rettifichePendenti = _rettificaFuoriStandardRepo.CercaRettificheDaApprovare(CodGruppo);
            ISubCollection<FuoriStandard> fsDaApprovare = _rettificaFuoriStandardRepo.GetFuoriStandardFromIDs(rettifichePendenti.Items.Select(ee => ee.IDFuoriStandard.ToString()).ToList());
            return fsDaApprovare;
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovareByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner)
        {
            return _rettificaFuoriStandardRepo.CercaRettificheDaApprovareByFilter(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard, isProcessOwner);
        }

        public ISubCollection<FuoriStandard> CercaRettificheTutteByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool isProcessOwner)
        {
            return _rettificaFuoriStandardRepo.CercaRettificheTutteByFilter(tipologie, indicatore, codRintracciabilita, codCliente, dataInizio, dataFine, inStandard, isProcessOwner);
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardRifiutati(String CodGruppo)
        {
            ISubCollection<RettificaFuoriStandard> rettifichePendenti = _rettificaFuoriStandardRepo.CercaRettificheRifiutate(CodGruppo);
            ISubCollection<FuoriStandard> fsDaApprovare = _rettificaFuoriStandardRepo.GetFuoriStandardFromIDs(rettifichePendenti.Items.Select(ee => ee.IDFuoriStandard.ToString()).ToList());
            return fsDaApprovare;
        }
        public String RettificaPrimoStep(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard, String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note, Int32 flgRettifica)
        {
            return _rettificaFuoriStandardRepo.RettificaPrimoStep(idFuoriStandard, dataInizioAttivita, dataFineAttivita, quantita, quantitaSosp, fuoriStandard, causale, sottoCausale, Utente, NonIndennizzabile, CodiceCliente, CodicePuf, CodiceContratto, Note, flgRettifica);
        }
        public String GetManagerInfo(String CodGruppo)
        {
            return _rettificaFuoriStandardRepo.GetManagerInfo(CodGruppo);
        }
        //public String AnnullaPrestazione(String idFuoriStandard, String codiceCliente, String codiceContratto, String codicePuf, String note, String utente)
        //{
        //    return _rettificaFuoriStandardRepo.AnnullaPrestazione(idFuoriStandard, codiceCliente, codiceContratto, codicePuf, note, utente);
        //}
        public String AnnullaPrestazione(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
               String causale, String sottoCausale, String Utente, String NonIndennizzabile, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note)
        {
            return _rettificaFuoriStandardRepo.AnnullaPrestazione(idFuoriStandard, dataInizioAttivita, dataFineAttivita, quantita, quantitaSosp, fuoriStandard, causale, sottoCausale, Utente, NonIndennizzabile, CodiceCliente, CodicePuf, CodiceContratto, Note);
        }
    }
}
