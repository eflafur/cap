using GruppoCap.Core;
using GruppoCap.Logging;
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
namespace GestioneRimborsi.Web.Controllers
{
    public class LottoRimborsiController : RevoController
    {

        private ILottoRimborsiService _lottorimborsiService = null;
        // CTOR
        public LottoRimborsiController(ILottoRimborsiService lottorimborsiService, ILogger log)
        {
            _lottorimborsiService = lottorimborsiService;
            _log = log;
        }

        ILogger _log;

        public ActionResult List()
        {
            return View(_lottorimborsiService.UsersOfRimborsi());
        }
        public ActionResult InnerList(string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                    return PartialMessage(HtmlSnippets.Alert.Info("Selezionare un utente e premere Cerca per visualizzare i rimborsi..."));

                ISubCollection<Rimborso> _rimborsi = _lottorimborsiService.LottoRimborsiByUserName(UserName);

                if (_rimborsi.Items.HasValues() == false)
                {
                    return PartialMessage(
                        HtmlSnippets.Alert.Warning(string.Format("Non ho trovato rimborsi per l'utente {0}", UserName))
                    );
                }
                ViewData.Add("UserName", UserName);
                return PartialView("_list", _rimborsi);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la lettura dei rimborsi...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la lettura dei rimborsi..."));
            }
        }
        [HttpPost]
        public JsonResult GeneraFileRimborsi(string UserName, DateTime DataValuta)
        {
            var proxy = new BankXMLManager.BankXMLServiceClient();
            var id = proxy.CreateXml(string.Format("{0}_{1}", UserName, DateTime.Now.ToString("yyyyMMddHHmmssms")), UserName, RevoRequest.CurrentUser.UserId);

            if (id <= 0)
                return Json(new { status = "failed", data = new { message = "Errore durante la creazione del file xml..." } });

            try
            {

                BankXMLOutput _XmlOutput = proxy.GetXmlDocument(id);

                IUpdateOperationResult setRimborsiUpdated = null;

                setRimborsiUpdated = _lottorimborsiService.Update(UserName, _XmlOutput.FileName, RevoContext.IdentityManager.CurrentUsername, DataValuta);

                if (!setRimborsiUpdated.GenericMeaning)
                    return Json(new { status = "failed", data = new { message = string.Format("Errore durante l'aggiornamento dei rimborsi...{0}{1}", Environment.NewLine, setRimborsiUpdated.Description) } });
                else _log.Append("Gestione disposizioni -> Generato file xml (" + _XmlOutput.FileName + ") -> Aggiornamento data rimborso: ", LogLevel.Info, null, "La data rimborso nella tabella GIN_INDENNIZZI_CDS è stata aggiornata al " + DataValuta.ToShortDateString() + " dall'utente " + RevoContext.IdentityManager.CurrentUsername);

            }
            catch (Exception ex)
            {
                return Json(new { status = "failed", data = new { message = string.Format("Errore durante l'aggiornamento dei rimborsi...{0}{1}", Environment.NewLine, ex) } });
            }
            
            return Json(new { status = "success", data = new { id = id } });

        }
        public FileResult DownloadFileRimborsi(long id)
        {
            var proxy = new BankXMLManager.BankXMLServiceClient();
            if (id <= 0)
                throw new ApplicationException("Errore durante la creazione del file xml...");
            BankXMLOutput _XmlOutput = proxy.GetXmlDocument(id);
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

            //Aggiorno campo data_valuta


            return File(bytes, "text/xml");
        }

        public ActionResult SetDataValuta(DateTime DataValuta, String UserName)
        {
            if (string.IsNullOrEmpty(UserName))
                return PartialMessage(HtmlSnippets.Alert.Info("Nessun utente selezionato"));

            ISubCollection<Rimborso> rimborsi = _lottorimborsiService.LottoRimborsiByUserName(UserName);
            String result = _lottorimborsiService.SetDataValuta(DataValuta, rimborsi);
            return new EmptyResult();
        }

        public ActionResult ModificaDisposizioniBancarie()
        {
            return View(_lottorimborsiService.UsersOfRimborsi());
        }

        [Authorize]
        public ActionResult SearchSepaHeader()
        {
            try
            {
                ISubCollection<SepaHeader> _elencoDisposizioni = _lottorimborsiService.GetSepaHeader();
                return PartialView("~/Views/LottoRimborsi/_elencoDisposizioni.cshtml", _elencoDisposizioni);
            }
            catch (Exception ex)
            {
                if (RevoContext.IdentityManager.CurrentUser.IsPrivileged)
                    return PartialMessage(HtmlSnippets.Alert.Error(string.Format("Errore durante la ricerca...{0}{1}", Environment.NewLine, ex)));
                else
                    return PartialMessage(HtmlSnippets.Alert.Error("Errore durante la ricerca..."));
            }
        }
    }



    internal sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}