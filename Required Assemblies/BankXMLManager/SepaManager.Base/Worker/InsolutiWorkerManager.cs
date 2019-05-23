using PetaPoco;
using SepaManager.Base.Entity.Insoluti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SepaManager.Base.Worker
{
    public class InsolutiWorkerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string SepaConnectionString
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionString"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionString"];
                else
                    throw new ApplicationException("SEPAConnectionString setting not found!");
            }
        }

        public FileInsoluti SaveXml(string XmlInsoluti, DateTime DataRegistrazione, string Utente)
        {
            FileInsoluti file = new FileInsoluti();
            if (string.IsNullOrEmpty(XmlInsoluti))
                throw new Exception("Xml non valorizzato correttamente");

            try
            {
                log.DebugFormat("Ricevuto file {0}", XmlInsoluti);
                file.XmlString = XmlInsoluti;

                XDocument xdoc = XDocument.Parse(file.XmlString);
                //var xmlNs = new XmlNamespaceManager();
                List<InsolutoObject> insoluti = new List<InsolutoObject>();
                //xdoc.XPathSelectElements("//*[local-name() = 'OrgnlPmtInfAndSts']").ToList().ForEach(p =>
                xdoc.XPathSelectElements("//*[local-name() = 'TxInfAndSts']").ToList().ForEach(p =>
                {
                    InsolutoObject insoluto = new InsolutoObject();
                    var pDoc = XDocument.Parse(p.ToString());

                    insoluto.COD_BOLLETTA = p.Descendants().Where(a => a.Name.LocalName == "OrgnlEndToEndId").ToList().Count == 0 ? "Non trovato" : p.Descendants().Where(a => a.Name.LocalName == "OrgnlEndToEndId").First().Value.Trim();
                    try
                    {
                        insoluto.COD_CLIENTE_INTEGRA = p.Descendants().Where(a => a.Name.LocalName == "Dbtr").Single()
                                                  .Descendants().Where(c => c.Name.LocalName == "OrgId").First()
                                                  .Descendants().Where(d => d.Name.LocalName == "Id").First().Value.Trim();
                    }
                    catch { insoluto.COD_CLIENTE_INTEGRA = string.Empty; }

                    if (pDoc.XPathSelectElement("//*[local-name() = 'Dbtr']/*[local-name() = 'Nm']")!=null)
                        insoluto.DES_RAGIONE_SOCIALE = pDoc.XPathSelectElement("//*[local-name() = 'Dbtr']/*[local-name() = 'Nm']").Value.Trim().Left(40);
                                                                                                                                             //insoluto.DES_RAGIONE_SOCIALE = p.Descendants().Where(a => a.Name.LocalName == "Dbtr").Single().Descendants().Where(c => c.Name.LocalName == "Nm").First().Value.Trim();
                    if (p.Descendants().Where(a => a.Name.LocalName == "InstdAmt").Any())
                        insoluto.IMPO_BOLLETTA = Convert.ToDecimal(p.Descendants().Where(a => a.Name.LocalName == "InstdAmt").First().Value.Trim().Replace(".", ","));

                    insoluto.FLG_STORNO = "1";
                    insoluto.DTA_INS = DateTime.Now;
                    insoluto.COD_UTENTE_INS = Utente;
                    insoluto.DES_NOTE = string.Empty;
                    insoluto.DTA_REGISTRAZIONE = DataRegistrazione;
                    insoluto.DATA_SCADENZA_BOLLETTA = Convert.ToDateTime(p.Descendants().Where(a => a.Name.LocalName == "ReqdColltnDt").First().Value.Trim());
                    insoluto.CAUSALE = p.Descendants().Where(a => a.Name.LocalName == "Rsn").First().Value.Trim();
                    insoluti.Add(insoluto);

                });
                file.Transazioni = insoluti;
                log.DebugFormat("Restituisco {0} righe", insoluti.Count());

                //file.Transazioni = from p in xdoc.XPathSelectElements("//*[local-name() = 'OrgnlPmtInfAndSts']")
                //                   //where p.Name.LocalName == "OrgnlPmtInfAndSts"
                //                   select new InsolutoObject
                //                          {
                //                              COD_BOLLETTA = p.Descendants().Where(a => a.Name.LocalName == "OrgnlEndToEndId").ToList().Count == 0 ? "Non trovato" : p.Descendants().Where(a => a.Name.LocalName == "OrgnlEndToEndId").First().Value.Trim(),
                //                              //COD_CLIENTE_INTEGRA = p.Descendants().Where(a => a.Name.LocalName == "Dbtr").Single()
                //                              //                          .Descendants().Where(c => c.Name.LocalName == "OrgId").First()
                //                              //                          .Descendants().Where(d => d.Name.LocalName == "Id").First().Value.Trim(),
                //                              //DES_RAGIONE_SOCIALE = p.Descendants().Where(a => a.Name.LocalName == "Dbtr").Single().Descendants().Where(c => c.Name.LocalName == "Nm").First().Value.Trim(),
                //                              //IMPO_BOLLETTA = Convert.ToDecimal(p.Descendants().Where(a => a.Name.LocalName == "InstdAmt").First().Value.Trim()),
                //                              //FLG_STORNO = "1",
                //                              //DTA_INS = DateTime.Now,  
                //                              //COD_UTENTE_INS = Utente, 
                //                              //DES_NOTE = string.Empty,
                //                              //DTA_REGISTRAZIONE = DataRegistrazione, 
                //                              //DATA_SCADENZA_BOLLETTA = Convert.ToDateTime(p.Descendants().Where(a => a.Name.LocalName == "ReqdColltnDt").First().Value.Trim()),


                //                              //DATA_EMISSIONE_BOLLETTA = Convert.ToDateTime(p.Descendants().Where(a => a.Name.LocalName == "ReqdColltnDt").First().Value.Trim()),
                //                              //CAUSALE = p.Descendants().Where(a => a.Name.LocalName == "Rsn").First().Value.Trim(),
                //                              //COD_CLIENTE_INTEGRA = p.Descendants().Where(a => a.Name.LocalName == "OrgnlInstrId").First().Value.Trim(),
                //                              //DES_NOTE = p.Descendants().Where(a => a.Name.LocalName == "AddtlInf").First().Value.Trim(),
                //                          };

                //var riepilogo = from riep in xdoc.Descendants()
                //                where riep.Name.LocalName == "CBISDDStsRptLogMsg"
                //                select new
                //                {
                //                    ImportoTotaleDichiarato = Convert.ToDecimal(riep.Descendants().Where(a => a.Name.LocalName == "DtldCtrlSum").First().Value.Trim()),
                //                    NumeroTransazioniDichiarate = Convert.ToInt32(riep.Descendants().Where(a => a.Name.LocalName == "DtldNbOfTxs").First().Value.Trim())
                //                };
                //if (riepilogo == null || (!(riepilogo.Count() > 0)))
                //    throw new Exception("Non è stato possibile leggere i dati di riepilogo contenuti nel file caricato");

                //if (!(riepilogo.First().ImportoTotaleDichiarato == file.ImportoCalcolato))
                //    throw new Exception(string.Format("Il totale dichiarato nella testata del file ({0}) risulta diverso da quello calcolato nelle singole transazioni ({1})", riepilogo.First().ImportoTotaleDichiarato, file.ImportoCalcolato));

                //if (!(riepilogo.First().NumeroTransazioniDichiarate == file.NumeroTransazioniCalcolate))
                //    throw new Exception(string.Format("Il numero di transazioni dichiarato nella testata del file ({0}) non risulta coerente con quelle inserite del dettaglio del file ({1})", riepilogo.First().NumeroTransazioniDichiarate, file.NumeroTransazioniCalcolate));


            }
            catch (Exception ex)
            { throw new Exception("Si è verificato un problema nella lettura del file: " + ex.Message); }

            if (!(file.Transazioni.Count() > 0))
                throw new Exception("Acquisizione interrotta. Nessun insoluto trovato");

            //Per ora non serve più
            //SaveFile(file);
            return file;
        }
        //Per ora non serve più
        //private void SaveFile(FileInsoluti file)
        //{

        //    using (Database db = new Database(SepaConnectionString, new OracleProvider()))
        //    {
        //        try
        //        {
        //            db.BeginTransaction();
        //            file.Transazioni.ToList().ForEach(transazione =>
        //            {
        //                var sql = Sql.Builder.Append("INSERT INTO INSO_RID_BANCA")
        //                                                    .Append(" (COD_BOLLETTA, COD_CLIENTE_INTEGRA, DES_RAGIONE_SOCIALE,")
        //                                                    .Append(" DATA_EMISSIONE_BOLLETTA, IMPO_BOLLETTA, CAUSALE, ")
        //                                                    .Append(" FLG_STORNO, DTA_INS, COD_UTENTE_INS, ")
        //                                                    .Append(" DES_NOTE, DTA_REGISTRAZIONE, COD_ID_ACQUISIZIONE, ")
        //                                                    .Append(" DATA_SCADENZA_BOLLETTA )")
        //                                                    .Append("")
        //                                                    .Append(" VALUES ( @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12 )",
        //                                                                 transazione.COD_BOLLETTA, transazione.COD_CLIENTE_INTEGRA, transazione.DES_RAGIONE_SOCIALE,
        //                                                                 transazione.DATA_EMISSIONE_BOLLETTA, transazione.IMPO_BOLLETTA, transazione.CAUSALE,
        //                                                                 transazione.FLG_STORNO, transazione.DTA_INS, transazione.COD_UTENTE_INS,
        //                                                                 transazione.DES_NOTE, transazione.DTA_REGISTRAZIONE, transazione.COD_ID_ACQUISIZIONE,
        //                                                                 transazione.DATA_SCADENZA_BOLLETTA);
        //                var esito = db.Execute(sql);
        //                if (esito == 0)
        //                    throw new Exception("Errore nel salvataggio delle transazioni ");
        //            });

        //            db.CompleteTransaction();
        //        }
        //        catch (Exception ex)
        //        {
        //            db.AbortTransaction();
        //            throw new Exception(ex.Message);
        //        }
        //    }
        //}
    }
}
