using GestioneRimborsi.Core;
using GruppoCap.Core;
using GruppoCap.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using GruppoCap.Core.Data;
using GruppoCap.Authentication.Core;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Mail;
using GruppoCap.Mail;
using System.Text;

namespace GestioneRimborsi.Web.Controllers
{
    public class RimborsiController : RevoController
    {
        private IClienteService _clienteService = null;
        private IRimborsoService _rimborsoService = null;

        public RimborsiController(IClienteService clienteService, IRimborsoService rimborsoService)
        {
            _clienteService = clienteService;
            _rimborsoService = rimborsoService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult DettaglioRimborso(String AnnoDocumento, String NumeroDocumento, String CodCliente, String Utente)
        {
            Utente = CheckUtente(Utente);
            ViewData.Add("Utente", Utente);
            var _rimborso = _rimborsoService.GetRimborso(CodCliente, Utente, Convert.ToInt16(AnnoDocumento), NumeroDocumento);
            return View("DettaglioRimborso", _rimborso);
        }

        public FileResult Guide()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath("~/Content/PDF/Guida.pdf"));
            string fileName = "Guida.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }

        private ReportViewer reportViewer;

        [Authorize]
        public ActionResult ConfermaRimborsi(String Utente)
        {
            Utente = CheckUtente(Utente);
            return View("ConfermaRimborsi", model: Utente);
        }

        [Authorize]
        public ActionResult ConfermareRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente, String NumeroProtocollo, String UtenteProtocollo, String DataProtocollo)
        {
            Utente = CheckUtente(Utente);
            String _contabilizza = _rimborsoService.ConfermaRimborsi(ClienteAnnoNumeroDocumento, Utente, NumeroProtocollo, UtenteProtocollo, DataProtocollo);

            //if (_contabilizza == "")
            //{
            //    List<String> bonusIdrici = new List<string>();
            //    foreach (var items in ClienteAnnoNumeroDocumento)
            //    {
            //        var elem = items.Split(';');
            //        if (elem[3] == "BONU")
            //            bonusIdrici.Add(items);
            //    }                
            //}
            return Content(_contabilizza);
        }

