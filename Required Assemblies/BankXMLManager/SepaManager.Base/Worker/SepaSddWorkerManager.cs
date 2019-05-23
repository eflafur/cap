using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using SepaManager.Base.Entity.Sdd;
using System.IO.Compression;
using PetaPoco;

namespace SepaManager.Base.Worker
{
    public static class SepaSddWorkerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Variabili
        /// <summary>
        /// Vista da cui recuperare i dati
        /// </summary>
        public static string ViewName
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SDDViewName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["SDDViewName"];
                else
                    throw new ApplicationException("SDDViewName setting not found!");
            }
        }
        /// <summary>
        /// Stringa di connessione 
        /// </summary>
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
        /// <summary>
        /// Provider
        /// </summary>
        public static string ConnectionProvider
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionProvider"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionProvider"];
                else
                    throw new ApplicationException("SEPAConnectionProvider setting not found!");
            }
        }
        #endregion
        #region Worker
        /// <summary>
        /// Classe che genera il file XML SDD
        /// </summary>
        /// <param name="Batch">Batch di fatturazione di riferimento</param>
        /// <returns>SedaCbiReturn</returns>
        public static SedaCbiSddReturn CreateXml(long Batch)
        {
            log.DebugFormat(string.Format("Ricevuta richiesta di creazione batch per BatchOnline --> {0}", Batch));

            var ret = new SedaCbiSddReturn();
            var XmlList = new List<string>();
            try
            {
                var SddList = CreateXmlWorker(Batch);
                SddList.ForEach(msg => { XmlList.Add(String.Format("{0}§{1}", msg.GrpHdr.MsgId, msg.ToXmlString())); });
                var Stream = CreateZipFile(XmlList);


                SaveZipStream(Batch, Convert.ToBase64String(Stream.ToArray()));
                ret.Stream = null;
                ret.FileExtension = "zip";
                ret.Result = true;
            }
            catch (Exception ex)
            {
                ret.Result = false;
                ret.LastError = ex.Message;
                ret.Stream = null;
                ret.FileExtension = string.Empty;
            }

            return ret;
        }

        public static SedaCbiSddReturn GetXml(long Batch)
        {
            var ret = new SedaCbiSddReturn();
            try
            {
                ret.Stream = GetFileGenerato(Batch);
                SaveHistory();
                ret.FileExtension = "zip";
                ret.Result = true;
            }
            catch (Exception ex)
            {
                ret.Result = false;
                ret.LastError = ex.Message;
                ret.Stream = null;
                ret.FileExtension = string.Empty;
            }

            return ret;

        }
        /// <summary>
        /// Creazione di una lista di SDD
        /// </summary>
        /// <param name="BillingBatch"></param>
        /// <returns></returns>
        private static List<CBISDDReqLogMsg000006> CreateXmlWorker(long BillingBatch)
        {
            var CbiSddLog = new List<CBISDDReqLogMsg000006>();
            var SddList = GetSedaCbiSddTableRowList(BillingBatch);

            log.DebugFormat(string.Format("Trovati {0} SDD", SddList.Count));

            //Raggruppo per message Id. Per ogni valore verrà generato in file differente
            SddList.GroupBy(rid => rid.MsgId).ToList().ForEach(msg =>
                {
                    var FirstRid = msg.First();

                    #region Dichiarazioni e Inizializzazioni
                    var CbiLog = new CBISDDReqLogMsg000006() { GrpHdr = new CBIGroupHeader2(), PmtInf = new PaymentInstructionInformation2[1] };
                    var infPmt = new List<PaymentInstructionInformation2>();
                    List<DirectDebitTransactionInformation1> lstOfRid;

                    CbiLog.GrpHdr.InitgPty = new CBIPartyIdentification1() { Id = new CBIIdType1() };
                    CbiLog.GrpHdr.InitgPty.Id.OrgId = new CBIGenericIdentification1[1];
                    #endregion
                    #region GrpHdr
                    //Riempio la testata utilizzando i valori del primo SDD (tanto saranno uguali per tutti)
                    CbiLog.GrpHdr.MsgId = FirstRid.MsgId;
                    CbiLog.GrpHdr.CreDtTm = Convert.ToDateTime(FirstRid.CreDtTm);
                    CbiLog.GrpHdr.NbOfTxs = FirstRid.NbOfTxs.ToString();
                    CbiLog.GrpHdr.CtrlSum = FirstRid.CtrlSum;
                    CbiLog.GrpHdr.InitgPty.Id.OrgId[0] = new CBIGenericIdentification1 { Id = FirstRid.IdGrpHdr, Issr = FirstRid.IssrGrpHdr };
                    #endregion
                    #region PmtInf
                    //Raggruppo per SeqTp. In ogni file ci possono essere più tag padre PmtInf e la discriminante è il SeqTp
                    msg.GroupBy(x => x.SeqTp).ToList().ForEach(inf =>
                    {
                        var p = new PaymentInstructionInformation2();
                        var FirstInf = inf.First();

                        p.PmtTpInf = new CBIPaymentTypeInformation2_1();
                        p.Cdtr = new CBIPartyIdentification3();
                        p.PmtTpInf.SeqTp = new SequenceType1Code();
                        p.PmtTpInf.SvcLvl = new CBIServiceLevel3Choice();
                        p.PmtTpInf.SvcLvl.Cd = new CBIServiceLevel2Code();
                        p.PmtTpInf.LclInstrm = new LocalInstrument1Choice();
                        p.CdtrAcct = new CBICashAccount7();
                        p.CdtrAcct.Id = new AccountIdentification3Choice();
                        p.CdtrAgt = new CBIBranchAndFinancialInstitutionIdentification1();
                        p.CdtrAgt.FinInstnId = new CBIFinancialInstitutionIdentification1();
                        p.CdtrAgt.FinInstnId.ClrSysMmbId = new CBIClearingSystemMemberIdentification1();
                        p.CdtrSchmeId = new CBIPartyIdentification2();
                        p.CdtrSchmeId.Id = new CBIParty4Choice();
                        p.CdtrSchmeId.Id.PrvtId = new CBIPersonIdentification2();
                        p.CdtrSchmeId.Id.PrvtId.Othr = new CBIGenericIdentification2();

                        p.PmtInfId = FirstInf.PmtInfId;
                        p.PmtMtd = PaymentMethod2Code.DD;
                        p.PmtTpInf.SvcLvl.Cd = CBIServiceLevel2Code.SEPA;
                        p.PmtTpInf.LclInstrm.Cd = FirstInf.CdLclInstrm;
                        p.PmtTpInf.SeqTp = GetSeqTip(FirstInf.SeqTp);
                        p.ReqdColltnDt = Convert.ToDateTime(FirstInf.ReqdColltnDt);
                        p.Cdtr.Nm = FirstInf.NmCdtr;
                        p.CdtrAcct.Id.IBAN = FirstInf.IbanCdtrAcct;
                        p.CdtrAgt.FinInstnId.ClrSysMmbId.MmbId = FirstInf.MmbIdClrSysMmbId;
                        p.CdtrSchmeId.Id.PrvtId.Othr.Id = FirstInf.IdCdtrSchmeIdOthr;

                    #endregion
                       #region DrctDbtTxInf
                        //Per ogni SeqTp inserisco nel relativo PmtInf gli SDD.
                        lstOfRid = new List<DirectDebitTransactionInformation1>();
                        p.DrctDbtTxInf = new DirectDebitTransactionInformation1[msg.Count()];
                        msg.Where(seq => seq.SeqTp == FirstInf.SeqTp).ToList().ForEach(r =>
                        {
                            var d = new DirectDebitTransactionInformation1() { PmtId = new CBIPaymentIdentification1(), InstdAmt = new ActiveOrHistoricCurrencyAndAmount(), DrctDbtTx = new DirectDebitTransaction1() };
                            d.DrctDbtTx.MndtRltdInf = new MandateRelatedInformation1();
                            d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls = new AmendmentInformationDetails1();
                            d.Dbtr = new CBIPartyIdentification4();
                            d.Purp = new CBIPurpose1Choice();
                            d.DbtrAcct = new CBICashAccount7();
                            d.DbtrAcct.Id = new AccountIdentification3Choice();
                            d.RmtInf = new RemittanceInformation5();

                            d.PmtId.InstrId = r.InstrID.ToString();
                            d.PmtId.EndToEndId = r.EndToEndID.ToString();
                            d.InstdAmt.Ccy = "EUR";
                            d.InstdAmt.Value = r.InstdAmt;
                            d.DrctDbtTx.MndtRltdInf.MndtId = r.MndtId;
                            d.DrctDbtTx.MndtRltdInf.DtOfSgntr = Convert.ToDateTime(r.DtOfSgntr);

                            d.DrctDbtTx.MndtRltdInf.AmdmntInd = Convert.ToBoolean(r.AmdmntInd);
                            d.DrctDbtTx.MndtRltdInf.AmdmntIndSpecified = true;


                            if (!(r.OrgnlCdtrId == null || string.IsNullOrEmpty(r.OrgnlCdtrId)))
                            {
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId = new CBIPartyIdentification6();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId.Nm = r.OrgnlCdtrNm;

                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId.Id = new CBIParty4Choice();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId.Id.PrvtId = new CBIPersonIdentification2();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId.Id.PrvtId.Othr = new CBIGenericIdentification2();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlCdtrSchmeId.Id.PrvtId.Othr.Id = r.OrgnlCdtrId;
                            }
                            if (!(r.IBANORGNLDBTRACCT == null || string.IsNullOrEmpty(r.IBANORGNLDBTRACCT)))
                            {
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct = new CBICashAccount7();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct.Id = new AccountIdentification3Choice();
                                d.DrctDbtTx.MndtRltdInf.AmdmntInfDtls.OrgnlDbtrAcct.Id.IBAN = r.IBANORGNLDBTRACCT;
                            }


                            d.Dbtr.Nm = r.NmDbtr;
                            d.DbtrAcct.Id.IBAN = r.IbanDbtrAcct;
                            string[] desc = { r.RmtInfUstrd };

                            d.RmtInf.Ustrd = desc;
                            d.Purp.Cd = r.CdPurp;


                            lstOfRid.Add(d);
                        });
                        p.DrctDbtTxInf = lstOfRid.ToArray();
                        infPmt.Add(p);
                        #endregion
                    });
                    CbiLog.PmtInf = infPmt.ToArray();
                    CbiSddLog.Add(CbiLog);
                }

                );
            return CbiSddLog;
        }
        #endregion
        #region Util
        /// <summary>
        /// Codifica il tipo di sequenza
        /// </summary>
        /// <param name="SeqTip">Da string a enum</param>
        /// <returns></returns>
        private static SequenceType1Code GetSeqTip(string SeqTip)
        {
            if (SeqTip == "RCUR") { return SequenceType1Code.RCUR; }
            if (SeqTip == "FNAL") { return SequenceType1Code.FNAL; }
            if (SeqTip == "FRST") { return SequenceType1Code.FRST; }
            if (SeqTip == "OOFF") { return SequenceType1Code.OOFF; }
            return SequenceType1Code.RCUR;
        }
        private static List<SedaCbiSddTableRow> GetSedaCbiSddTableRowList(long BillingBatch)
        {
            using (Database db = new Database(SepaConnectionString, new OracleProvider()))
            {
                try
                {
                    log.DebugFormat(string.Format("select * from {0} WHERE BATCHFATTURAZIONE = @BATCHFATTURAZIONE", ViewName));
                    return db.Query<SedaCbiSddTableRow>(string.Format("select * from {0} WHERE BATCHFATTURAZIONE = @BATCHFATTURAZIONE", ViewName), new { BATCHFATTURAZIONE = BillingBatch }).ToList<SedaCbiSddTableRow>();
                    //return db.Query<SedaCbiSddTableRow>(string.Format("select * from {0}", ViewName)).ToList<SedaCbiSddTableRow>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("GetSepaObjectList Error retrive DB Data", ex);
                }
            }
        }
        private static MemoryStream GetFileGenerato(long Batch)
        {
            using (Database db = new Database(SepaConnectionString, new OracleProvider()))
            {
                try
                {
                    var res = db.Query<SedaCbiSddLottiGenerati>("select * from SDD_LOTTIGENERATI WHERE BATCHONLINEID = @BATCHONLINEID", new { BATCHONLINEID = Batch }).First();
                    return res.Stream;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("GetSepaObjectList Error retrive DB Data", ex);
                }
            }
        }
        private static MemoryStream CreateZipFile(List<string> XmlSdd)
        {
            log.DebugFormat("Creo zip");
            var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in XmlSdd)
                {
                    var file = archive.CreateEntry(item.Split('§')[0] + ".xml");
                    using (var entryStream = file.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(item.Split('§')[1]);
                    }
                }
            }
            return memoryStream;
        }
        private static void SaveZipStream(long BillingBatch, string FileBase64)
        {
            try
            {
                using (Database db = new Database(SepaConnectionString, new OracleProvider()))
                {
                    var sql = Sql.Builder.Append("UPDATE SDD_LOTTIGENERATI SET FILEGENERATO = @FILEGENERATO WHERE BATCHONLINEID = @BATCHONLINEID",
                    new { FILEGENERATO = FileBase64, BATCHONLINEID = BillingBatch });

                    Int32 _result = db.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static void SaveHistory()
        {
            try
            {
                using (Database db = new Database(SepaConnectionString, new OracleProvider()))
                {
                    var sql = Sql.Builder.Append("EXEC SDD_ARCHIVIAZIONE_STORICO");
                    db.EnableAutoSelect = false;
                    var result = db.Query<int>(sql); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }

}
