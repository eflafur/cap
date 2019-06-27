using GestioneRimborsi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestioneRimborsi.Core.Process;
using System.IO;
using GestioneRimborsi.Core.Models;
using GestioneRimborsi.Web.Models;
using GruppoCap.Core.Mvc;
using GruppoCap.DAL;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Web.Controllers
{
    public class BonusIdricoController : Controller
    {
        IBonusIdricoService bis;

        public BonusIdricoController(IBonusIdricoService _bis)
        {
            this.bis = _bis;
        }

        public ActionResult Bi()
        {
            return View();
        }

        public JsonResult Acquisizione()
        {
            string messaggio = "Caricamento Lotti impossibile";
            IEnumerable<BICapLotto> model;
            try {
                model = bis.GetLotti();
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 531;
                var error= (new HttpStatusCodeResult(0, messaggio));
                return Json(error);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SgateValidate(int lotto, List<int> reqId = null)
        {
            string messaggio = "errore nella procedura di validazione lotto";
            IEnumerable<BICapLotto> model;
            try
            {
                model = bis.ValidaLotto(lotto, reqId);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 536;
                var error = (new HttpStatusCodeResult(0,messaggio));
                return Json(error);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LotConfirm(int lotto)
        {
            var model = bis.ConfermaLotto(lotto);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetLotti()
        //{
        //    var model = bis.GetLotto();
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetCapReqS(int lotto)
        {
            var model = bis.GetCapReqS(lotto);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Recupera i dati di dettaglio di un lotto di richieste.
        /// </summary>
        /// <param name="lotId">E' l'id del lotto da ricercare</param>
        /// <param name="pageSize">e' la dimensione della pagina del risultato da restituire</param>
        /// <param name="pageIndex">e' l'indice della pagina da visualizzare</param>
        /// <param name="criterias">oggetto che contiene i criteri di ricerca con cui filtrare i risultati</param>
        /// <returns></returns>
        public JsonResult getLotDetails(int lotId, int pageSize, int pageIndex, Models.LotDetailsSearchCriterias criterias)
        {
            string messaggio = "Recupero dati impossibile";
            Models.SearchResults<BICapRequest> results = new Models.SearchResults<BICapRequest>() { PageIndex = pageIndex, PageSize = pageSize };
            QueryOptions options = new QueryOptions()
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            options.ConditionCriterias.Add(new QueryOptionLotId(), lotId.ToString());
            if (criterias.CustomerType > 0)
                options.ConditionCriterias.Add(new QueryOptionUserType(), criterias.CustomerType.ToString());
            if (criterias.RequestStatus !=2)
                options.ConditionCriterias.Add(new QueryOptionRequestStatus(), criterias.RequestStatus.ToString());
            if (!string.IsNullOrWhiteSpace(criterias.Outcome))
                options.ConditionCriterias.Add(new QueryOptionOutcome(), criterias.Outcome);
            if (criterias.RequestId > 0)
                options.ConditionCriterias.Add(new QueryOptionRequestId(), criterias.RequestId.ToString());
            int recordCount = bis.getLotRequestCount(options);
            results.RecordCount = recordCount;

            try
            {
                results.Results = bis.getLotDetails(options);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 533;
                var error = (new HttpStatusCodeResult(0, messaggio));
                return Json(error);
            }

            return Json(results, JsonRequestBehavior.DenyGet);

        }
        public JsonResult GetClient(int id)
        {
            string messaggio = "Recupero dati utente impossibile";

            SgateRichieste model;
            BICapRequest modelcap;
            BICapLotto modelLot;
            try
            {
                model = bis.GetCliente(id);
                modelcap = bis.GetCapReq(id);
                modelLot = bis.getLotById(model.lotCapId);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 534;
                var error = (new HttpStatusCodeResult(0, messaggio));
                return Json(error);
            }            
            SgateCapModel capmodel = new SgateCapModel();
            capmodel.sgate = model;
            capmodel.integra = modelcap.Integra;
            capmodel.ImportoBi = modelcap.ImportoBi;
            capmodel.ImportoIntegrativo = modelcap.ImportoIntegrativo;
            capmodel.status = (modelLot != null ? modelLot.Status : -1);
            return Json(capmodel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getRequestValidationDetails(int requestId)
        {
            string messaggio = "Recupero dettaglio validazione impossibile";

            BICapRequest model;
            try
            {
                 model = bis.getValidatedRequest(requestId);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 535;
                var error = (new HttpStatusCodeResult(0, messaggio));
                return Json(error);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ForceIntegra(int id, string integra, List<string> lscode, string codcliente)
        {
            int res = 0;
            string messaggio="Validazione non effettuata";
            try
            {
                 res=bis.SetIntegra(id, integra, lscode, codcliente);
            }
            catch (Exception ex)
            {

               if (ex.Data.Contains("messaggio"))
                     messaggio =ex.Data["messaggio"].ToString();

                Response.StatusCode = 537;
                var error = (new HttpStatusCodeResult(0, messaggio));
                return Json(error);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetcontrattoDetail(string codCliente)
        {
            string messaggio="imposibile recuparere i dati";
            List<BIContratto> model;
            try
            {
                model = bis.GetUtenze(codCliente);
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("messaggio"))
                    messaggio = ex.Data["messaggio"].ToString();
                Response.StatusCode = 534;
                var error = (new HttpStatusCodeResult(0,messaggio));
                return Json(error);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUtenza(string utenza)
        {
            var model = bis.GetUtenza(utenza);
            if (model == null)
                return Json("KO", JsonRequestBehavior.AllowGet);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ExcelResult loadExcelNuoviClienti(int lotId)
        {            
            var res = bis.GetInfoNuoviClienti(lotId).ToSubCollection<BiInfoNuoviClienti>();

            Dictionary<String, String> settings = new Dictionary<String, String>()
                {
                        { "ID_LOTTO", "ID_LOTTO" },
                        { "CLIENTE_ORIGINALE", "CLIENTE_ORIGINALE" },
                        { "COD_CLIENTE_RIMBORSATO", "COD CLIENTE RIMBORSATO" },
                        { "TIPOLOGIA_RICHIESTA", "TIPOLOGIA_RICHIESTA" },
                        { "IMPORTO_BONUS_INTEGRATIVO", "IMPORTO_BONUS_INTEGRATIVO" },
                        { "IMPORTO_BONUS_SOCIALE", "IMPORTO_BONUS_SOCIALE" },
                        { "IMPORTO_TOTALE_RIMBORSO", "IMPORTO_TOTALE_RIMBORSO" },
                        { "ID_RICHIESTA", "ID_RICHIESTA" },
                        { "PROTOCOLLO_RICHIESTA", "PROTOCOLLO_RICHIESTA" },
                        { "PROTOCOLLO_DOMANDA", "PROTOCOLLO_DOMANDA" },
                        { "RAGIONE_SOCIALE", "RAGIONE_SOCIALE" },
                        { "CODICE_FISCALE", "CODICE_FISCALE" },
                        { "COMUNE_FORNITURA", "COMUNE_FORNITURA" },
                        { "INDIRIZZO_FORNITURA", "INDIRIZZO_FORNITURA" },
                        { "CAP_FORNITURA", "CAP_FORNITURA" },
                        { "DATA_RICHIESTA", "DATA_RICHIESTA" },
                        { "DATA_SCADENZA_RICHIESTA", "DATA_SCADENZA_RICHIESTA" }                        
                    };

            System.Data.DataTable _t = res.Items.ToDataTable<BiInfoNuoviClienti>(settings);
            _t.TableName = "ClientiBiin_" + DateTime.Now.ToString("yyyyMMddHHmmssms");
            var fileResult = new ExcelResult(_t);
            return fileResult;
        }

        [HttpPost]
        public JsonResult UploadFile( DateTime DataAcquisizione, DateTime DataCarico, DateTime DataScadenza, string Desc, HttpPostedFileBase file = null)
        {
            string newFileName = string.Format("App_Data/Uploads/{0}.xml", Guid.NewGuid().ToString());

            using (var fileStream = System.IO.File.Create(Server.MapPath(newFileName)))
            {
                file.InputStream.Seek(0, SeekOrigin.Begin);
                file.InputStream.CopyTo(fileStream);
                fileStream.Close();
            }

            var model = bis.GetLotto(DataAcquisizione,DataCarico, DataScadenza,Desc,newFileName);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadOutcome(int id)
        {
            var fileName = string.Format("outcomeToSgate-lot{0}.xml", id);

            var fileGenerated = bis.GenerateOutcomeFile(id);

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileGenerated);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public JsonResult getLotProgress() {
            var result = bis.getLotProgress();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getComuneByIstat(string istat)
        {
            var result = bis.getComuneByIstat(istat);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
