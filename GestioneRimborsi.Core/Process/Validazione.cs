using GestioneRimborsi.Core.xsd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using GestioneRimborsi.Core.Process;

namespace GestioneRimborsi.Core.Process
{
    public class Validazione
    {
        private static BIEsito StatoErrori = new BIEsito();
        
        
        public static BIEsito CheckReqInd(SgateRichieste req, IBonusIdricoRepo bi,DateTime scadenzalotto)
        {
            StatoErrori.cliente = null;
            string balance = null;
            string esitosgate = null;
            int i=0 ;
            BICLienti ana = null;

            try
            {
                    i = 7;

                    var ret = bi.GetCliente(string.Format("select * from  GRI_DATI_VALIDAZIONE_BI_V  b where b.COD_CODICE_FISCALE = '{0}'", req.IndCf));
                    i = ret != null ? --i : i;
                    balance = ret != null ? "" : '-' + esito.KO3.ToString();
                    esitosgate = ret != null ? "" : '-' + esito.KO3.ToString();

                    var ret1 = bi.GetCliente(string.Format("select * from GRI_DATI_VALIDAZIONE_BI_V  b where b.cod_cliente_integra={0}", req.CodUtenteInd));
                    i = ret1 != null ? --i : i;
                    balance += ret1 != null ? "" : "-INTEGRA";


                req.IndCognome.Replace("'", "''");
                req.IndNome.Replace("'", "''");
                var den = req.IndCognome + " " + req.IndNome;
                    var deninv = req.IndNome + " " + req.IndCognome;
                    var ret2 = bi.GetCliente(string.Format("select * from  GRI_DATI_VALIDAZIONE_BI_V  b where b.des_ragione_sociale like '{0}' or b.des_ragione_sociale like '{1}'", den, deninv));
                    i = ret2 != null ? --i : i;
                    balance += ret2 != null ? "" : "-DEN";

                    ana = ret != null ? ret : ret1 != null ? ret1 : ret2 != null ? ret2 : null;
            }
            catch (Exception e)
            {
                throw;
            }

            StatoErrori.errorana = i;
            StatoErrori.error = i;
            StatoErrori.esito = balance;
            StatoErrori.esitosgate = esitosgate;

            if (i == 7 | ana == null)
                return StatoErrori;

            StatoErrori.cliente = ana.codCliente;

            return CheckReqInd2(req, ana, bi, balance,esitosgate,scadenzalotto);

        }

