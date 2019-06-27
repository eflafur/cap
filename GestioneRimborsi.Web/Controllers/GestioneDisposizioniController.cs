using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GruppoCap.Core.Data;
using GestioneRimborsi.Core;
using GruppoCap;
using System.Xml;
using System.Text;
using System.IO;
using SepaManager.Base;
using System.Xml.Linq;
using System.Net.Mail;
using GruppoCap.Mail;

namespace GestioneRimborsi.Web.Controllers
{
    public class GestioneDisposizioniController : RevoController
    {

        private ILottoRimborsiService _lottorimborsiService = null;
        // CTOR
        public GestioneDisposizioniController(ILottoRimborsiService lottorimborsiService)
        {
            _lottorimborsiService = lottorimborsiService;
        }

        public ActionResult List()
        {
            return View(_lottorimborsiService.SepaUsers());
        }

        public ActionResult InnerList(string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                    return PartialMessage(HtmlSnippets.Alert.Info("Nessun risultato da mostrare..."));

                ISubCollection<SepaHeader> _elencoDisposizioni = _lottorimborsiService.GetSepaHeaderByUser(UserName);


                if (_elencoDisposizioni.Items.HasValues() == false)
                {
                    return PartialMessage(
                        HtmlSnippets.Alert.Warning(string.Format("Non ho trovato risultati per l'utente {0}", UserName))
                    );
                }
                ViewData.Add("UserName", UserName);
                return PartialView("~/Views/GestioneDisposizioni/_elencoDisposizioni.cshtml", _elencoDisposizioni);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la lettura delle disposizioni...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la lettura delle disposizioni..."));
            }
        }

        public FileResult DownloadFileDisposizioni(long id)
        {
            var proxy = new BankXMLManager.BankXMLServiceClient();

            if (id <= 0)
                throw new ApplicationException("Errore durante la creazione del file xml...");


            //   BankXMLOutput _XmlOutput = proxy.GetXmlDocument(id, true);            
            BankXMLOutput _XmlOutput = proxy.RegenerateXmlDocument(id);

            System.Xml.Linq.XElement _xmlFile = XElement.Parse(_XmlOutput.XmlDocument.OuterXml);
            //StringBuilder fs = new StringBuilder();
            StringWriterWithEncoding fs = new StringWriterWithEncoding(Encoding.UTF8);

            using (XmlWriter xmlW = XmlWriter.Create(fs, new XmlWriterSettings() { OmitXmlDeclaration = false, ConformanceLevel = ConformanceLevel.Document, Encoding = Encoding.UTF8 }))
            {
                _xmlFile.WriteTo(xmlW);
                xmlW.Flush();
            }
            Console.WriteLine("Task Executed");
            byte[] bytes = Encoding.UTF8.GetBytes(fs.ToString());
            HttpContext.Response.Headers.Add("Content-Disposition", string.Format("attachment; filename=\"{0}\"", (_XmlOutput.FileName ?? string.Format("{0}_{1}", id, DateTime.Now.ToString("yyyyMMddHHmmssms"))).EnsureEndsWith(".xml")));

            return File(bytes, "text/xml");
        }

