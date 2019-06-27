using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestioneRimborsi.Core.Models;
using GestioneRimborsi.Core.Process;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public enum stato { acq = 1, inelab, elab, inval, val, approvato };
    public enum esito { KO = 0, OK, KO1, KO2, OK2, KO3, KO4, KO5, KO6, OK6, KO7, INTEGRA, FORCE };

    public class BonusIdricoService : IBonusIdricoService
    {
        IBonusIdricoRepo bi;

        public BonusIdricoService(IBonusIdricoRepo _bi)
        {
            this.bi = _bi;
        }

        public IEnumerable<BICapLotto> GetLotto(DateTime DataAcquisizione, DateTime DataCarico, DateTime DataScadenza, string Desc, string file = null)
        {
            //if(file!=null)
            //    Xml2Struct.UploadXml(bi);s

            if (file != null)
                try
                {
                    {
                        XmlProcessor proc = new XmlProcessor();
                        proc.ReadRequests(bi, DataAcquisizione, DataCarico, DataScadenza, Desc, file);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            // Validazione.UpLoadLotto(bi);
            return GetLotti();
        }

        public IEnumerable<BICapLotto> GetLotti()
        {
            try
            {
                return bi.getRequestLots(null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public BICapLotto getLotById(int lotId)
        {
            return bi.getRequestLotById(lotId);
        }

        public string GenerateOutcomeFile(int lotId)
        {
            XmlProcessor proc = new XmlProcessor();
            return proc.CreateOutcomeFile(bi, lotId);
        }

        public void InsertLotto(BICapLotto lotto)
        {
            bi.InsertCapLotto(lotto);
        }

        public IEnumerable<SgateRichieste> GetRichieste(int id)
        {
            return bi.getOriginalSGATeRequests(id);
        }

        public List<BICapRequest> GetCapReqS(int id)
        {
            return bi.getCAPRequestById(id);
        }

        public BICapRequest GetCapReq(int id)
        {
            try
            {
                return bi.CapReqByID<BICapRequest>(id);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw;
            }
        }

        public IEnumerable<BICapLotto> ValidaLotto(int lotto, List<int> reqId = null)
        {
            try
            {
                return this.newValidateLotto(lotto);
            }
            catch
            {
                throw;
            }

            //    int validate = 0;
            //    int autoval = 0;
            //    BIEsito err = null;
            //    var nodo = bi.getRequestLots(lotto).FirstOrDefault();
            //    BICapRequest res;
            //    var nodi = bi.getOriginalSGATeRequest(lotto);
            //    string stress = "";

            //    foreach (var item in nodi)
            //    {
            //      //  VALIDAZIONE PER UTENTI INDIVIDUALI
            //                    if (item.CodUtenteInd != null)
            //        {
            //            if (item.EsitoD == esito.OK.ToString())
            //            {
            //                continue;
            //            }

            //            var temp = item.EsitoD;
            //            var id = item.Id;

            //            try
            //            {
            //                if (reqId.Contains(item.Id))
            //                {
            //                    item.EsitoD = esito.OK.ToString();
            //                    stress = '-' + esito.FORCE.ToString();
            //                }
            //                else
            //                    throw new Exception();
            //            }
            //            catch (Exception e)
            //            {
            //                err = Validazione.CheckReqInd(item, bi, nodo.DataScadenza);
            //               var result = err.error >= 6 ? esito.KO1.ToString() :
            //                             Validazione.CheckEsito(err, ref validate);
            //                try
            //                {
            //                    if (validate > 0 && item.EsitoD.Contains(esito.OK.ToString()))
            //                        validate += 0;
            //                }
            //                catch (NullReferenceException ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //                item.EsitoD = result;

            //                item.EsitoS = err.esito == "" ? esito.OK.ToString() : err.esito;
            //            }

            //            if (nodo.Status == 3)
            //            {
            //                autoval = nodo.RichiesteAutoVal;
            //                res = bi.getCapRequestById(item.Id);
            //                item.EsitoD = res.EsitoManVal != null ? res.EsitoManVal : item.EsitoD;
            //                res.EsitoAutoVal = res.EsitoManVal != null ? res.EsitoAutoVal : item.EsitoD;
            //                res.Esito = item.EsitoS;
            //                res.Processato = item.EsitoD.Contains(esito.OK.ToString()) ? 1 : res.Processato;
            //                bi.UpdateCapReq("GRI_BI_REQUEST_CAP", "BI_REQ_CAP_ID", res);
            //            }
            //            else
            //            {
            //                var capReq = new BICapRequest();
            //                capReq.codCliente = err.cliente != null ? err.cliente : "ND";
            //                capReq.Cognome = item.IndCognome;
            //                capReq.Nome = item.IndNome;
            //                capReq.Integra = item.CodUtenteInd;
            //                capReq.lotId = lotto;
            //                capReq.EsitoAutoVal = item.EsitoD;
            //                capReq.Esito = item.EsitoS;
            //                capReq.Id = item.Id;
            //                capReq.TipoUtente = 1;
            //                capReq.Processato = item.EsitoD.Contains(esito.OK.ToString()) ? 1 : capReq.Processato;
            //                bi.InsertCapReq(capReq);
            //            }

            //            bi.UpdateSgateReq("GRI_BI_REQUEST", "BI_REQ_ID", item);
            //        }

            ////        VALIDAZIONE PER UTENTI CENTRALIZZATI
            //        else if (item.CodUtenteInd == null)
            //        {
            //            if (item.EsitoD == esito.OK.ToString())
            //            {
            //                continue;
            //            }

            //            var temp = item.EsitoD;

            //            try
            //            {
            //                if (reqId.Contains(item.Id))
            //                {
            //                    item.EsitoD = esito.OK.ToString();
            //                    stress = '-' + esito.FORCE.ToString();
            //                }
            //                else
            //                    throw new Exception();
            //            }
            //            catch (Exception e)
            //            {
            //                err = Validazione.CheckReqCentr(item, bi, nodo.DataScadenza);

            //             var   result = err.error >= 4 ? (esito.KO1).ToString() :// err.error == 99 ? esito.KO2.ToString() :
            //                             Validazione.CheckEsitoCentr(err, ref validate);

            //                try
            //                {
            //                    if (validate > 0 && item.EsitoD.Contains(esito.OK.ToString()))
            //                        validate += 0;
            //                }
            //                catch (NullReferenceException ex)
            //                {
            //                    Console.WriteLine(ex.Message);
            //                }
            //                item.EsitoD = result;

            //                item.EsitoS = err.esito == "" ? esito.OK.ToString() : err.esito;
            //                autoval += item.EsitoD.Contains("OK") == true ? 1 : 0;
            //            }

            //            if (nodo.Status == 3)
            //            {
            //                autoval = nodo.RichiesteAutoVal;
            //                res = bi.getCapRequestById(item.Id);
            //                item.EsitoD = res.EsitoManVal != null ? res.EsitoManVal : item.EsitoD;
            //                res.EsitoAutoVal = res.EsitoManVal != null ? res.EsitoAutoVal : item.EsitoD;
            //                res.Esito = item.EsitoS;
            //                res.Processato = item.EsitoD.Contains(esito.OK.ToString()) ? 1 : res.Processato;
            //                bi.UpdateCapReq("GRI_BI_REQUEST_CAP", "BI_REQ_CAP_ID", res);
            //            }
            //            else
            //            {
            //                var capReq = new BICapRequest();
            //                capReq.codCliente = err.cliente;
            //                capReq.Denominazione = item.CentrDenCondominio;
            //                capReq.Integra = item.CodUtenteCentr;
            //                capReq.lotId = lotto;
            //                capReq.EsitoAutoVal = item.EsitoD;
            //                capReq.Esito = item.EsitoS;
            //                capReq.Id = item.Id;
            //                capReq.TipoUtente = 0;
            //                capReq.Processato = item.EsitoD.Contains(esito.OK.ToString()) ? 1 : capReq.Processato;
            //                bi.InsertCapReq(capReq);
            //            }

            //            bi.UpdateSgateReq("GRI_BI_REQUEST", "BI_REQ_ID", item);
            //        }
            //        else
            //            continue;
            //    }

            //    var capreqlist = bi.getCapRequestsByLot<BICapRequest>(lotto);
            //    var tot = capreqlist.Where(x => x.Processato == 1).Count();
            //    nodo.Status = capreqlist.Where(x => x.Processato == 1).Count() == nodo.RichiesteTotali ? (int)stato.val : (int)stato.elab;

            //    nodo.Status = validate < nodi.Count() ? (int)stato.elab : (int)stato.val;
            //    nodo.RichiesteTotali = nodi.Count();
            //    nodo.RichiesteAutoVal += validate;
            //    nodo.RichiesteVal += validate;
            //    bi.UpdateCapLotto("GRI_BI_LOTS_CAP", "BI_CAP_ID", nodo);

            //    bi.CalcoloBonusIdrico(nodo.Id);

            //    return bi.getRequestLots(null);

        }

        protected IEnumerable<BICapLotto> newValidateLotto(int lotId)
        {
            try
            {
                bi.ValidateLot(lotId);
                bi.CalcoloBonusIdrico(lotId);
            }
            catch (Exception e)
            {
                throw e;
            }
            return bi.getRequestLots(null);

        }

        public IEnumerable<BICapLotto> ConfermaLotto(int lotId)
        {
            bi.ConfirmLot(lotId);
            return bi.getRequestLots(null);
        }

        public SgateRichieste GetCliente(int id)
        {
            try
            {
                return bi.getOriginalSGATeRequestById(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public BICapRequest getValidatedRequest(int requestId)
        {
            try
            {
                return bi.getCapRequestById(requestId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int SetIntegra(int id, string integra, List<string> lscode, string codcliente)
        {
            int res = 1;
            try
            {
                var nodo = bi.getOriginalSGATeRequestById(id);
                var nodocap = bi.getCapRequestById(id);
                var lotto = bi.getRequestLotById(nodo.lotCapId);

                var status = lscode.Where(x => x.Contains(esito.OK.ToString())).Count();

                if (integra != "" && status > 0)
                {
                    try
                    {
                        var dativalidazione = bi.CodCLienteByIntegra(integra.Trim());
                        nodocap.codCliente = dativalidazione.codCliente.Trim();
                        nodocap.Integra = integra.Trim();
                    }
                    catch (NullReferenceException e)
                    {
                        throw e;
                    }
                }

                nodocap.Processato = 1;
                nodocap.codCliente = codcliente.Trim();

                var capreqlist = bi.getCapRequestsByLot(lotto.Id);

                nodo.EsitoD = lscode.Count() == 0 ? nodo.EsitoD : string.Join(",", lscode.ToArray());
                nodocap.EsitoManVal = string.Join(",", lscode.ToArray());

                lotto.Status = capreqlist.Where(x => x.Processato == 1).Count() >= lotto.RichiesteTotali ? (int)stato.val : (int)stato.elab;
                lotto.RichiesteVal = capreqlist.Where(x => x.Processato == 1).Count();

                bi.UpdateIntegraTable(nodo, nodocap, lotto);

                bi.CalcoloBonusIdrico(lotto.Id);
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        public List<BIContratto> GetUtenze(string cliente)
        {
            if (!cliente.Equals("ND"))
            {
                try
                {
                    List<BIContratto> nodi = null;
                    if (!string.IsNullOrEmpty(cliente))
                        nodi = bi.GetContrattoByCliente(string.Format(@"select * from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE='{0}'", cliente));
                    return nodi;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return new List<BIContratto>();
        }

        public BIContratto GetUtenza(string utenza)
        {
            try
            {
                var nodi = bi.GetContrattoByCliente(string.Format(@"select * from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE_INTEGRA='{0}'", utenza)).First();
                return nodi;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<BICapRequest> getLotDetails(QueryOptions options)
        {
            try
            {
                return bi.getLotDetails(options);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int getLotRequestCount(QueryOptions options)
        {
            return bi.getLotRequestCount(options);
        }

        public List<LotProgressInfo> getLotProgress()
        {
            return bi.getLotProgress();
        }

        public Comune getComuneByIstat(string istat)
        {
            return bi.getComuneByIstat(istat);
        }

        public List<BiInfoNuoviClienti> GetInfoNuoviClienti(int id)
        {
            return bi.GetInfoNuoviClienti(id);
        }
    }
}