        public static BIEsito CheckReqInd2(SgateRichieste req, BICLienti cliente, IBonusIdricoRepo bi, string balance,string esitosgate,DateTime scadenzaLotto)
        {
            int i = StatoErrori.error;

            try
            {
                if (i <= 5)
                {
                    req.DataInizioAgev = req.Allineamento == true ? req.DataFineAgev : scadenzaLotto;

                    var stato = bi.GetContratto(string.Format(@"select c.COD_PUNTO_FORNITURA from COM_CONTRATTO c where c.FLG_LAST = 'Y' and c.COD_STATO_CONTRATTO ='CA00'
                                                    and c.cod_intestatario='{0}'", cliente.codCliente));
                    i = stato != null ? --i : i;
                    balance += stato != null ? "" : '-' + esito.KO2.ToString();
                    esitosgate += stato != null ? "" : '-' + esito.KO2.ToString();
                    if (stato == null)
                    {
                        StatoErrori.error = i;
                        StatoErrori.esito = balance;
                        StatoErrori.esitosgate = esitosgate;
                        return StatoErrori;
                    }
                }
                else
                    --i;

                var isresidente = bi.GetContratto(string.Format(@"select IS_DOMESTICORESIDENTE from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE='{0}' AND IS_DOMESTICORESIDENTE = 1", cliente.codCliente));
                i = isresidente != null ? --i : i;
                balance += isresidente != null ? "" : "-DOMESTICORESIDENTE";

                if (isresidente != "1")
                {
                    var isNonResidente = bi.GetContratto(string.Format(@"select IS_DOMESTICONONRESIDENTE from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE='{0}' AND IS_DOMESTICONONRESIDENTE = 1", cliente.codCliente));
                    i = isNonResidente != null ? --i : i;
                    balance += isNonResidente != null ? "" : '-' + esito.KO4.ToString();
                    esitosgate += isNonResidente != null ? "" : '-' + esito.KO4.ToString();

                    BIPuf residenza = bi.GetPuf(string.Format(@"select COMUNE_PUF, INDIRIZZO_PUF,CIVICO_PUF,CAP_PUF from GRI_DATI_VALIDAZIONE_BI_V  where COD_CLIENTE='{0}'", cliente.codCliente));
                    try
                    {
                        var count = residenza.Cap.Trim().Contains(req.IndCap.Trim()) ? 1 : 0;
                        count += residenza.Strada.Trim().Contains(req.IndAreaCirc.Trim()) ? 1 : 0;
                        count += residenza.Civico.Trim().Contains(req.IndCivico.Trim()) ? 1 : 0;
                        i = count == 3 ? --i : i;
                        balance += count == 3 ? "" : '-' + esito.KO5.ToString();
                        esitosgate += count == 3 ? "" : '-' + esito.KO5.ToString();
                    }
                    catch (Exception)
                    {
                        balance += '-' + esito.KO5.ToString();
                    }
                }
                else
                    i = i - 2;

            }
            catch (Exception e)
            {
                throw;
            }

            StatoErrori.error = i;
            StatoErrori.esito = balance;
            StatoErrori.esitosgate = esitosgate;

            return StatoErrori;
        }


        public static string CheckEsito(BIEsito report, ref int validate)
        {
            string val;

            if (report.error == 0)
            {
                val = esito.OK.ToString();
                ++validate;
            }
            else if (report.error == 1 && report.esito.Contains("INTEGRA"))
            {
                val = "OK*";
                ++validate;
            }
            else
                val = report.esitosgate != "" ? report.esitosgate : "KO";
            //  val = report.esito.Split('-').Where(x => x.Contains("OK") || x.Contains("KO")).ToString();


            return val;
        }

        //VALIDAZIONE PER UTENTI CENTRALIZZATI
        public static BIEsito CheckReqCentr(SgateRichieste req, IBonusIdricoRepo bi,DateTime scadenzaLotto)
        {
            BICLienti ret=null ;
            string balance = null;
            string esitosgate = null;
            int i = 5;
            StatoErrori.cliente = null;


            try
            {
                    if (req.CodUtenteCentr != null)
                    {
                        ret = bi.GetCliente(string.Format("select * from GRI_DATI_VALIDAZIONE_BI_V  b where b.cod_cliente_integra={0}", req.CodUtenteCentr));
                        i = ret != null ? --i : i;
                        balance += ret != null ? "" : "-INTEGRA";
                    }
                    else
                        balance += "-INTEGRA";

                    BIPuf residenza = bi.GetPuf(string.Format(@"select COD_CLIENTE, INDIRIZZO_PUF,CIVICO_PUF,CAP_PUF from GRI_DATI_VALIDAZIONE_BI_V  where CIVICO_PUF like('{0}') AND INDIRIZZO_PUF like('{1}') AND
                                                   CAP_PUF like('{2}')", req.CentrCivico, req.CentrAreaCircolazione, req.CentrCap));
                    i = residenza != null ? --i : i;
                    balance += residenza != null ? "" : '-' + esito.KO6.ToString();
                    esitosgate += residenza != null ? "" : '-' + esito.KO6.ToString();

                    if (i == 5)
                    {
                        StatoErrori.error = i;
                        StatoErrori.esito = balance;
                        StatoErrori.esitosgate = esitosgate;
                        return StatoErrori;
                    }

                    var cliente = ret != null ? ret.codCliente : residenza != null ? residenza.codCliente : null;

                    req.DataInizioAgev = req.Allineamento == true ? req.DataFineAgev : scadenzaLotto;

                    var stato = bi.GetContratto(string.Format(@"select c.COD_PUNTO_FORNITURA from COM_CONTRATTO c where c.FLG_LAST = 'Y' and c.COD_STATO_CONTRATTO ='CA00'
                                                    and c.cod_intestatario='{0}'", cliente));
                    i = stato != null ? --i : i;
                    balance += stato != null ? "" : '-' + esito.KO2.ToString();
                    esitosgate += stato != null ? "" : '-' + esito.KO2.ToString();
                    if (stato == null)
                    {
                        StatoErrori.error = 99;
                        StatoErrori.esito = balance;
                        StatoErrori.esitosgate = esitosgate;
                        return StatoErrori;
                    }

                    var den = bi.GetContratto(string.Format(@"select DES_RAGIONE_SOCIALE from GRI_DATI_VALIDAZIONE_BI_V where DES_RAGIONE_SOCIALE LIKE ('{0}')", req.CentrDenCondominio));
                    i = den != null ? --i : i;
                    balance += den != null ? "" : "-DEN";

                    var isCondominiale = bi.GetContratto(string.Format(@"select IS_CONDOMINIALE from GRI_DATI_VALIDAZIONE_BI_V where COD_CLIENTE='{0}'", cliente));
                    i -= isCondominiale != null ? int.Parse(isCondominiale) : 0;
                    balance += isCondominiale == null ? '-' + esito.KO7.ToString() : isCondominiale == "1" ? "" : '-' + esito.KO7.ToString();
                    esitosgate += isCondominiale == null ? '-' + esito.KO7.ToString() : isCondominiale == "1" ? "" : '-' + esito.KO7.ToString();

                    StatoErrori.error = i;
                    StatoErrori.cliente = cliente;
                    StatoErrori.esito = balance;
                    StatoErrori.esitosgate = esitosgate;
            }
            catch (Exception e)
            {
                throw;
            }
            return StatoErrori;

        }

        //VERIFICA ESITO PER UTENTI CENTRALIZZATI
        public static string CheckEsitoCentr(BIEsito report, ref int validate)
        {
            string val = "";


            if ((report.error == 0) || (report.error == 1 && report.esito.Contains("DEN")))
            {
                val = esito.OK.ToString();
                ++validate;
            }

            else if ((report.error <= 2) && ((report.esito.Contains("INTEGRA")) || ((report.esito.Contains("DEN") && report.esito.Contains("INTEGRA")))))
            {
                val = "OK*";
                ++validate;
            }
            else
                val = report.esitosgate != null ? report.esitosgate : "KO";
            //  val = report.esito.Split('-').Where(x=>x.Contains("OK") || x.Contains("KO")).ToString();

            return val;
        }

        public static void UpLoadLotto(IBonusIdricoRepo bi)
        {
            var nodi = bi.getOriginalSGATeRequests(null, "select * from gri_bi_request");
            var nodiT = nodi.Where(x => x.lotCapId == 0 );
            //rilevo data piu vecchia
            if (nodiT.Count() == 0)
                return;
            //       var dt = nodiT.Where(x => x.Id > 0).OrderBy(x => x.ReqDataDoc).First();
            var nodo = new BICapLotto();
            nodo.DataAcquisizione = DateTime.Now;
            nodo.DataCarico = DateTime.Now;
            var olderdata = nodi.OrderBy(x=>x.DataAcquisizione).First();
            nodo.DataScadenza = new DateTime(olderdata.DataAcquisizione.Year, olderdata.DataAcquisizione.Month, 1).AddMonths(2);
            //nodo.DataInvioEsiti = new DateTime(2018, 10, 06);
            nodo.RichiesteTotali = nodiT.Count();
            nodo.RichiesteAutoVal = 0;
            nodo.RichiesteVal = 0;
            nodo.Status = (int)stato.acq;

            bi.InsertCapLotto(nodo);

            var idlotto = bi.getRequestLots(null).Last();

            foreach (var item in nodiT)
            {
                item.lotCapId = idlotto.Id;
                item.DataAcquisizione = DateTime.Now;
                bi.UpdateSgateReq("GRI_BI_REQUEST", "BI_REQ_ID", item);
            }

            ////VERIFICA E TEST IN ASSENZA DI SCARICO SGATE



            //// Create an instance of the XmlSerializer.
            //XmlSerializer serializer = new XmlSerializer(typeof(tipoFileRichiesteSgate));

            //// Declare an object variable of the type to be deserialized.
            //tipoFileRichiesteSgate i;

            //using (Stream reader = new FileStream(HttpContext.Current.Server.MapPath("~/Xml/bonusidrico.xml"), FileMode.Open))
            //{
            //    // Call the Deserialize method to restore the object's state.
            //    i = (tipoFileRichiesteSgate)serializer.Deserialize(reader);
            //}
            //var ret = i.GetHashCode();

        }
    }
}