        public FileResult DownloadCsvDisposizioni(long id)
        {
            var proxy = new BankXMLManager.BankXMLServiceClient();

            if (id <= 0)
                throw new ApplicationException("Errore durante la creazione del file csv...");

            List<String> _CsvOutput = proxy.CreateCsvDocument(id);            
            string csv = String.Empty;

            foreach (var item in _CsvOutput)
            {
                csv += item + ";";
                csv += Environment.NewLine;
            }

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "CsvDoc_n" + id.ToString() + ".csv");
        }

        [Authorize]
        public ActionResult SearchSepaHeader()
        {
            try
            {
                ISubCollection<SepaHeader> _elencoDisposizioni = _lottorimborsiService.GetSepaHeader();
                return PartialView("~/Views/GestioneDisposizioni/_elencoDisposizioni.cshtml", _elencoDisposizioni);
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
        public ActionResult SearchSepaCreditTransaction(long id)
        {
            try
            {
                ISubCollection<SepaCreditTransaction> _elencoTransazioni = _lottorimborsiService.GetSepaCreditTransaction(id);
                var count = 0;
                foreach(var item in _elencoTransazioni.Items)
                {
                    count++;
                    if (item.CdtrpStladrTownName == "LONATE POZZOLO   VA")
                    {
                        string comune = item.CdtrpStladrTownName;
                        string provincia = item.CdtrpStladrProvince;
                    }
                }
                return PartialView("~/Views/GestioneDisposizioni/_elencoTransazioni.cshtml", _elencoTransazioni);
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
        public ActionResult DeleteSepaCreditTransaction(long id)
        {
            try
            {
                ISubCollection<SepaCreditTransaction> _elencoTransazioni = _lottorimborsiService.DeleteSepaCreditTransaction(id, RevoRequest.CurrentUser.UserId);
                return PartialView("~/Views/GestioneDisposizioni/_elencoTransazioni.cshtml", _elencoTransazioni);
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
        public ActionResult RecuperaSepaCreditTransaction(long id)
        {
            try
            {
                ISubCollection<SepaCreditTransaction> _elencoTransazioni = _lottorimborsiService.RecuperaSepaCreditTransaction(id, RevoRequest.CurrentUser.UserId);
                return PartialView("~/Views/GestioneDisposizioni/_elencoTransazioni.cshtml", _elencoTransazioni);
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
        public ActionResult SchedaModifiche(long id)
        {
            try
            {
                SepaCreditTransaction _transazione = _lottorimborsiService.GetTransactionByID(id);
                return PartialView("~/Views/GestioneDisposizioni/_schedaModifiche.cshtml", _transazione);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult SchedaElencoModifiche(long id)
        {
            try
            {
                return PartialView("~/Views/GestioneDisposizioni/_schedaElencoModifiche.cshtml", id);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult ModificaDisposizioni(long id, String nuovoIban, String nuovoBeneficiario, String motivazione)
        {
            try
            {

                SepaCreditTransaction _VecchiaTransazione = _lottorimborsiService.GetTransactionByID(id);
                ISubCollection <SepaCreditTransaction> _transazioni = _lottorimborsiService.ModificaTransazione(id, nuovoIban, nuovoBeneficiario, motivazione, RevoRequest.CurrentUser.UserId);

                if (_transazioni.Items.Any())
                {

                    String emailCambioIBAN = SendMailChangeIBAN(_transazioni.Items.FirstOrDefault().CreditorName,_transazioni.Items.FirstOrDefault().CreditorIban, _VecchiaTransazione.CreditorIban);
                    if (!String.IsNullOrEmpty(emailCambioIBAN))
                    {
                        return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante invio mail di notifica per cambio IBAN : {0}", emailCambioIBAN)));
                    }

                }


                return PartialView("~/Views/GestioneDisposizioni/_elencoTransazioni.cshtml", _transazioni);





            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult ModificaMotivazione(long id, String motivazione)
        {
            try
            {
                String _motivazione = _lottorimborsiService.ModificaMotivazione(id, motivazione);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult BloccaDisposizione(long id, String autore)
        {
            try
            {
                if (!String.IsNullOrEmpty(autore))
                {
                    if (RevoRequest.CurrentUser.IsSuperUser() || RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    {
                        ISubCollection<SepaHeader> _disposizioni = _lottorimborsiService.BloccaDisposizione(id, true, RevoRequest.CurrentUser.UserId);
                        return PartialView("~/Views/GestioneDisposizioni/_elencoDisposizioni.cshtml", _disposizioni);
                    }
                    else return new EmptyResult();
                }
                else
                {
                    ISubCollection<SepaHeader> _disposizioni = _lottorimborsiService.BloccaDisposizione(id, false, RevoRequest.CurrentUser.UserId);
                    return PartialView("~/Views/GestioneDisposizioni/_elencoDisposizioni.cshtml", _disposizioni);
                }
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }

        [Authorize]
        public ActionResult GetStoricoModifica(Int32 internalId)
        {
            try
            {
                DisposizioneModificata _modifica = _lottorimborsiService.GetStoricoModifica(internalId);
                return PartialView("~/Views/GestioneDisposizioni/_modificheIbanBeneficiario.cshtml", _modifica);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca ...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }
        [HttpPost]
        [Authorize]
        public String SendMailChangeIBAN(String CreditorName,string CreditoIban,string VecchioIban)
        {
            MailMessage message = new MailMessage();
            //Cliente _cliente = _clienteService.InfoCliente(CodiceCliente);

            //String destinatario = System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();
            //destinatario = destinatario.Trim();
            //var _arrayDestinatari = destinatario.Split(';');

            String managerEmail = _lottorimborsiService.GetManagerMail_Iban(RevoRequest.CurrentUser.MainGroupingCode);

            if (string.IsNullOrEmpty(managerEmail))
                    managerEmail= System.Configuration.ConfigurationManager.AppSettings["EmailResponsabileRimborsi"].ToString();

            var _arrayDestinatari = managerEmail.Split(';');

            if (message.Body.Length == 0)
            {
                message = new MailMessage("ApplicationServer@capholding.gruppocap.it",
                _arrayDestinatari[0].ToString(), "GRI - Cambio IBAN cliente ",
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
            message.Body = "Al cliente " + CreditorName + " e' stato assegnato questo nuovo IBAN: " + CreditoIban + ". Il Vecchio IBAN era:" + VecchioIban;

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


    internal sealed class StringWriterWithEncodings : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncodings(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}