        [Authorize]
        public ActionResult RimborsiConfermati(String ClienteAnnoNumeroDocumento, String Utente, String NumeroProt, String UtenteProt, String DataProt)
        {
            Utente = CheckUtente(Utente);

            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            DataTable datatable;

            List<string> ClienteAnnoNumeroDocumentoList = new List<string>();
            ClienteAnnoNumeroDocumentoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ClienteAnnoNumeroDocumento);
            datatable = _rimborsoService.GetRimborsiTestataByClienteAnnoNumeroDocumento(ClienteAnnoNumeroDocumentoList, Utente).Items.ToDataTable();

            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);

            reportViewer.LocalReport.ReportPath = System.IO.Path.Combine(Server.MapPath("~/Reports"), "GestioneRimborsi.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RimborsiTestata", datatable));


            string mimeType;
            string encoding;
            string extension;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string[] streamIds;

            bool error = false;
            try
            {
                byte[] result = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                //return Convert.ToBase64String(result);
                return new FileContentResult(result, "application/pdf");
            }
            catch (Exception ex)
            {
                error = true;
            }
            finally
            {
                reportViewer.LocalReport.SubreportProcessing -= new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            }
            return new EmptyResult();
            //return null;
        }


        //[Authorize]
        //[HttpPost]
        //public ActionResult RimborsiConfermatiIE(String ClienteAnnoNumeroDocumento)
        //{            
        //        //return Convert.ToBase64String(result);
        //        return new FileContentResult((Byte)(ClienteAnnoNumeroDocumento), "application/pdf");           
        //}

        [Authorize]
        private string CheckUtente(string Utente)
        {
            if (String.IsNullOrEmpty(Utente) || Utente == "undefined" || (Utente != RevoRequest.CurrentUser.UserId && !RevoRequest.CurrentUser.IsSuperUser()))
                Utente = RevoRequest.CurrentUser.UserId;
            return Utente;
        }

        [Authorize]
        private List<String> CheckPermission(string Utente)
        {
            List<String> Permission = new List<String>();

            if (RevoRequest.CurrentUser.IsPrivileged || RevoRequest.CurrentUser.IsSuperUser())
            {
                Permission.Add("Privileged");
                return Permission;
            }

            //if (RevoRequest.CurrentUser.GroupingCodes.Contains("gri.OperatoreRimborso"))
                Permission.Add("gri.OperatoreRimborso");
            if (RevoRequest.CurrentUser.GroupingCodes.Contains("gri.OperatoreBonusIdrico"))
                Permission.Add("gri.OperatoreBonusIdrico");
            if (RevoRequest.CurrentUser.GroupingCodes.Contains("gri.OperatoreIndennizzi"))
                Permission.Add("gri.OperatoreIndennizzi");           

            //if (RevoRequest.CurrentUser.HasPermissionFor("GestioneRimborsi.GestionePermessi.OperatoreRimborso"))
            //    Permission.Add("OperatoreRimborso");
            //if (RevoRequest.CurrentUser.HasPermissionFor("GestioneRimborsi.GestionePermessi.OperatoreBonusIdrico"))
            //    Permission.Add("OperatoreBonusIdrico");
            //if (RevoRequest.CurrentUser.HasPermissionFor("GestioneRimborsi.GestionePermessi.OperatoreIndennizzi"))
            //    Permission.Add("OperatoreIndennizzi");

            return Permission;
        }

        [Authorize]
        public ActionResult CercaRimborsi(String CodCliente, String Utente)
        {
            Utente = CheckUtente(Utente);
            List<String> Permission = CheckPermission(Utente);

            try
            {
                String cliente = _clienteService.ClienteByID(CodCliente);
                ViewData.Add("UserName", cliente);

                ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetRimborsiFiltered(CodCliente, Utente, Permission);


                return PartialView("~/Views/Rimborsi/_elencoRimborsi.cshtml", _rimborsi);

            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la lettura dei rimborsi...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la lettura dei rimborsi..."));
            }
        }

        [Authorize]
        public ActionResult CercaRimborsiConfermati(String codCliente, String Utente)
        {
            Utente = CheckUtente(Utente);

            try
            {
                ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetRimborsiConfermati(codCliente, Utente);
                return PartialView("~/Views/Rimborsi/_elencoRimborsiConfermati.cshtml", _rimborsi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la lettura dei rimborsi...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la lettura dei rimborsi..."));
            }
        }

        [Authorize]
        public ActionResult CercaClienti(String term)
        {
            ISubCollection<Cliente> _clienti = _clienteService.FilterByTerm(term);
            return PartialView("~/Views/Rimborsi/_risultatiRicercaCliente.cshtml", _clienti);
        }

        [Authorize]
        public ActionResult CercaCliente(String term)
        {
            return PartialView("~/Views/Rimborsi/_schedaClienteModal.cshtml", term);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AnnullaRimborsi(String Utente)
        {
            Utente = CheckUtente(Utente);
            return View("AnnullaRimborsi", model: Utente);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CancellaRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente)
        {
            Utente = CheckUtente(Utente);

            bool annullaRimborsi = _rimborsoService.AnnullaRimborso(ClienteAnnoNumeroDocumento, Utente);
            if (!annullaRimborsi)
                return PartialMessage(HtmlSnippets.Alert.Error("Cancellazione del rimborso non riuscita."));
            return View("AnnullaRimborsi", model: Utente);
        }

        [Authorize]
        public ActionResult RistampaRimborsiSelezionati(String ClienteAnnoNumeroDocumento, String Utente)
        {
            Utente = CheckUtente(Utente);

            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            DataTable datatable;

            List<string> ClienteAnnoNumeroDocumentoList = new List<string>();
            ClienteAnnoNumeroDocumentoList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ClienteAnnoNumeroDocumento);
            datatable = _rimborsoService.GetRimborsiTestataByClienteAnnoNumeroDocumento(ClienteAnnoNumeroDocumentoList, Utente).Items.ToDataTable();

            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            reportViewer.LocalReport.ReportPath = System.IO.Path.Combine(Server.MapPath("~/Reports"), "GestioneRimborsi.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("RimborsiTestata", datatable));

            string mimeType;
            string encoding;
            string extension;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string[] streamIds;

            bool error = false;
            try
            {
                byte[] result = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                return new FileContentResult(result, "application/pdf");
            }
            catch (Exception ex)
            {
                error = true;
            }
            finally
            {
                reportViewer.LocalReport.SubreportProcessing -= new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            }
            return new EmptyResult();
        }

        [Authorize]
        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            DataTable datatableDett;
            String AnnoDocumento = e.Parameters["AnnoDocumento"].Values[0];
            String NumeroDocumento = e.Parameters["NumeroDocumento"].Values[0];
            String TipoRimborso = e.Parameters["TipoRimborso"].Values[0];
            String ImportoTotaleRimborso = e.Parameters["ImportoTotaleRimborso"].Values[0];

            String CodiceCliente = _clienteService.GetCodiceCliente(e.Parameters["CodiceCliente"].Values[0]);

            String TipoDocumento = e.Parameters["TipoDocumento"].Values[0];
            DataTable datatableInso = _clienteService.GetStampaInsoluti(CodiceCliente, AnnoDocumento, NumeroDocumento, TipoDocumento).Items.ToDataTable();

            datatableDett = _rimborsoService.GetRimborsoDettaglio(AnnoDocumento, NumeroDocumento).Items.ToDataTable();

            e.DataSources.Add(new ReportDataSource("RimbDett", datatableDett));
            e.DataSources.Add(new ReportDataSource("Insoluti", datatableInso));
        }

        [Authorize]
        public ActionResult RistampaRimborsi(String Utente)
        {   
            Utente = CheckUtente(Utente);
            return View("RistampaRimborsi", model: Utente);
        }

        [Authorize]
        public ActionResult UtenteRimborso(String Utente)
        {
            Utente = CheckUtente(Utente);

            ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetRimborsiConfermabili(Utente, RevoRequest.CurrentUser.IsSuperUser());
            return PartialView("~/Views/Rimborsi/_confermaRimborsi.cshtml", _rimborsi);
        }

        [Authorize]
        public ActionResult UtenteRimborsoAnn(String Utente)
        {
            Utente = CheckUtente(Utente);

            ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetRimborsiAnnullabili(Utente, RevoRequest.CurrentUser.IsSuperUser());
            return PartialView("~/Views/Rimborsi/_annullaRimborsi.cshtml", _rimborsi);
        }

        [Authorize]
        public ActionResult RistampaMassiva(String Utente, DateTime calendar, DateTime calendar2)
        {
            Utente = CheckUtente(Utente);
            ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetRimborsiRistampaMassiva(calendar, new DateTime(calendar2.Year, calendar2.Month, calendar2.Day, 23, 59, 59), Utente);
            return PartialView("~/Views/Rimborsi/_elencoRimborsiConfermati.cshtml", _rimborsi);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SalvaRimborsi(GestioneRimborsi.Core.RimborsoGestito Rimborso)
        {
            try
            {
                var rimborsoNativo = _rimborsoService.GetRimborso(Rimborso.CodiceCliente, Rimborso.UtenteInserimento, Convert.ToInt16(Rimborso.AnnoDocumento), Rimborso.NumeroDocumento);
                //var salvaRimborso = _rimborsoService.SalvaRimborso(
                //    rimborsoNativo,
                //    Rimborso,
                //    _clienteService.InfoRecapito(Rimborso.CodicePuntoFornitura, Rimborso.NumeroDocumento, Rimborso.TipoDocumento, Rimborso.CodiceCliente)
                //);                
                //return new EmptyResult();

                var clienteRecapito = _clienteService.InfoRecapito(Rimborso.CodicePuntoFornitura, Rimborso.NumeroDocumento, Rimborso.TipoDocumento, Rimborso.CodiceCliente);

                if (!string.IsNullOrEmpty(rimborsoNativo.CodicePuntoFornitura))
                    Rimborso.CodicePuntoFornitura = rimborsoNativo.CodicePuntoFornitura;

                if (clienteRecapito != null)
                {
                    if (!(clienteRecapito.RagioneSociale != Rimborso.Beneficiario || rimborsoNativo.Beneficiario != Rimborso.Beneficiario))
                        Rimborso.Beneficiario = null;
                }

                if (Rimborso.TipoRimborso == "BON" && Rimborso.IBAN != null)
                {
                    Rimborso.IndirizzoAssegno = null;
                    Rimborso.LocalitaAssegno = null;
                    Rimborso.CAPAssegno = null;
                    Rimborso.ProvinciaAssegno = null;
                }
                else if (Rimborso.TipoRimborso == "ASS")
                    Rimborso.IBAN = null;

                var salvaRimborso2 = _rimborsoService.InserisciRimborso(rimborsoNativo, Rimborso);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AggiungiFile(HttpPostedFileBase file, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente)
        {
            try
            {
                if (file != null)
                {
                    string Extension = System.IO.Path.GetExtension(file.FileName);
                    string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"].ToString();

                    String result = _rimborsoService.AggiungiFile(file.InputStream, file.FileName, Extension, ServerPath, AnnoDocumento, NumeroDocumento, FileDescription, Utente);

                    return PartialMessage(HtmlSnippets.Alert.Success("Operazione eseguita con successo"));
                }
                else
                {
                    return PartialMessage(HtmlSnippets.Alert.Warning("File non valido"));
                }
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Warning(ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ElencoFileRimborso(String AnnoDocumento, String NumeroDocumento)
        {
            var _elencoDocRimb = _rimborsoService.GetElencoDocumenti(AnnoDocumento, NumeroDocumento);
            return PartialView("~/Views/Rimborsi/_elencoFileRimborsi.cshtml", _elencoDocRimb);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteFile(String Filename, String TipoFile, String AnnoDocumento, String NumeroDocumento)
        {
            try
            {
                string ServerPath = System.Web.HttpContext.Current.Server.MapPath("~") + System.Configuration.ConfigurationManager.AppSettings["DocumentsPath"].ToString();

                _rimborsoService.DeleteFile(Filename, ServerPath, TipoFile);

                var _elencoDocRimb = _rimborsoService.GetElencoDocumenti(AnnoDocumento, NumeroDocumento);
                return PartialView("~/Views/Rimborsi/_elencoFileRimborsi.cshtml", _elencoDocRimb);
            }
            catch (Exception ex)
            {
                return PartialMessage(HtmlSnippets.Alert.Error(ex.Message));
            }
        }

        [Authorize]
        public ActionResult CambiamentoIBAN(String CodiceCliente, String IBAN, DateTime DataInserimento, String UtenteInserimento)
        {
            if (RevoRequest.CurrentUser.HasPermissionOrPrivileged("gri.iban.update"))
            {
                Cliente _cliente = _clienteService.InfoCliente(CodiceCliente);
                string VecchioIban = _clienteService.GetIBAN(CodiceCliente)?.CodiceIBAN;

                bool result = _clienteService.RegistraIBAN(CodiceCliente, IBAN, DateTime.Now, UtenteInserimento);
                if (result == true)
                {
                    String emailCambioIBAN = SendMailChangeIBAN(CodiceCliente, VecchioIban);
                    if (!String.IsNullOrEmpty(emailCambioIBAN))
                    {
                        return Json(new { result, errorMessage = emailCambioIBAN });
                    }
                }
                return Json(new { result });
            }
            else return PartialMessage(HtmlSnippets.Alert.Warning("Accesso negato."));
        }

        [HttpPost]
        [Authorize]
        public String SendMailBonusIdrico(List<String> bonusIdrici)
        {
            MailMessage message = new MailMessage();
            String Utente = CheckUtente("");
            ISubCollection<GestioneRimborso> _rimborsi = _rimborsoService.GetTestataBozzaRimborsi(bonusIdrici, Utente);
            foreach (var documento in _rimborsi.Items)
            {
                String destinatario = System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();
                destinatario = destinatario.Trim();
                var _arrayDestinatari = destinatario.Split(';');

                if (message.Body.Length == 0)
                {
                    message = new MailMessage("ApplicationServer@capholding.gruppocap.it",
                    _arrayDestinatari[0].ToString(), "GRI - Conferma Rimborsi ",
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
                        }
                    }
                }
                message.Body = message.Body + System.Environment.NewLine;
                if (documento.DataEmissione != null)
                    message.Body = message.Body + " Il rimborso n. " + documento.NumeroDocumento + " del " + documento.DataEmissione.Value.ToShortDateString() + " del cliente" + documento.RagioneSociale + " e' stato confermato.";
                else message.Body = message.Body + " Il rimborso n. " + documento.NumeroDocumento + " del cliente " + documento.RagioneSociale + " e' stato confermato.";
            }
            //string server = "smtp.gruppocap.it";
            //IMailSender mailSender = new SmtpMailSender(server);
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gruppocap.it";
                client.Credentials = new System.Net.NetworkCredential(" ", " ");
                client.Send(message);
                //mailSender.Send(message, false);
                return String.Empty;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return ex.Message;
            }
        }

        [HttpPost]
        [Authorize]
        public String SendMailChangeIBAN(String CodiceCliente, string VecchioIban)
        {
            MailMessage message = new MailMessage();
            Cliente _cliente = _clienteService.InfoCliente(CodiceCliente);

            //String destinatario = System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();
            //destinatario = destinatario.Trim();
            //var _arrayDestinatari = destinatario.Split(';');

            String managerEmail = _rimborsoService.GetManagerMail_Iban(RevoRequest.CurrentUser.MainGroupingCode);

            if (string.IsNullOrEmpty(managerEmail))
                managerEmail = System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();

            if (string.IsNullOrEmpty(managerEmail))
                return string.Empty;

            var _arrayDestinatari = managerEmail.Split(';');

            if (message.Body.Length == 0)
            {
                message = new MailMessage("ApplicationServer@capholding.gruppocap.it",
                _arrayDestinatari[0].ToString(), "GRI - Variazione IBAN cliente ",
                "");
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
                    }
                }
            }
            string newIban = _clienteService.GetIBAN(CodiceCliente)?.CodiceIBAN;
            if (string.IsNullOrEmpty(newIban))
                return "Mail non inviata. Nuovo IBAN non trovato.";

            StringBuilder sb = new StringBuilder();

            sb.Append($"Notifica: </br>Al cliente <strong>{_cliente.ragioneSocialeCliente} ({_cliente.codCliente})</strong> e' stato assegnato il nuovo IBAN: <strong>{_clienteService.GetIBAN(CodiceCliente)?.CodiceIBAN}</strong>.");

            if (string.IsNullOrEmpty(VecchioIban))
                sb.Append($"</br><u>Il Vecchio IBAN non era presente.</u>");
            else
                sb.Append($"</br>Il Vecchio IBAN era: <strong>{VecchioIban}</strong>");

            message.Body = sb.ToString();
            message.IsBodyHtml = true;

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

        [HttpPost]
        [Authorize]
        public String SendMailCambioBeneficiario(List<GestioneRimborso> Rimborsi, List<String> VecchioBeneficiario)
        {
            MailMessage message = new MailMessage();
            String Utente = CheckUtente("");

            int i = 0;
            foreach (var documento in Rimborsi)
            {
                String destinatario = System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();
                destinatario = destinatario.Trim();
                var _arrayDestinatari = destinatario.Split(';');

                if (message.Body.Length == 0)
                {
                    message = new MailMessage("ApplicationServer@capholding.gruppocap.it",
                    _arrayDestinatari[0].ToString(), "GRI - Cambio Beneficiario ",
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
                        }
                    }
                }
                message.Body = message.Body + System.Environment.NewLine;
                message.Body = message.Body + System.Environment.NewLine;
                if (documento.DataEmissione != null)
                    message.Body = message.Body + "Il rimborso n. " + documento.NumeroDocumento + " del " + documento.DataEmissione.Value.ToShortDateString() + " che aveva come beneficiario originale "
                    + VecchioBeneficiario[i].ToString() + " ora e' stato cambiato in: " + documento.Beneficiario + ".";
                else message.Body = message.Body + "Il rimborso n. " + documento.NumeroDocumento + " che aveva come beneficiario originale "
                    + VecchioBeneficiario[i].ToString() + " ora e' stato cambiato in: " + documento.Beneficiario + ".";
                i++;
            }
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

    }
}