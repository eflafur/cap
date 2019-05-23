using GestioneRimborsi.Core;
using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using GruppoCap.Core.Mvc.Logging;
using GruppoCap.Core.Mvc;
using System.Text;
using System.Data;
using GestioneRimborsi.Web.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GruppoCap;

namespace GestioneRimborsi.Web.Controllers
{
    public class FuoriStandardController : RevoController
    {
        private IFuoriStandardService _fuoriStandardService = null;
        private IRettificaFuoriStandardService _rettificaFuoriStandardService = null;
        private IRettificaSospensioneService _rettificaSospensioneService = null;
        private IAnniBloccatiService _anniBloccatiService = null;

        public FuoriStandardController(IFuoriStandardService fuoriStandardService, IRettificaFuoriStandardService rettificaFuoriStandardService, IRettificaSospensioneService rettificaSospensioneService, IAnniBloccatiService anniBloccatiService)
        {
            _fuoriStandardService = fuoriStandardService;
            _rettificaFuoriStandardService = rettificaFuoriStandardService;
            _rettificaSospensioneService = rettificaSospensioneService;
            _anniBloccatiService = anniBloccatiService;
        }

        [Authorize]
        public ActionResult Index()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard"))
                return View();
            //else return new RedirectResult(CommonUrls.ErrorPage);
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        public ActionResult Rettifiche()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        public ActionResult ReportPrestazioni()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.reportPrestazioni"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        public FileResult GuidaGFS()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/PDF/GuidaGFS.pdf"));
            string fileName = "GuidaFuoriStandard.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }

        [Authorize]
        public ActionResult AnteprimaPrestazioni()
        {
            try
            {
                ISubCollection<ReportFuoriStandard> _prestazioni = null;
                var totaleGruppi = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
                _prestazioni = _fuoriStandardService.ExportReportPrestazioni(totaleGruppi);
                return PartialView("~/Views/FuoriStandard/_reportPrestazioni.cshtml", _prestazioni);
            }
            catch (Exception ex)
            {
                return PartialMessage("errore nel caricamento dei dati..." + ex.Message);
            }
        }

        [Authorize]
        public ActionResult ApprovazioneRettifiche()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.approvazione") || RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.annullaPrestazione"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        private string CheckUtente(string Utente)
        {
            if (String.IsNullOrEmpty(Utente) || Utente == "undefined" || (Utente != RevoRequest.CurrentUser.UserId && !RevoRequest.CurrentUser.IsSuperUser()))
                Utente = RevoRequest.CurrentUser.UserId;
            return Utente;
        }

        [Authorize]
        public ActionResult CercaFuoriStandardAperti()
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = null;
                Dictionary<string, int> countIndennizzi = null;
                countIndennizzi = _fuoriStandardService.GetCountFuoriStandard(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
                if (_fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).Count == 1 || countIndennizzi.Count == 1)
                {
                    if (countIndennizzi.Count == 1)
                        _indennizzi = _fuoriStandardService.GetFuoriStandardByTipologia(_fuoriStandardService.GetTipologieByGrouping(countIndennizzi.Keys.ToList<String>()).FirstOrDefault(), RevoRequest.CurrentUser.UserId);
                    else
                        _indennizzi = _fuoriStandardService.GetFuoriStandardByTipologia(_fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).FirstOrDefault(), RevoRequest.CurrentUser.UserId);
                    return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _indennizzi);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }


        [Authorize]
        public ActionResult CercaFuoriStandardByTipologia(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetFuoriStandardByTipologia(CodGruppo, RevoRequest.CurrentUser.UserId);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _indennizzi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }


        [Authorize]
        public ActionResult CercaIndennizziPagabili()
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetIndennizziPagabili(RevoRequest.CurrentUser.UserId);
                return PartialView("~/Views/FuoriStandard/_elencoIndennizziPagabili.cshtml", _indennizzi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult CercaFuoriStandardStoricoByTipologia()
        {
            try
            {
                if (_fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).Count == 1)
                {
                    ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetFuoriStandardStorico(_fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).FirstOrDefault());
                    return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _indennizzi);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult CercaStoricoChiusi(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetStoricoChiusi(CodGruppo);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _indennizzi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult CercaStoricoPendenti(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetStoricoPendenti(CodGruppo);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _indennizzi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult CercaStoricoTutti(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetStoricoTutti(CodGruppo);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _indennizzi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricarca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult RegistraFuoriStandard(String Utente)
        {
            if (RevoRequest.CurrentUser.IsPrivileged)
                return View();
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        public ActionResult IndennizziPagabili(String Utente)
        {
            if (RevoRequest.CurrentUser.GroupingCodes.Contains("gri.OperatoreIndennizzi"))
            {
                ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.GetIndennizziPagabili(RevoRequest.CurrentUser.UserId);
                return View("IndennizziPagabili", model: _indennizzi);
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult ValidaFuoriStandardFirstStep(String FuoriStandardDataCliente, String CodiceCausa, String CodiceSottocausa, String Note, String NonIndennizzabile)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    String _validazione = _fuoriStandardService.FirstValidation(FuoriStandardDataCliente, CodiceCausa, CodiceSottocausa, RevoRequest.CurrentUser.UserId, Note, NonIndennizzabile);
                    return Content(_validazione);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la validazione...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la validazione..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult ValidaFuoriStandardNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf, String CodiceCausa, String CodiceSottocausa, String Note, String NonIndennizzabile)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    String _validazione = _fuoriStandardService.ValidazioneNuovoCliente(IdFS, CodiceCliente, CodiceContratto, CodicePuf, CodiceCausa, CodiceSottocausa, RevoRequest.CurrentUser.UserId, Note, NonIndennizzabile);
                    return Content(_validazione);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la validaione...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la validazione..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult RettificaPrimoStep(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
               String causale, String sottoCausale, String note, String nonIndennizzabile, String codiceCliente, String codicePuf, String codiceContratto, Int32 flgRettifica)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    String _rettifica = _rettificaFuoriStandardService.RettificaPrimoStep(idFuoriStandard, dataInizioAttivita, dataFineAttivita, quantita, quantitaSosp, fuoriStandard, causale, sottoCausale,
                                        RevoRequest.CurrentUser.UserId, nonIndennizzabile, codiceCliente, codicePuf, codiceContratto, note, flgRettifica);
                    if (_rettifica == String.Empty)
                    {
                        String esitoEmail = SendMailToApprover(idFuoriStandard);
                    }
                    return Content(_rettifica);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la rettifica...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la rettifica..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult RifiutaFuoriStandard(List<String> FuoriStandardDataCliente, String Note)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.approvazione.update"))
            {
                try
                {
                    String _rifiutati = _fuoriStandardService.RifiutaFuoriStandard(FuoriStandardDataCliente, RevoRequest.CurrentUser.UserId, Note);
                    return Content(_rifiutati);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la validazione...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la validazione..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult IndePagabili(List<String> FuoriStandardDataCliente, String Note)
        {
            try
            {
                String _pagabili = _fuoriStandardService.IndennizziPagabili(FuoriStandardDataCliente, RevoRequest.CurrentUser.UserId, Note);
                return Content(_pagabili);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RegistrazioneFuoriStandard(String CodiceCliente, String CodicePUF, String Tipologia, DateTime? calendar2, String FuoriStandard, String IdSAFO, String Note, String contratti, String CodGruppo, String ValStandard)
        {
            if (RevoRequest.CurrentUser.IsPrivileged)
            {
                if (Tipologia == "-1")
                    Tipologia = "";
                if (CodGruppo == "-1")
                    CodGruppo = "";

                try
                {
                    String _registra = _fuoriStandardService.RegistrazioneFuoriStandard(CodiceCliente, CodicePUF, Tipologia, (DateTime)calendar2, FuoriStandard, IdSAFO, RevoRequest.CurrentUser.UserId, Note, contratti, CodGruppo, ValStandard);
                    if (_registra == String.Empty)
                    {
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else return Content(_registra);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la registrazione...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la registrazione..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        public ActionResult SchedaFuoriStandard(String CodiceIndennizzo, String FuoriStandard, bool SolaLettura)
        {
            try
            {
                //DateTime date = DateTime.FromOADate(43550);
                bool _checkSolaLettura = _anniBloccatiService.CheckIsBlocked(CodiceIndennizzo);
                if (SolaLettura == false)
                    SolaLettura = _checkSolaLettura;
                return PartialView("~/Views/FuoriStandard/_schedaFuoriStandard.cshtml", new SchedaFuoriStandardModel() { IDFS = CodiceIndennizzo, FuoriStandard = FuoriStandard, SolaLettura = SolaLettura });
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult SchedaErroreUmano(String CodiceIndennizzo)
        {
            return PartialView("~/Views/FuoriStandard/_approvazioneErroreUmanoModal.cshtml", CodiceIndennizzo);
        }

        [Authorize]
        public ActionResult SchedaCausaCAP(String CodiceIndennizzo)
        {
            String censito = String.Empty;
            if (_fuoriStandardService.GetFuoriStandardByID(CodiceIndennizzo) != null)
                censito = _fuoriStandardService.ClienteCensitoInCom(_fuoriStandardService.GetFuoriStandardByID(CodiceIndennizzo).CodCliente);
            if (String.IsNullOrEmpty(censito))
            {
                return PartialView("~/Views/FuoriStandard/_clienteNonCensitoModal.cshtml", CodiceIndennizzo);
            }
            else
                return Content("");
        }

        [Authorize]
        public ActionResult SchedaAssociaCliente(String CodiceIndennizzo)
        {
            return PartialView("~/Views/FuoriStandard/_causaCAPModal.cshtml", CodiceIndennizzo);
        }

        [Authorize]
        public ActionResult SchedaFuoriStandardStorico(String CodiceIndennizzo)
        {
            return PartialView("~/Views/FuoriStandard/_schedaStorico.cshtml", CodiceIndennizzo);
        }

        [Authorize]
        public ActionResult SchedaClienteStorico(String CodiceCliente)
        {
            return PartialView("~/Views/FuoriStandard/_schedaClienteStorico.cshtml", CodiceCliente);
        }

        [Authorize]
        public ActionResult FiltraDatiSottoCategoria(String OptionCategoria, bool firstTime)
        {
            return PartialView("~/Views/FuoriStandard/_filtroCategorieIndennizzi.cshtml", new CategorieFuoriStandardModel() { Categoria_Id = OptionCategoria, firstTime = firstTime });
        }

        [Authorize]
        public ActionResult FiltraDatiIndicatori(String view, String OptionGruppo, bool firstTime)
        {
            return PartialView("~/Views/FuoriStandard/_filtroIndicatori.cshtml", new IndicatoriModel() { view = view, Indicatore_Id = OptionGruppo, firstTime = firstTime });
        }

        [Authorize]
        public ActionResult FiltraDatiContratti(String CodiceCliente)
        {
            return PartialView("~/Views/FuoriStandard/_filtroContratti.cshtml", CodiceCliente);
        }

        [Authorize]
        public ActionResult FiltraDatiPUF(String CodiceContratto)
        {
            String _puf = "";
            if (CodiceContratto != "-1")
            {
                _puf = _fuoriStandardService.GetPUFByContratto(CodiceContratto);
            }
            return Content(_puf);
        }

        [Authorize]
        public ActionResult VisualizzaStorico()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.storico"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        public ActionResult RecuperoFuoriStandard()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.nonIndennizzabili"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        public ActionResult GestisciRettifiche()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.gestisciRettifiche"))
                return View();
            else return new RedirectResult(CommonUrls.BaseUrl);
        }

        [Authorize]
        [HttpPost]
        public ActionResult IsApproved(String CodiceCategoria)
        {
            bool _isApproved = _fuoriStandardService.IsApproved(CodiceCategoria);
            return Content(_isApproved.ToString());
        }

        [Authorize]
        public ActionResult RicercaAvanzata(DateTime calendar, DateTime calendar2, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.RicercaAvanzata(calendar, new DateTime(calendar2.Year, calendar2.Month, calendar2.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), CodiceRintracciabilita, Cliente, CodGruppo, TuttiPendChiusi);
            return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _indennizzi);
        }

        [Authorize]
        public ActionResult RicercaAvanzataStorico(DateTime calendar, DateTime calendar2, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            ISubCollection<FuoriStandard> _indennizzi = _fuoriStandardService.RicercaAvanzataStorico(calendar, new DateTime(calendar2.Year, calendar2.Month, calendar2.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), CodiceRintracciabilita, Cliente, CodGruppo, TuttiPendChiusi);
            return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _indennizzi);
        }

        [Authorize]
        public ActionResult RicercaAvanzataApprovatore(DateTime calendar, DateTime calendar2, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiAperti)
        {
            ISubCollection<FuoriStandard> _indennizzi = _rettificaFuoriStandardService.RicercaAvanzataApprovatore(calendar, new DateTime(calendar2.Year, calendar2.Month, calendar2.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), CodiceRintracciabilita, Cliente, CodGruppo, TuttiAperti);
            return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _indennizzi);
        }

        [Authorize]
        public ActionResult CercaFuoriStandardDaApprovare(String CodGruppo)
        {
            try
            {
                if (CodGruppo == "" && _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).Count == 1)
                {
                    ISubCollection<FuoriStandard> _indennizzi = _rettificaFuoriStandardService.CercaFuoriStandardDaApprovare(_fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>()).FirstOrDefault());

                    return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _indennizzi);
                }
                else if (!String.IsNullOrEmpty(CodGruppo))
                {
                    ISubCollection<FuoriStandard> _rettifiche = _rettificaFuoriStandardService.CercaFuoriStandardDaApprovare(CodGruppo);

                    return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _rettifiche);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        public ActionResult CercaTutteLeRettifiche(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _rettifiche = _rettificaFuoriStandardService.CercaTutteLeRettifiche(CodGruppo);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _rettifiche);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        public ActionResult CercaFuoriStandardRifiutati(String CodGruppo)
        {
            try
            {
                ISubCollection<FuoriStandard> _rettifiche = _rettificaFuoriStandardService.CercaFuoriStandardRifiutati(CodGruppo);

                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _rettifiche);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ApprovaRettifica(String FuoriStandard, String CodiceCliente, String CodicePuf, String CodiceContratto, String Note, String ErrDataInizio, String ErrDataFine, String ErrTempoLavorazione, String ErrSospensione, String ErrFlagStandard, String CodiceCausa, String CodiceSottocausa, String FlagErrore, String NonIndennizzabile, String NoteApprovatore)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.approvazione.update"))
            {
                try
                {
                    bool checkIsBlockedApprovazione = _anniBloccatiService.CheckIsBlockedApprovazione(FuoriStandard, ErrDataInizio, ErrDataFine);
                    if (checkIsBlockedApprovazione)
                        return Content("Impossibile Approvare: le date di inizio/fine attività ricadono nell'anno di competenza bloccato.");
                    String _userGroupId = String.Empty;
                    Object _o = RevoContext.PermissionManager.GetFirstPermissionGroupIdForUser(RevoRequest.CurrentUser.UserId);
                    if (_o != null)
                        _userGroupId = _o.ToString();
                    string pemString = (RevoContext.PermissionManager.GetPermissionGroup(_userGroupId) != null ? RevoContext.PermissionManager.GetPermissionGroup(_userGroupId).DisplayText : "");
                    String _approvazione = _rettificaFuoriStandardService.ApprovaRettifica(FuoriStandard, CodiceCliente, CodicePuf, CodiceContratto, RevoRequest.CurrentUser.UserId, Note, ErrDataInizio, ErrDataFine, ErrTempoLavorazione, (String.IsNullOrEmpty(ErrSospensione) ? "0" : ErrSospensione), ErrFlagStandard, CodiceCausa, CodiceSottocausa, FlagErrore, NonIndennizzabile, NoteApprovatore, (pemString == "FS - Process Owner" ? true : false));
                    return Content(_approvazione);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante l'approvazione...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante l'approvazione..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult RifiutaRettifica(String FuoriStandard, String Note, String NoteApprovatore)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.approvazione.update"))
            {
                try
                {
                    String _rifiuta = _rettificaFuoriStandardService.RifiutaRettifica(FuoriStandard, Note, NoteApprovatore, RevoRequest.CurrentUser.UserId);
                    return Content(_rifiuta);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult FiltraDatiDropdown(String view, bool firstTime)
        {
            try
            {
                if (firstTime == true)
                {
                    if (view == "RettificheFS")
                    {
                        return PartialView("~/views/FuoriStandard/_containerFilters.cshtml", new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.InFuoriStd}));
                    }
                    else if (view == "StoricoFS")
                    {
                        return PartialView("~/views/FuoriStandard/_containerFilters.cshtml", new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.DataFineAttivita,
                                UnifiedSearchOptions.IndicatoreStd, UnifiedSearchOptions.FuoriStandardStorico, UnifiedSearchOptions.Tipologia}));
                    }
                    else if (view == "ApprovazioneFS")
                    {
                        return PartialView("~/views/FuoriStandard/_containerFilters.cshtml", new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.DataFineAttivita,
                                UnifiedSearchOptions.IndicatoreStd, UnifiedSearchOptions.InFuoriStd, UnifiedSearchOptions.Tipologia}));
                    }
                    else if (view == "RecuperoFS")
                    {
                        return PartialView("~/views/FuoriStandard/_containerFilters.cshtml", new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.DataFineAttivita,
                                UnifiedSearchOptions.IndicatoreStd, UnifiedSearchOptions.InFuoriStd, UnifiedSearchOptions.Tipologia, UnifiedSearchOptions.FlagCodCliente, UnifiedSearchOptions.FlagIndennizzabile}));
                    }
                    else
                    {
                        return PartialView("~/views/FuoriStandard/_containerFilters.cshtml", new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.DataFineAttivita,
                                UnifiedSearchOptions.IndicatoreStd, UnifiedSearchOptions.SoloFuoriStd, UnifiedSearchOptions.Tipologia}));
                    }
                }
                else return new EmptyResult();
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DownloadFile(String Filename, String Extension)
        {
            try
            {
                string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPath"].ToString() + Filename + Extension;
                byte[] fileBytes = System.IO.File.ReadAllBytes(ServerPath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Filename + Extension,
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return new FileContentResult(fileBytes, "application/unknown");
            }

            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult DownloadFileStorico(String Filename, String Extension)
        {
            try
            {
                string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPath"].ToString() + Filename + Extension;
                byte[] fileBytes = System.IO.File.ReadAllBytes(ServerPath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Filename + Extension,
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return new FileContentResult(fileBytes, "application/unknown");
            }

            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult DownloadFileSospensione(String Filename, String Extension)
        {
            try
            {
                string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPathSospensioni"].ToString() + Filename + Extension;
                byte[] fileBytes = System.IO.File.ReadAllBytes(ServerPath);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Filename + Extension,
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return new FileContentResult(fileBytes, "application/unknown");
            }

            catch (Exception ex)
            {
                return Content("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AggiungiAllegato(HttpPostedFileBase file, String IdFS)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    if (file != null && file.ContentLength <= 5000000)
                    {
                        string Extension = System.IO.Path.GetExtension(file.FileName);
                        string NomeFile = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPath"].ToString();

                        String result = _fuoriStandardService.AggiungiFile(IdFS, file.InputStream, NomeFile, Extension, ServerPath, RevoRequest.CurrentUser.UserId);

                        return PartialMessage(HtmlSnippets.Alert.Success("Operazione eseguita con successo"));
                    }
                    else
                        return PartialMessage(HtmlSnippets.Alert.Warning("File non valido o troppo grande(max 5MB)"));
                }
                catch (Exception ex)
                {
                    return PartialMessage(HtmlSnippets.Alert.Warning(ex.Message));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AggiungiAllegatoSospensione(HttpPostedFileBase file, long IdSospensione, String IdFs)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    if (file != null && file.ContentLength <= 5000000)
                    {
                        string Extension = System.IO.Path.GetExtension(file.FileName);
                        string NomeFile = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPathSospensioni"].ToString();

                        String result = _rettificaSospensioneService.AggiungiFileSosp(IdSospensione, IdFs, file.InputStream, NomeFile, Extension, ServerPath, RevoRequest.CurrentUser.UserId);

                        return PartialMessage(HtmlSnippets.Alert.Success("Operazione eseguita con successo"));
                    }
                    else
                    {
                        return PartialMessage(HtmlSnippets.Alert.Warning("File non valido o troppo grande(max 5MB)"));
                    }
                }
                catch (Exception ex)
                {
                    return PartialMessage(HtmlSnippets.Alert.Warning(ex.Message));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult ElencoAllegatiFS(String IdFS)
        {
            var _elencoAllegati = _fuoriStandardService.GetElencoAllegati(IdFS);
            return PartialView("~/Views/FuoriStandard/_elencoFileAllegati.cshtml", new AllegatiFSModel() { IdFS = IdFS, SolaLettura = false });
        }

        [Authorize]
        [HttpPost]
        public ActionResult ElencoAllegatiSospensione(long IdSospensione, String IdFs, long RowId)
        {
            var _elencoAllegati = _rettificaSospensioneService.GetElencoAllegatiSosp((IdSospensione != 0 ? IdSospensione : RowId));
            return PartialView("~/Views/FuoriStandard/_elencoAllegatiSospensione.cshtml", new AllegatiSospensione() { allegatiSospensione = _elencoAllegati, IdSospensione = IdSospensione, IdFs = IdFs, RowId = RowId, SolaLettura = false });
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminaAllegato(String Filename, String TipoFile, String IdFS)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPath"].ToString();
                    _fuoriStandardService.DeleteAllegato(Filename, ServerPath, TipoFile);

                    var _elencoAllegati = _fuoriStandardService.GetElencoAllegati(IdFS);
                    return PartialView("~/Views/FuoriStandard/_elencoFileAllegati.cshtml", new AllegatiFSModel() { IdFS = IdFS, SolaLettura = false });
                }
                catch (Exception ex)
                {
                    return PartialMessage(HtmlSnippets.Alert.Error(ex.Message));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminaAllegatoSospensione(String Filename, String TipoFile, long IdSospensione, long RowId)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["DocumentsPathSospensioni"].ToString();
                    _rettificaSospensioneService.DeleteAllegatoSosp(Filename, ServerPath, TipoFile);

                    var _elencoAllegati = _rettificaSospensioneService.GetElencoAllegatiSosp(IdSospensione);
                    return PartialView("~/Views/FuoriStandard/_elencoAllegatiSospensione.cshtml", new AllegatiSospensione() { allegatiSospensione = _elencoAllegati, IdSospensione = IdSospensione, RowId = RowId, SolaLettura = false });
                }
                catch (Exception ex)
                {
                    return PartialMessage(HtmlSnippets.Alert.Error(ex.Message));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        public String SendMailToApprover(String IdFs)
        {
            MailMessage message = new MailMessage();
            FuoriStandard fuoriStandard = _fuoriStandardService.GetFuoriStandardByID(IdFs);
            if (fuoriStandard == null)
            {
                fuoriStandard = _fuoriStandardService.GetFuoriStandardByIdwh(IdFs);
            }
            RettificaFuoriStandard rettifica = _rettificaFuoriStandardService.GetRettificaApertaByID(fuoriStandard.IDFS.ToString());
            if (fuoriStandard != null)
            {
                String managerEmail = _rettificaFuoriStandardService.GetManagerInfo(fuoriStandard.CodiceGruppo);
                managerEmail = managerEmail.Trim();
                var _arrayDestinatari = managerEmail.Split(';');

                if (message.Body.Length == 0)
                {
                    message = new MailMessage("ApplicationServer@capholding.gruppocap.it",
                    _arrayDestinatari[0].ToString(), "Gestione Fuori Standard - Fuori Standard in attesa di approvazione ",
                    "Notifica: ");
                }

                foreach (var item in _arrayDestinatari)
                {
                    if (item.ToString() != message.To.ToString())
                    {
                        try
                        {
                            message.To.Add(item);
                        }
                        catch (Exception ex)
                        {
                            var x = ex.Message;
                            return ex.Message;
                        }
                    }
                }
                String autore = rettifica.Autore != null ? rettifica.Autore : "unnamed";
                message.Body = message.Body + Environment.NewLine;
                message.Body = message.Body + "Il Fuori Standard con Codice Rintracciabilita' num. " + fuoriStandard.CodiceRintracciabilita + " (" + fuoriStandard.CodiceGruppo + ")" + " di " + autore + " del " + fuoriStandard.DataInserimento + " e' in attesa di una vostra approvazione.";
                message.Body = message.Body + Environment.NewLine;
                String nome = _fuoriStandardService.GetCliFuoriStandard(fuoriStandard.CodCliente) != null ? _fuoriStandardService.GetCliFuoriStandard(fuoriStandard.CodCliente).RagioneSociale : fuoriStandard.CodCliente;
                message.Body = message.Body + "Cliente: " + nome + ".";
                string server = "smtp.gruppocap.it";
                IMailSender mailSender = new SmtpMailSender(server);
                try
                {
                    mailSender.Send(message, false);
                    return String.Empty;
                }
                catch (Exception ex)
                {
                    var x = ex.Message;
                    return ex.Message;
                }
            }
            else return String.Empty;
        }

        [Authorize]
        [HttpPost]
        public ActionResult CercaFsRettifiche(String tipologia, String indicatore, String codRintracciabilita, String codCliente, DateTime? dataInizio, DateTime? dataFine, String inStandard, String stato)
        {
            List<String> tipologie = new List<String>();
            ISubCollection<FuoriStandard> _fs = null;
            if (string.IsNullOrEmpty(tipologia) || tipologia == "-1" || tipologia == "undefined")
                tipologie = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
            else tipologie.Add(tipologia);
            if (dataInizio == null)
                dataInizio = DateTime.MinValue;
            if (dataFine == null)
                dataFine = DateTime.MaxValue;
            String _userGroupId = String.Empty;
            Object _o = RevoContext.PermissionManager.GetFirstPermissionGroupIdForUser(RevoRequest.CurrentUser.UserId);
            if (_o != null)
                _userGroupId = _o.ToString();
            string pemString = (RevoContext.PermissionManager.GetPermissionGroup(_userGroupId) != null ? RevoContext.PermissionManager.GetPermissionGroup(_userGroupId).DisplayText : "");

            try
            {
                if (stato == "rettificheTutte")
                    _fs = _rettificaFuoriStandardService.CercaRettificheTutteByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard, (pemString == "FS - Process Owner" ? true : false));
                else if (stato == "daApprovare")
                    _fs = _fuoriStandardService.CercaFuoriStandardDaApprovareByFilters(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard, stato, (pemString == "FS - Process Owner" ? true : false));
                else if (stato == "daApprovareSolaLettura")
                    _fs = _fuoriStandardService.CercaFuoriStandardDaApprovareByFilters(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard, stato, false);
                else
                    _fs = _fuoriStandardService.CercaFuoriStandardByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard, stato, (pemString == "FS - Process Owner" ? true : false));

                if (!RevoRequest.CurrentUser.HasPermissionFor("gfs.fuoriStandard.annullaPrestazione") && stato == "daApprovare")
                {
                    List<FuoriStandard> _daAnnullare = new List<FuoriStandard>();
                    foreach (var item in _fs.Items)
                    {
                        RettificaFuoriStandard rettificato = _rettificaFuoriStandardService.GetRettificaApertaByID(item.IDFS.ToString());
                        if (rettificato != null && rettificato.Causale == "ANN" && rettificato.FlagStato != 1)
                            _daAnnullare.Add(item);
                    }
                    foreach (var itm in _daAnnullare)
                        _fs.Items.Remove(itm);
                }
                else if (RevoRequest.CurrentUser.HasPermissionFor("gfs.fuoriStandard.annullaPrestazione") && stato == "daApprovare" && pemString != "FS - Manager" && pemString != "FS - Process Owner")
                {
                    List<FuoriStandard> _daAnnullare = new List<FuoriStandard>();
                    foreach (var item in _fs.Items)
                    {
                        RettificaFuoriStandard rettificato = _rettificaFuoriStandardService.GetRettificaApertaByID(item.IDFS.ToString());
                        if (rettificato != null && (rettificato.Causale != "ANN" || (rettificato.Causale == "ANN" && rettificato.FlagStato != 2)))
                            _daAnnullare.Add(item);
                    }
                    foreach (var itm in _daAnnullare)
                        _fs.Items.Remove(itm);
                }
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardAperti.cshtml", _fs);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CercaFsStorico(String tipologia, String indicatore, String codRintracciabilita, String codCliente, DateTime? dataInizio, DateTime? dataFine, String inStandard)
        {
            List<String> tipologie = new List<String>();
            if (tipologia == "-1")
                tipologie = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
            else tipologie.Add(tipologia);
            if (dataInizio == null)
                dataInizio = DateTime.MinValue;
            if (dataFine == null)
                dataFine = DateTime.MaxValue;
            try
            {
                ISubCollection<FuoriStandard> _storico = _fuoriStandardService.CercaFuoriStandardStoricoByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardStorico.cshtml", _storico);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CercaFSNonIndennizzabili(String tipologia, String indicatore, String codRintracciabilita, String codCliente, DateTime? dataInizio, DateTime? dataFine, String inStandard, bool checkCliente, bool checkIndennizzabile)
        {
            List<String> tipologie = new List<String>();
            if (tipologia == "-1")
                tipologie = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
            else tipologie.Add(tipologia);
            if (dataInizio == null)
                dataInizio = DateTime.MinValue;
            if (dataFine == null)
                dataFine = DateTime.MaxValue;
            try
            {
                ISubCollection<FuoriStandard> _nonIndennizzabili = _fuoriStandardService.CercaFuoriStandardNonIndennizzabili(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59), inStandard, checkCliente, checkIndennizzabile);
                return PartialView("~/Views/FuoriStandard/_elencoFuoriStandardNonIndennizzabili.cshtml", _nonIndennizzabili);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare.." + ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AssociaNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.nonIndennizzabili.update"))
            {
                try
                {
                    String _associazione = _fuoriStandardService.AssociaNuovoCliente(IdFS, CodiceCliente, CodiceContratto, CodicePuf);
                    return Content(_associazione);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return Content(string.Format("Errore durante l'associazione del cliente...{0}{1}", Environment.NewLine, ex));
                    else
                        return Content("Errore durante l'associazione del cliente...");
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AnnullaPrestazione(String idFuoriStandard, DateTime dataInizioAttivita, DateTime dataFineAttivita, String quantita, String quantitaSosp, String fuoriStandard,
               String causale, String sottoCausale, String note, String nonIndennizzabile, String codiceCliente, String codicePuf, String codiceContratto)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    String _annulla = _rettificaFuoriStandardService.AnnullaPrestazione(idFuoriStandard, dataInizioAttivita, dataFineAttivita, quantita, quantitaSosp, fuoriStandard, causale, sottoCausale,
                                        RevoRequest.CurrentUser.UserId, nonIndennizzabile, codiceCliente, codicePuf, codiceContratto, note);
                    if (_annulla == String.Empty)
                    {
                        String esitoEmail = SendMailToApprover(idFuoriStandard);
                    }
                    return Content(_annulla);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        public ActionResult DropDownFilters(String view)
        {
            UnifiedSearchModel _searchModel = new UnifiedSearchModel(view, new List<UnifiedSearchOptions>() {
                                UnifiedSearchOptions.CaseId, UnifiedSearchOptions.CodCliente, UnifiedSearchOptions.DataFineAttivita,
                                UnifiedSearchOptions.IndicatoreStd, UnifiedSearchOptions.InFuoriStd, UnifiedSearchOptions.Tipologia});
            return PartialView("~/Views/FuoriStandard/_dropDownFilters.cshtml", _searchModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ExportReportPrestazioni()
        {
            try
            {
                ISubCollection<ReportFuoriStandard> _prestazioni = null;
                var totaleGruppi = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
                _prestazioni = _fuoriStandardService.ExportReportPrestazioni(totaleGruppi);
                Dictionary<String, String> settings = new Dictionary<String, String>()
                {
                        { "Anno", "Anno" },
                        { "Numero", "Numero" },
                        { "Data", "Censito il" },
                        { "Importo", "Importo" },
                        { "Cliente", "Codice Cliente" },
                        { "Puf", "Codice Puf" },
                        { "Contratto", "Codice Contratto" },
                        { "Rintracciabilita", " Codice Rintracciabilita" },
                        { "Prestazione", "Codice Prestazione" },
                        { "DescPrestazione", "Descrizione Prestazione" },
                        { "Esito", "Esito" },
                        { "DettaglioEsito", "Dettaglio Esito" },
                        { "NoteEsito", "Note Esito" },
                        { "TipologiaCase", "Tipologia Case" },
                        { "DescStandard", "Descrizione Standard" },
                        { "TipoStandard", "Tipo Standard" },
                        { "CodiceCausa", "Codice causa" },
                        { "DescSottoCausa", "Descrizione Sottocausa" },
                        { "Rettificato", "Rettificato" },
                        { "InStandardSeRettificato", "In Standard se Rettificato" },
                        { "DataInizio", "Data Inizio" },
                        { "DataFine", "Data Fine" },
                        { "ValStandard", "Valore Standard" },
                        { "TempoLavorazione", "Tempo Lavorazione" },
                        { "RetDataInizio", "Rett. Data Inizio" },
                        { "RetDataFine", "Rett. Data Fine" },
                        { "RetTempoLavorazione", "Rett. Tempo Lav." },
                        { "ImportoMoltX", "Molt.X Importo" },
                        { "StadioIndennizzo", "Stadio Indennizzo" },
                        { "DichNonIndennizzabile", "Dich. Non Indennizzabile" },
                        { "Note", "Note" },
                        { "StatoFuoriStandard", "Stato Fuori Standard" },
                        { "DataPagamento", "Data Pagamento" },
                        { "DataEmissioneBolletta", "Data Emissione Bolletta" },
                        { "Bolletta", "Bolletta" },
                        { "UtenteInserimento", "Utente Inserimento" },
                        { "DataInserimento", "Data Inserimento" },
                        { "UtenteValidazione", "Utente Validazione" },
                        { "DataValidazione", "Data Validazione" },
                        { "UtenteRettifica", "Utente Rettifica" },
                        { "DataRettifica", "Data Rettifica" },
                        { "UtenteBenestarePagamento", "Utente Benestare Pagamento" },
                        { "DataBenestarePagamento", "Data Benestare Pagamento" },
                        { "UtenteLiquidazione", "Utente Liquidazione" },
                        { "UtenteAnnullamento", "Utente Annullamento" },
                        { "DataAnnullamento", "Data Annullamento" },
                        { "CodGruppo", "Codice Gruppo" }
                    };
                System.Data.DataTable _t = _prestazioni.Items.ToDataTable<ReportFuoriStandard>(settings);
                _t.TableName = "ReportPrestazioni";
                return new ExcelResult(_t);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Errore durante la generazione del file.." + ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult ExportPrestazioni(String view, String tipologia, String indicatore, String codRintracciabilita, String codCliente, DateTime? dataInizio, DateTime? dataFine, String inStandard, String stato, bool checkCliente, bool checkIndennizzabile)
        {
            List<String> tipologie = new List<String>();
            if (string.IsNullOrEmpty(tipologia) || tipologia == "-1" || tipologia == "undefined")
                tipologie = _fuoriStandardService.GetTipologieByGrouping(RevoRequest.CurrentUser.GroupingCodes.ToList<String>());
            else tipologie.Add(tipologia);
            if (dataInizio == null)
                dataInizio = DateTime.MinValue;
            else dataInizio = new DateTime(dataInizio.Value.Year, dataInizio.Value.Month, dataInizio.Value.Day);
            if (dataFine == null)
                dataFine = DateTime.MaxValue;
            else dataFine = new DateTime(dataFine.Value.Year, dataFine.Value.Month, dataFine.Value.Day, 23, 59, 59);
            if (indicatore == "undefined")
                indicatore = null;
            if (codCliente == "undefined")
                codCliente = null;
            if (codRintracciabilita == "undefined")
                codRintracciabilita = null;
            String _userGroupId = String.Empty;
            Object _o = RevoContext.PermissionManager.GetFirstPermissionGroupIdForUser(RevoRequest.CurrentUser.UserId);
            if (_o != null)
                _userGroupId = _o.ToString();
            string pemString = (RevoContext.PermissionManager.GetPermissionGroup(_userGroupId) != null ? RevoContext.PermissionManager.GetPermissionGroup(_userGroupId).DisplayText : "");

            try
            {
                ISubCollection<FuoriStandard> _fuoriStandard = null;
                if (view == "RecuperoFS")
                    _fuoriStandard = _fuoriStandardService.CercaFuoriStandardNonIndennizzabili(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, (DateTime)dataFine, inStandard, checkCliente, checkIndennizzabile);
                else if (view == "StoricoFS")
                    _fuoriStandard = _fuoriStandardService.CercaFuoriStandardStoricoByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, (DateTime)dataFine, inStandard);
                else if (view == "ApprovazioneTutti")
                    _fuoriStandard = _rettificaFuoriStandardService.CercaRettificheTutteByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, (DateTime)dataFine, inStandard, (pemString == "FS - Process Owner" ? true : false));
                else if (view == "ApprovazioneDaValidare")
                    _fuoriStandard = _rettificaFuoriStandardService.CercaFuoriStandardDaApprovareByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, (DateTime)dataFine, inStandard, (pemString == "FS - Process Owner" ? true : false));
                else if (view == "DaPagare")
                    _fuoriStandard = _fuoriStandardService.GetIndennizziPagabili(RevoRequest.CurrentUser.UserId);
                else _fuoriStandard = _fuoriStandardService.CercaFuoriStandardByFilter(tipologie, indicatore, codRintracciabilita, codCliente, (DateTime)dataInizio, (DateTime)dataFine, inStandard, stato, (pemString == "FS - Process Owner" ? true : false));

                foreach (var item in _fuoriStandard.Items)
                {
                    if (_fuoriStandardService.GetTipologiaStandard(item.IdStandard) != null)
                        item.NumeroPrestazione = _fuoriStandardService.GetTipologiaStandard(item.IdStandard).DescStandard;
                }

                if (!RevoRequest.CurrentUser.HasPermissionFor("gfs.fuoriStandard.annullaPrestazione") && stato == "daApprovare")
                {
                    List<FuoriStandard> _daAnnullare = new List<FuoriStandard>();
                    foreach (var item in _fuoriStandard.Items)
                    {
                        RettificaFuoriStandard rettificato = _rettificaFuoriStandardService.GetRettificaApertaByID(item.IDFS.ToString());
                        if (rettificato != null && rettificato.Causale == "ANN" && rettificato.FlagStato != 1)
                            _daAnnullare.Add(item);
                    }
                    foreach (var itm in _daAnnullare)
                        _fuoriStandard.Items.Remove(itm);
                }
                else if (RevoRequest.CurrentUser.HasPermissionFor("gfs.fuoriStandard.annullaPrestazione") && stato == "daApprovare" && pemString != "FS - Manager" && pemString != "FS - Process Owner")
                {
                    List<FuoriStandard> _daAnnullare = new List<FuoriStandard>();
                    foreach (var item in _fuoriStandard.Items)
                    {
                        RettificaFuoriStandard rettificato = _rettificaFuoriStandardService.GetRettificaApertaByID(item.IDFS.ToString());
                        if (rettificato != null && (rettificato.Causale != "ANN" || (rettificato.Causale == "ANN" && rettificato.FlagStato != 2)))
                            _daAnnullare.Add(item);
                    }
                    foreach (var itm in _daAnnullare)
                        _fuoriStandard.Items.Remove(itm);
                }

                Dictionary<String, String> settings = new Dictionary<String, String>()
                {
                        //{ "IDFS", "ID Fuori Standard" },
                        { "IDFS", "Numero" },
                        { "CensitoIl", "Censito il" },
                        { "Importo", "Importo" },
                        { "CodCliente", "Codice cliente" },
                        { "CodPuf", "Codice PUF" },
                        { "CodContratto", "Codice contratto" },
                        { "CodiceRintracciabilita", "Codice Rintracciabilita" },
                        { "CodicePrestazione", "Codice Prestazione" },
                        { "NumeroPrestazione", "Descrizione Standard" },
                        { "DescrizionePrestazione", "Descrizione Prestazione" },
                        { "Esito", "Esito" },
                        { "DettaglioEsito", "Dettaglio Esito" },
                        { "NoteEsito", "Note Esito" },
                        { "TipoStandard", "Tipo Standard" },
                        { "CodiceCausa", "Codice causa" },
                        { "CodiceSottocausa", "Codice sottocausa" },
                        { "DataInizio", "Data inizio" },
                        { "DataFine", "Data fine" },
                        { "ValoreStandard", "Valore Standard" },
                        { "EvasoIn", "Tempo Lavorazione" },
                        { "ErrDataInizio", "Rettifica Data Inizio" },
                        { "ErrDataFine", "Rettifica Data Fine" },
                        { "ErrTempoLavorazione", "Rettifica Tempo Lavorazione" },
                        { "StadioIndennizzo", "Stadio Indennizzo" },
                        { "NonIndennizzabile", "Flag non indennizzabile" },
                        { "Note", "Note" },
                        { "Stato", "Stato" },
                        { "DataRimborso", "Data rimborso" },
                        { "EmissioneBolletta", "Emissione bolletta" },
                        { "CodBolletta", "Cod bolletta" },
                        { "UtenteInserimento", "Utente inserimento" },
                        { "DataInserimento", "Data inserimento" },
                        { "GestitoDa", "Gestito da" },
                        { "GestitoIl", "Gestito il" },
                        { "UtenteErrore", "Utente Rettifica" },
                        { "ValidazioneErrore", "Data Approvazione Rettifica" },
                        { "UtenteAnnullamento", "Utente annullamento" },
                        { "DataAnnullamento", "Data annullamento" },
                        { "CodiceGruppo", "Codice gruppo" },
                        //{ "Provenienza", "Provenienza" },
                        //{ "CaseID", "ID Case" },                                                
                        //{ "CodStandard", "Codice Standard" },
                        //{ "IdStandard", "ID Standard" },
                        { "DataMigrazione", "Data migrazione" },
                        { "UtenteMigrazione", "Utente migrazione" },
                        { "DataChiusura", "Data chiusura" },
                        //{ "IndennizzoDWH", "Indennizzo DWH" },
                        //{ "NumeroPrestazione", "Numero Prestazione" },                        
                        //{ "Tipo", "Tipo fs" },
                        //{ "ErrFlagStandard", "Rettifica Flag Standard" },
                        //{ "FlagErrore", "Flag Rettifica" },
                        //{ "FlagOrigine", "Flag Origine" }
                    };
                System.Data.DataTable _t = _fuoriStandard.Items.ToDataTable<FuoriStandard>(settings);
                _t.TableName = "EstrazionePrestazioni_" + view;
                //byte[] bytes = Encoding.UTF8.GetBytes(new ExcelResult(_t).ToString());
                return new ExcelResult(_t);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Info("Errore durante la generazione del file.." + ex.Message));
            }
            //return File(bytes, "text/csv", "CsvDoc_n" + ".csv");
        }

        [HttpPost]
        public ActionResult ValidaSospensioni(CalcoloDurataSospensioniRequest jsonSospensioni, String idFs, String numeroPrestazione, DateTime dataInizio, DateTime dataFine, bool salva)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    //ISubCollection<RettificaSospensione> _sospensioni;
                    if (salva)
                    {
                        _rettificaSospensioneService.ConfermaSospensioniInLavorazione(Convert.ToInt32(idFs));
                        //_sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFs, false);
                    }
                    else
                    {
                        CalcoloDurataSospensioniResponse operationResult = CallSafoCalcoloSospensioni(jsonSospensioni, idFs, numeroPrestazione, dataInizio, dataFine);
                        if (operationResult.OutputData.result == true)
                        {
                            ISubCollection<RettificaSospensione> _sospensioni;
                            //if (salva)
                            //{
                            //    _rettificaSospensioneService.ConfermaSospensioniInLavorazione(Convert.ToInt32(idFs));
                            //    _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFs, false);
                            //}
                            _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFs, true);
                            SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = operationResult.OutputData.durataPrestazione.ToString(), durataTotaleSospensioni = operationResult.OutputData.durataTotaleSospensioni.ToString(), sospensioni = _sospensioni };
                            return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
                        }
                        else
                        {
                            return Json(new { esito = "danger", messaggio = operationResult.OutputData.errorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //_sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFs, true);
                    SospensioniViewModel sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", durataTotaleSospensioni = "", sospensioni = null };
                    return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", sospensioniViewModel);
                }
                catch (Exception ex)
                {
                    return Json(new { esito = "danger", messaggio = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        public ActionResult SospendiModifiche(int idFuoriStandard)
        {
            try
            {
                _rettificaSospensioneService.SospendiModifiche(idFuoriStandard);
                ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFuoriStandard.ToString(), false);
                SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", sospensioni = _sospensioni };
                return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle sospensioni."));
            }
        }

        private static String SafoServiceUrlCalcoloSospensioni
        {
            get
            {
                return ConfigurationManager.AppSettings["Safo.Service.Url.CalcoloSospensioni"] ?? "";
            }
        }

        private CalcoloDurataSospensioniResponse CallSafoCalcoloSospensioni(CalcoloDurataSospensioniRequest sosp, String idFs, String numeroPrestazione, DateTime dataInizio, DateTime dataFine)
        {
            try
            {
                ISubCollection<RettificaSospensione> sospensioniList = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFs, true);
                List<Sospensione> _sospensioni = new List<Sospensione>();
                foreach (var item in sospensioniList.Items)
                    _sospensioni.Add(new Sospensione { codiceSospensione = item.ROW_ID.ToString(), inizioSospensione = item.DATA_INIZIO_SOSPENSIONE, fineSospensione = item.DATA_FINE_SOSPENSIONE });

                sospList _sospList = new sospList { sospRequest = _sospensioni };
                inputData _inputData = new inputData
                {
                    numeroPrestazione = "ESE_SPO_PRE_00504716",
                    //numeroPrestazione = numeroPrestazione,
                    inizioPrestazione = dataInizio,
                    finePrestazione = dataFine,
                    sospList = _sospList,
                };
                CalcoloDurataSospensioniRequest sospensioni = new CalcoloDurataSospensioniRequest { InputData = _inputData };

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, SafoServiceUrlCalcoloSospensioni);

                    var json = JsonConvert.SerializeObject(sospensioni);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = client.SendAsync(request).Result;
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = responseMessage.Content.ReadAsStringAsync().Result;
                        var jObject = JObject.Parse(responseJson);
                        if (jObject.HasValues)
                        {
                            var _result = jObject["outputData"]["result"].CoerceToOrDefault<bool>(false);
                            if (_result == true)
                            {
                                var sospe = (jObject["outputData"]["sospensioni"] != null ? JsonConvert.SerializeObject(jObject["outputData"]["sospensioni"]) : "");
                                //var _sosp = (jObject["outputData"]["sospensioni"].HasValues == true ? JsonConvert.SerializeObject(jObject["outputData"]["sospensioni"]) : "");
                                List<SospensioneOut> _sospensioniList = JsonConvert.DeserializeObject<List<SospensioneOut>>(sospe);
                                var _numeroPrestazioneOut = jObject["outputData"]["numeroPrestazioneOut"].CoerceTo<string>();
                                var _errorMessage = jObject["outputData"]["errorMessage"].CoerceTo<string>();
                                var _errorCode = jObject["outputData"]["errorCode"].CoerceTo<string>();
                                var _durataPrestazione = jObject["outputData"]["durataPrestazione"].CoerceToOrDefault<decimal>(0);
                                var _durataTotaleSospensioni = jObject["outputData"]["durataTotaleSospensioni"].CoerceToOrDefault<decimal>(0);
                                var y = new CalcoloDurataSospensioniResponse()
                                {
                                    OutputData = new OutputData
                                    {
                                        sospensioni = _sospensioniList,
                                        result = _result,
                                        numeroPrestazioneOut = _numeroPrestazioneOut,
                                        errorMessage = _errorMessage,
                                        errorCode = _errorCode,
                                        durataPrestazione = _durataPrestazione,
                                        durataTotaleSospensioni = _durataTotaleSospensioni
                                    }
                                };

                                foreach (var item in sospensioniList.Items)
                                    item.DURATA_SOSPENSIONE = _sospensioniList.Where(s => s.codiceSospensioneOut == item.ROW_ID.ToString()).Select(x => x.durataSospensione).FirstOrDefault();

                                IUpdateOperationResult updateSospensione = _rettificaSospensioneService.AggiornaDurataSospensioni(sospensioniList.Items.Where(x => x.STATO_SOSPENSIONE != "O").ToList());

                                return y;
                                //return new OperationResult(
                                //_result,
                                ////jObject["outputData"]["errorCode"].CoerceTo<string>(),
                                //_durataPrestazione.ToString(),
                                //new String[] { jObject["outputData"]["errorMessage"].CoerceTo<string>() });
                            }
                            else
                            {
                                return new CalcoloDurataSospensioniResponse()
                                {
                                    OutputData = new OutputData
                                    {
                                        sospensioni = null,
                                        result = _result,
                                        numeroPrestazioneOut = "",
                                        errorMessage = jObject["outputData"]["errorMessage"].CoerceTo<string>(),
                                        errorCode = jObject["outputData"]["errorCode"].CoerceTo<string>(),
                                        durataPrestazione = 0,
                                        durataTotaleSospensioni = 0
                                    }
                                };
                                //return new OperationResult(
                                //_result,
                                //jObject["outputData"]["errorCode"].CoerceTo<string>(),
                                //new String[] { jObject["outputData"]["errorMessage"].CoerceTo<string>() });
                            }
                        }
                        else
                        {
                            return new CalcoloDurataSospensioniResponse()
                            {
                                OutputData = new OutputData
                                {
                                    sospensioni = null,
                                    result = false,
                                    numeroPrestazioneOut = "",
                                    errorMessage = "La risposta di SalesForce non contiene valori",
                                    errorCode = "Badresponse",
                                    durataPrestazione = 0,
                                    durataTotaleSospensioni = 0
                                }
                            };
                            //return new OperationResult(
                            //false,
                            //"Badresponse",
                            //new String[] { "La risposta di SalesForce non contiene valori" });
                        }
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseJson = responseMessage.Content.ReadAsStringAsync().Result;
                        var jObject = JToken.Parse(responseJson);
                        if (jObject is JArray)
                        {
                            jObject = jObject.First;
                        }
                        return new CalcoloDurataSospensioniResponse()
                        {
                            OutputData = new OutputData
                            {
                                sospensioni = null,
                                result = false,
                                numeroPrestazioneOut = "",
                                errorMessage = jObject["message"].CoerceTo<string>(),
                                errorCode = jObject["errorCode"].CoerceTo<string>(),
                                durataPrestazione = 0,
                                durataTotaleSospensioni = 0
                            }
                        };
                        //return new OperationResult(
                        //false,
                        //jObject["errorCode"].CoerceTo<string>(),
                        //new String[] { jObject["message"].CoerceTo<string>() });
                    }
                    else
                    {
                        string responseJson = responseMessage.Content.ReadAsStringAsync().Result;

                        //return ResponseKO(false, "Error", responseJson);
                        return new CalcoloDurataSospensioniResponse()
                        {
                            OutputData = new OutputData
                            {
                                sospensioni = null,
                                result = false,
                                numeroPrestazioneOut = "",
                                errorMessage = responseJson,
                                errorCode = "Error",
                                durataPrestazione = 0,
                                durataTotaleSospensioni = 0
                            }
                        };
                        //return new OperationResult(
                        //    false,
                        //    "Error",
                        //    new String[] { responseJson });
                    }
                }
            }
            catch (Exception ex)
            {
                return new CalcoloDurataSospensioniResponse()
                {
                    OutputData = new OutputData
                    {
                        sospensioni = null,
                        result = false,
                        numeroPrestazioneOut = "",
                        errorMessage = ex.InnerException.Message,
                        errorCode = "Exception on Validazione Sospensioni",
                        durataPrestazione = 0,
                        durataTotaleSospensioni = 0
                    }
                };
                //return new OperationResult(
                //    false,
                //    "Exception on Validazione Sospensioni",
                //    new String[] { ex.Message });
            }
        }

        public ActionResult ElencoSospensioni(String idFS)
        {
            try
            {
                ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFS, false);
                SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", sospensioni = _sospensioni };
                return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle sospensioni."));
            }
        }

        public ActionResult CategorieSospensioni(String numeroPrestazione)
        {
            try
            {
                List<CategoriaSospensione> _categorie = _rettificaSospensioneService.GetCategorieSospensione(numeroPrestazione.Replace(" ", ""));
                return PartialView("~/Views/FuoriStandard/_categorieSospensione.cshtml", _categorie);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle categorie della sospensione."));
            }
        }

        public ActionResult TipiSospensione(String codCategoria)
        {
            try
            {
                List<TipoSospensione> _tipi = _rettificaSospensioneService.GetTipiSospensione(codCategoria);
                return PartialView("~/Views/FuoriStandard/_tipiSospensione.cshtml", _tipi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle tipologie della sospensione."));
            }
        }

        [HttpPost]
        public ActionResult AggiornaSospensione(SospensioneModel data)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    FuoriStandard indennizzo = _fuoriStandardService.GetFuoriStandardByID(data.ID_INDENNIZZO.ToString());
                    RettificaSospensione sospensione = new RettificaSospensione();
                    sospensione = EntityMapper<RettificaSospensione>.MapEntity(data);
                    sospensione.NUMERO_PRESTAZIONE = indennizzo.NumeroPrestazione;
                    sospensione.ID_STANDARD = indennizzo.IdStandard;
                    sospensione.UTE_INS = RevoRequest.CurrentUser.UserId;
                    sospensione.DATA_INS = DateTime.Now;
                    sospensione.STATO_SOSPENSIONE = (data.ROW_ID == 0 ? "N" : "R");
                    sospensione.STATUS = 0;
                    if (data.ROW_ID != 0)
                        sospensione.ID_SOSPENSIONE = data.ROW_ID;

                    IInsertOperationResult insertSospensione = _rettificaSospensioneService.AggiornaSospensione(sospensione);
                    if (insertSospensione.GenericMeaning == true)
                    {
                        //ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(data.ID_INDENNIZZO.ToString());
                        ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(data.ID_INDENNIZZO.ToString(), true);
                        //_sospensioni.Items.Add(sospensione);
                        SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", sospensioni = _sospensioni };
                        return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
                    }
                    else
                    {
                        return Content(insertSospensione.Description);
                    }
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle sospensioni."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        public ActionResult RipristinaSospensioniCancellate(String idFuoriStandard)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    _rettificaSospensioneService.RipristinaSospensioniCancellate(idFuoriStandard);
                    ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idFuoriStandard, false);
                    SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", sospensioni = _sospensioni };
                    return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante il ripristino delle sospensioni."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        public ActionResult AnnullaSospensione(int idIndennizzo, long idSospensione, long rowID)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.validazione.update"))
            {
                try
                {
                    _rettificaSospensioneService.AnnullaSospensione(idIndennizzo, idSospensione, rowID);
                    ISubCollection<RettificaSospensione> _sospensioni = _rettificaSospensioneService.GetSospensioniByIDFuoriStandard(idIndennizzo.ToString(), true);
                    SospensioniViewModel _sospensioniViewModel = new SospensioniViewModel { durataPrestazione = "", sospensioni = _sospensioni };
                    return PartialView("~/Views/FuoriStandard/_sospensioniList.cshtml", _sospensioniViewModel);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle sospensioni."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        public ActionResult GetSospensione(int idIndennizzo, long rowID)
        {
            try
            {
                var data = _rettificaSospensioneService.GetSospensioneModifica(rowID.ToString());
                SospensioneModel _sospensioneModel = new SospensioneModel
                {
                    CATEGORIA_SOSPENSIONE = data.CATEGORIA_SOSPENSIONE,
                    DATA_COMUNICAZIONE = data.DATA_COMUNICAZIONE,
                    DATA_FINE_SOSPENSIONE = data.DATA_FINE_SOSPENSIONE,
                    DATA_INIZIO_SOSPENSIONE = data.DATA_INIZIO_SOSPENSIONE,
                    NOTE = data.NOTE,
                    ROW_ID = data.ROW_ID,
                    STATO_SOSPENSIONE = data.STATO_SOSPENSIONE,
                    TIPO_SOSPENSIONE = data.TIPO_SOSPENSIONE,
                    ID_INDENNIZZO = data.ID_INDENNIZZO,
                    ID_SOSPENSIONE = data.ID_SOSPENSIONE
                };
                return PartialView("~/Views/FuoriStandard/_sospensioniModal.cshtml", _sospensioneModel);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca delle sospensioni."));
            }
        }

        [HttpPost]
        public ActionResult GetNuovaSospensione(int idIndennizzo)
        {
            try
            {
                FuoriStandard indennizzo = _fuoriStandardService.GetFuoriStandardByID(idIndennizzo.ToString());
                SospensioneModel _sospensioneModel = new SospensioneModel
                {
                    DATA_INIZIO_SOSPENSIONE = indennizzo.DataInizio.Value,
                    DATA_FINE_SOSPENSIONE = indennizzo.DataInizio.Value.AddDays(1),
                    //DATA_FINE_SOSPENSIONE = indennizzo.DataFine.Value,
                    ID_INDENNIZZO = indennizzo.IDFS,
                    NUMERO_PRESTAZIONE = indennizzo.NumeroPrestazione,
                    ID_STANDARD = indennizzo.IdStandard
                };
                return PartialView("~/Views/FuoriStandard/_sospensioniModal.cshtml", _sospensioneModel);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca della prestazione."));
            }
        }

        [Authorize]
        public ActionResult GetAnniBloccati()
        {
            try
            {
                ISubCollection<AnnoBloccato> _anniBloccati = _anniBloccatiService.GetAnniBloccati();
                return PartialView("~/Views/FuoriStandard/_elencoGestioneRettifiche.cshtml", _anniBloccati);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricarca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult ModificaDataBlocco()
        {
            try
            {
                AnnoBloccato _annoBloccato = _anniBloccatiService.GetAnnoBloccato();
                if (_annoBloccato != null)
                    return PartialView("~/Views/FuoriStandard/_modificaDataBlocco.cshtml", new AnnoBloccatoModel { annoBloccato = _annoBloccato, edit = true });
                else return Content(String.Empty);
                //else return PartialMessage(HtmlSnippets.Alert.Info("Non ci sono anni di competenza per cui è ancora possibile la modifica della data di blocco..."));
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricarca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult NuovaDataBlocco()
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.gestisciRettifiche.update"))
            {
                try
                {
                    AnnoBloccato _annoBloccato = _anniBloccatiService.GetLastAnnoBloccato();
                    return PartialView("~/Views/FuoriStandard/_modificaDataBlocco.cshtml", new AnnoBloccatoModel { annoBloccato = _annoBloccato, edit = false });
                    //else return PartialMessage(HtmlSnippets.Alert.Info("Non ci sono anni di competenza per cui è ancora possibile la modifica della data di blocco..."));
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricarca...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [Authorize]
        public ActionResult AggiornaDataBlocco(bool nuovoBlocco, string annoCompetenza, DateTime dataBlocco)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gfs.fuoriStandard.gestisciRettifiche.update"))
            {
                try
                {
                    if (nuovoBlocco)
                        _anniBloccatiService.InsertDataBlocco(annoCompetenza, dataBlocco, RevoRequest.CurrentUsername);
                    else
                        _anniBloccatiService.UpdateDataBlocco(annoCompetenza, dataBlocco, RevoRequest.CurrentUsername);

                    ISubCollection<AnnoBloccato> _anniBloccati = _anniBloccatiService.GetAnniBloccati();
                    return PartialView("~/Views/FuoriStandard/_elencoGestioneRettifiche.cshtml", _anniBloccati);
                }
                catch (Exception ex)
                {
                    if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricarca...{0}{1}", Environment.NewLine, ex)));
                    else
                        return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
                }
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

    }
}