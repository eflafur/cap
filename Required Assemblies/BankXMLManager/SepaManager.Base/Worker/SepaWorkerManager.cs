using PetaPoco;
using SepaManager.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SepaManager.Base.Worker
{
    public static class SepaWorkerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ViewName
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SEPAViewName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["SEPAViewName"];
                else
                    throw new ApplicationException("SEPAViewName setting not found!");
            }
        }
        public static string ConnectionStringSepa
        {
            get
            {
                if (System.Configuration.ConfigurationManager.ConnectionStrings["sepaEntities"] != null)
                    return System.Configuration.ConfigurationManager.ConnectionStrings["sepaEntities"].ToString();
                else
                    throw new ApplicationException("sepaEntities connection string not found!");
            }
        }
        public static string ConnectionString
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionString"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["SEPAConnectionString"];
                else
                    throw new ApplicationException("SEPAConnectionString setting not found!");
            }
        }
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
        public static long CreateXml(string FileName, string PkOperazione, string Autore)
        {

            log.DebugFormat("GetSepaObjectList: FileName: {0} - PkOperazione: {1}", FileName, PkOperazione);

            List<SepaObject> SepaObjects = GetSepaObjectList(PkOperazione);

            if (SepaObjects == null)
            {
                log.ErrorFormat(string.Format("Sepa Object is null or empty!"), "Sepa Object");
                throw new ApplicationException("Sepa Object is null or empty!");
            }
            if (string.IsNullOrEmpty(FileName))
            {
                log.ErrorFormat(string.Format("FileName is null or empty!"), "FileName");
                throw new ApplicationException("FileName is null or empty!");
            }
            SepaHeader header = null;

            try
            {



                SepaObjects.ForEach(objSepa =>
                {
                    log.DebugFormat("new SepaHeader()  CompanyNumber(CFIVA): {0} - CompanyOrgId(OrgId): {1} - BankAccountNumber(IBANOrd): {2} - OrgId(CFIVA): {3} - Issr(CUC): {4} - InitiatingPartyName(RAGIONESOCIALE): {5}", objSepa.CFIVA, objSepa.OrgId, objSepa.IBANOrd, objSepa.CFIVA, objSepa.CUC, objSepa.RAGIONESOCIALE);

                    if (header == null)
                        header = new SepaHeader()
                        {
                            CompanyNumber = objSepa.CFIVA,
                            CompanyOrgId = objSepa.OrgId,
                            BankAccountNumber = objSepa.IBANOrd,
                            OrgId = objSepa.CFIVA,
                            Issr = objSepa.CUC,
                            InitiatingPartyName = objSepa.RAGIONESOCIALE,
                            Autore = Autore

                        };

                    if (header.SepaPaymentElements == null || header.SepaPaymentElements.Count == 0)
                    {
                        log.DebugFormat("new SepaPaymentElement() DebtorName(RAGIONESOCIALE): {0} - DebtorTaxID(CFIVA): {1} - DebtorABI(ABIOrd): {2} - DebtorIBAN(IBANOrd): {3}", objSepa.RAGIONESOCIALE, objSepa.CFIVA, objSepa.ABIOrd, objSepa.IBANOrd);
                        header.SepaPaymentElements.Add(new SepaPaymentElement()
                        {
                            RequestedExecutionDate = (Int64)objSepa.DataOp.ToOADate(),
                            DebtorName = objSepa.RAGIONESOCIALE,
                            DebtorPostalAddressAndCountry = "",
                            DebtorPostalAddressLines = "",
                            DebtorTaxID = objSepa.CFIVA,
                            DebtorABI = objSepa.ABIOrd,
                            DebtorIBAN = objSepa.IBANOrd,
                            DebtorBIC = "",
                            UltimateDebtorName = "",
                            UltimateDebtorTaxID = "",
                            PaymentInformationPaymentMethod = objSepa.PaymentInformationPaymentMethod,
                            PaymentInformationBatchBooking = objSepa.PaymentInformationBatchBooking

                        });
                    }


                    log.DebugFormat("new SepaCreditTransferTransaction() CreditorName(DescrDest): {0} - MmbId_ABIDest(ABIDest): {1} - CreditorTaxID(IdCliente): {2} - CreditorIBAN(IBANDest): {3} - Purpose(Descr50): {4} ", objSepa.DescrDest, objSepa.ABIDest, objSepa.IdCliente, objSepa.IBANDest, objSepa.Descr50);
                    header.SepaPaymentElements[0].SepaCreditTransferTransactions.Add(new SepaCreditTransferTransaction()
                    {
                        InstructedAmount = objSepa.Importo.ToString(),
                        CreditorBIC = string.Empty,
                        CreditorName = objSepa.DescrDest,
                        MmbId_ABIDest = objSepa.ABIDest,
                        CreditorPostalAddressCountryCode = "",
                        CreditorPostalAddress = "",
                        CreditorTaxID = objSepa.IdCliente,
                        CreditorIBAN = objSepa.IBANDest,
                        CreditorCF = objSepa.CODICE_FISCALE,
                        UltimateCreditorName = "",
                        UltimateCreditorTaxID = "",
                        Purpose = objSepa.Descr50,
                        ChequeInstructionChequeType = objSepa.ChequeInstructionChequeType,
                        PaymentTypeInformationServiceLevel = objSepa.PaymentTypeInformationServiceLevel,
                        PaymentTypeInformationCategoryPurpose = objSepa.PaymentTypeInformationCategoryPurpose,
                        CreditorPostalAddressPostalCode = objSepa.CAP,
                        CreditorPostalAddressStreet = objSepa.INDIRIZZO,
                        CreditorPostalAddressTownName = objSepa.CITTA,
                        CreditorPostalAddressProvince = objSepa.PROVINCIA,
                        PurposeCode = objSepa.PurposeCode,
                        PurposeProprietary = objSepa.PurposeProprietary,
                        RemittanceInformationUnstructured = objSepa.RemittanceInformationUnstructured

                    });
                });

            }
            catch (Exception ex)
            {
                log.ErrorFormat(string.Format("new SepaHeader() Exception : {0} ", ex.Message), "SepaHeader");
            }

            if (header == null)
                return -1;

            //string xmlPath = string.Format("sepa{0}.xml", DateTime.Now.ToOADate().ToString().Replace(",", "."));
            SepaXMLManager xmlManager = new SepaXMLManager();
            string retXml = null;
            retXml = xmlManager.CreateXMLInMemory(header);

            if (retXml == null)
                return -1;

            SepaXML sepaXml = new SepaXML()
            {
                State = 0,
                FileName = FileName,
                XmlFile = retXml,       //Convert.ToBase64String(retXml.ToByteArray()),
                Created = DateTime.Now
            };

            return SaveData(header, sepaXml) > 0 ? header.ID : -1;
        }
        private static List<SepaObject> GetSepaObjectList(string PkOperazione)
        {
            log.DebugFormat("GetSepaObjectList: ConnectionString: {0} ", ConnectionString);
            using (Database db = new Database(ConnectionString, new OracleProvider()))
            {
                try
                {
                    log.DebugFormat(string.Format("db.Query<SepaObject> : select * from {0} where pkOperazione = {1}", ViewName, PkOperazione));
                    return db.Query<SepaObject>(string.Format("select * from {0} where pkOperazione = @parPKOperazione", ViewName), new { parPKOperazione = PkOperazione }).ToList<SepaObject>();
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("GetSepaObjectList Error retrive DB Data: {0} ", ex.ToString());
                    throw new ApplicationException("GetSepaObjectList Error retrive DB Data", ex);
                }
            }
        }
        private static long SaveData(SepaHeader header, SepaXML xml)
        {
            using (Database db = new Database(ConnectionStringSepa, new OracleProvider()))
            {
                try
                {
                    db.BeginTransaction();

                    header.Created = DateTime.Now;
                    header.State = 0;
                    db.Save(header);

                    header.SepaPaymentElements.ForEach(payment =>
                    {
                        payment.IDHeader = header.ID;
                        payment.Created = DateTime.Now;
                        db.Save(payment);
                        payment.SepaCreditTransferTransactions.ForEach(tran =>
                        {
                            tran.IDPayment = payment.ID;
                            tran.Created = DateTime.Now;
                            db.Save(tran);
                        });
                    });
                    xml.SepaHeaderID = header.ID;
                    db.Save(xml);

                    db.CompleteTransaction();
                    return (long)header.ID;
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("GetSepaObjectList Error SaveData: {0} ", ex.Message);
                    db.AbortTransaction();
                    return -1;
                }
            }
        }

        public static List<String> CreateCsvDocument(long pkOperazione)
        {
            using (Database db = new Database(ConnectionString, new OracleProvider()))
            {
                SepaHeader transaction = db.Query<SepaHeader>("select * from SEPAHEADER where id=@parP0", new { parP0 = pkOperazione }).FirstOrDefault();
                if (transaction == null)
                    throw new ArgumentException(string.Format("Impossibile recuperare alcuna transazione per l'ID:{0}", pkOperazione), "pkOperazione");
                SepaPaymentElement transactionContainer = db.Query<SepaPaymentElement>("select * from SEPAPAYMENTELEMENTS where IDHEADER=@fkOperazione", new { fkOperazione = pkOperazione }).FirstOrDefault();
                IEnumerable<SepaCreditTransferTransaction> transactionItems = db.Query<SepaCreditTransferTransaction>("select * from SEPACREDITTRANSFERTRANSACTION where IDPAYMENT=@fkPayment and eliminato_da is null", new { fkPayment = transactionContainer.ID });

                List<String> strCsv = new List<String>();
                List<String> completeCsv = new List<String>();

                strCsv.Add("CodiceCliente");
                strCsv.Add("Beneficiario");
                strCsv.Add("IBAN");
                strCsv.Add("TipoPagamento");
                strCsv.Add("Importo");
                strCsv.Add("Indirizzo");
                strCsv.Add("CAP");
                strCsv.Add("Localita");

                String csvString = String.Join(";", strCsv);
                completeCsv.Add(csvString);
                strCsv.Clear();

                foreach (var item in transactionItems)
                {
                    strCsv.Add(item.CreditorTaxID != null ? item.CreditorTaxID : "");
                    strCsv.Add(item.CreditorName != null ? item.CreditorName : "");
                    strCsv.Add(item.CreditorIBAN != null ? item.CreditorIBAN : "");
                    strCsv.Add(item.PaymentTypeInformationCategoryPurpose != null ? item.PaymentTypeInformationCategoryPurpose : "");
                    strCsv.Add(item.InstructedAmount != null ? item.InstructedAmount : "");
                    strCsv.Add(item.CreditorPostalAddressStreet != null ? item.CreditorPostalAddressStreet : "");
                    strCsv.Add(item.CreditorPostalAddressPostalCode != null ? item.CreditorPostalAddressPostalCode : "");
                    strCsv.Add(item.CreditorPostalAddressTownName != null ? item.CreditorPostalAddressTownName : "");

                    csvString = String.Join(";", strCsv);
                    completeCsv.Add(csvString);
                    strCsv.Clear();
                }
                return completeCsv;
            }
        }

        public static BankXMLOutput RegenerateXmlDocument(long pkOperazione)
        {

            SepaXMLManager xmlManager = new SepaXMLManager();
            string retXml = null;
            using (Database db = new Database(ConnectionString, new OracleProvider()))
            {
                SepaHeader transaction = db.Query<SepaHeader>("select * from SEPAHEADER where id=@parP0", new { parP0 = pkOperazione }).FirstOrDefault();
                if (transaction == null)
                    throw new ArgumentException(string.Format("Impossibile recuperare alcuna transazione per l'ID:{0}", pkOperazione), "pkOperazione");
                SepaPaymentElement transactionContainer = db.Query<SepaPaymentElement>("select * from SEPAPAYMENTELEMENTS where IDHEADER=@fkOperazione", new { fkOperazione = pkOperazione }).FirstOrDefault();
                IEnumerable<SepaCreditTransferTransaction> transactionItems = db.Query<SepaCreditTransferTransaction>("select * from SEPACREDITTRANSFERTRANSACTION where IDPAYMENT=@fkPayment and eliminato_da is null", new { fkPayment = transactionContainer.ID });

                transactionContainer.SepaCreditTransferTransactions.AddRange(transactionItems);
                transaction.SepaPaymentElements.Add(transactionContainer);
                retXml = xmlManager.CreateXMLInMemory(transaction);
                try
                {
                    if (transactionContainer.SepaCreditTransferTransactions.Count > 0)
                    {
                        SepaXML originalTransaction = db.Query<SepaXML>("select * from sepaXml where SEPAHEADERID=@fkOperazione", new { fkOperazione = pkOperazione }).FirstOrDefault();

                        db.BeginTransaction();
                        SepaXML newTransaction = new SepaXML()
                        {

                            Created = DateTime.Now,
                            FileName = originalTransaction.FileName,
                            SepaHeaderID = pkOperazione,
                            State = 0,
                            XmlFile = retXml
                        };
                        db.Save(newTransaction);
                    }

                    IEnumerable<SepaXML> _ret;
                    _ret = db.Query<SepaXML>("select VIEWNAME, XMLFILE FROM (select VIEWNAME, XMLFILE from SEPAXML where SEPAHEADERID = @parSEPAHEADERID order by ID desc) where ROWNUM <= 1", new { parSEPAHEADERID = pkOperazione });

                    if (_ret != null)
                        if (_ret.Count() > 0)
                            if (_ret.FirstOrDefault() != null)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(_ret.FirstOrDefault().XmlFile);
                                BankXMLOutput output = new BankXMLOutput(_ret.FirstOrDefault().FileName, _ret.FirstOrDefault().XmlFile, doc.DocumentElement);
                                db.CompleteTransaction();
                                return output;
                            }
                    db.CompleteTransaction();
                    return null;

                }
                catch (Exception ex)
                {
                    db.AbortTransaction();
                    throw new ApplicationException("RegenerateXmlDocument Error:", ex);
                }

            }

        }
        //[Obsolete]
        //public static string GetXmlDocument(long SepaHederID)
        //{
        //    string xml = null;
        //    using (Database db = new Database(ConnectionString, ConnectionProvider))
        //    {
        //        try
        //        {
        //            xml = db.ExecuteScalar<string>(string.Format("select XMLFILE FROM (select XMLFILE from SEPAXML where SEPAHEADERID = @parSEPAHEADERID order by ID desc) where ROWNUM <= 1"), new { parSEPAHEADERID = SepaHederID });
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ApplicationException("XMLFILE Error retrive DB Data", ex);
        //        }
        //        if (!string.IsNullOrEmpty(xml))
        //        {
        //            return xml;
        //        }
        //        else
        //            return null;
        //    }
        //}

        public static BankXMLOutput GetXmlDocument(long SepaHederID)
        {
            using (Database db = new Database(ConnectionString, new OracleProvider()))
            {
                IEnumerable<SepaXML> _ret;
                try
                {
                    _ret = db.Query<SepaXML>("select VIEWNAME, XMLFILE FROM (select VIEWNAME, XMLFILE from SEPAXML where SEPAHEADERID = @parSEPAHEADERID order by ID desc) where ROWNUM <= 1", new { parSEPAHEADERID = SepaHederID });
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("XMLFILE Error retrive DB Data", ex);
                }
                if (_ret != null)
                    if (_ret.Count() > 0)
                        if (_ret.FirstOrDefault() != null)
                        {
                            XmlDocument doc = new XmlDocument();
                            //string _retVal = SepaWorkerManager.GetXmlDocument(SepaHederID);
                            doc.LoadXml(_ret.FirstOrDefault().XmlFile);
                            return new BankXMLOutput(_ret.FirstOrDefault().FileName, _ret.FirstOrDefault().XmlFile, doc.DocumentElement);
                        }
                return null;
            }
        }







        //    #region OLD Methods
        //    public static long CreateXml(string connString, string viewName, long id)
        //    {
        //        SQLDBAccess sql = new SQLDBAccess();
        //        SQLDBAccess.ExternalConnectionString = connString;

        //        DataSet set = sql.ExecuteQuery("select * from " + viewName + " where pkOperazione = " + id.ToString());

        //        SepaMessage message = null;
        //        SepaPaymentElement payment = null;

        //        foreach (DataRow row in set.Tables[0].Rows)
        //        {
        //            //string ci = "0333067M";
        //            //"IT44ZZZ0000001234567890";

        //            if (message == null)
        //                message = new SepaMessage(row["CFIVA"].ToString(), row["OrgId"].ToString(), row["IBANOrd"].ToString(), row["CFIVA"].ToString(), row["CUC"].ToString(), row["RAGIONESOCIALE"].ToString());

        //            if (payment == null)
        //            {
        //                DateTime reqDate = DateTime.Now;
        //                DateTime.TryParse(row["DataOp"].ToString(), out reqDate);
        //                payment = message.AddPayment(reqDate, row["RAGIONESOCIALE"].ToString(), "", "",
        //                    row["CFIVA"].ToString(), row["ABIOrd"].ToString(), row["IBANOrd"].ToString(), "", "", "");
        //            }

        //            payment.AddTransaction(row["Importo"].ToString(), "GEBA BE BB", row["DescrDest"].ToString(), row["ABIDest"].ToString(),
        //                "", "", "", row["IBANDest"].ToString(),
        //              "", "", row["Descr50"].ToString());
        //        }

        //        if (message == null)
        //            return -1;

        //        long messageId = 0;
        //        if (!message.Store(ref messageId) || messageId <= 0)
        //            return -1;

        //        string xmlPath = "sepa" + DateTime.Now.ToOADate().ToString().Replace(",", ".") + ".xml";
        //        if (!SepaMessage.CreateXML(messageId, xmlPath))
        //            return -1;

        //        sepaEntities ctx = new sepaEntities();
        //        SepaXML sepaXml = new SepaXML();
        //        sepaXml.State = 0;
        //        sepaXml.ViewID = (decimal)id;
        //        sepaXml.FileName = viewName;
        //        sepaXml.XmlFile = Convert.ToBase64String(System.IO.File.ReadAllBytes(xmlPath));
        //        sepaXml.Created = DateTime.Now;
        //        ctx.AddToSepaXML(sepaXml);

        //        int iRes = ctx.SaveChanges();
        //        return iRes > 0 ? id : -1;
        //    }

        //    static string _errorMessageXml = string.Empty;
        //    public static string ErrorMessageXml
        //    {
        //        get
        //        {
        //            return _errorMessageXml;
        //        }
        //    }

        //    public static void ClearErrorMessage()
        //    {
        //        _errorMessageXml = string.Empty;
        //        //SQLDBAccess.ClearErrorMessage();
        //    }

        //    public static bool CreateXml(string connString, string viewName, long id, string xmlPath)
        //    {
        //        try
        //        {

        //            String SelectCmdString = "select * from " + viewName + " where pkOperazione = " + id.ToString();
        //            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(SelectCmdString, connString);
        //            DataSet set = new DataSet();
        //            mySqlDataAdapter.Fill(set, "Bonifico");

        //            //SQLDBAccess sql = new SQLDBAccess();
        //            //SQLDBAccess.ExternalConnectionString = connString;

        //            //DataSet set = sql.ExecuteQuery("select * from " + viewName + " where pkOperazione = " + id.ToString());
        //            if (set == null || set.Tables.Count <= 0)
        //            {
        //                _errorMessageXml += "ExecuteQuery failed. (" + SQLDBAccess.ErrorMessage + ")";
        //                return false;
        //            }

        //            if (set.Tables[0].Rows.Count <= 0)
        //            {
        //                _errorMessageXml += "No data found for id " + id.ToString();
        //                return false;
        //            }

        //            SepaMessage message = null; SepaPaymentElement payment = null;
        //            foreach (DataRow row in set.Tables[0].Rows)
        //            {
        //                if (message == null)
        //                    message = new SepaMessage(row["CFIVA"].ToString(), row["OrgId"].ToString(), row["IBANOrd"].ToString(), row["CFIVA"].ToString(), row["CUC"].ToString(), row["RAGIONESOCIALE"].ToString());

        //                if (payment == null)
        //                {
        //                    DateTime reqDate = DateTime.Now;
        //                    DateTime.TryParse(row["DataOp"].ToString(), out reqDate);
        //                    payment = message.AddPayment(reqDate, row["RAGIONESOCIALE"].ToString(), "", "",
        //                        row["CFIVA"].ToString(), row["ABIOrd"].ToString(), row["IBANOrd"].ToString(), "", "", "");
        //                }

        //                payment.AddTransaction(row["Importo"].ToString(), "GEBA BE BB", row["DescrDest"].ToString(), row["ABIDest"].ToString(),
        //                    "", "", "", row["IBANDest"].ToString(),
        //                  "", "", row["Descr50"].ToString());
        //            }

        //            if (message == null)
        //            {
        //                _errorMessageXml += "SepaMessage object creation has failed. (" + id.ToString() + ")";
        //                return false;
        //            }

        //            long messageId = 0;
        //            if (!message.Store(ref messageId) || messageId <= 0)
        //            {
        //                _errorMessageXml += "SepaMessage store has failed. (" + id.ToString() + ")";
        //                _errorMessageXml += "\r\n" + SQLDBAccess.ErrorMessage;
        //                return false;
        //            }

        //            if (!SepaMessage.CreateXML(messageId, xmlPath))
        //            {
        //                _errorMessageXml += "SepaMessage XML creation has failed. (" + id.ToString() + ")";
        //                _errorMessageXml += "\r\n" + SQLDBAccess.ErrorMessage;
        //                return false;
        //            }

        //            sepaEntities ctx = new sepaEntities();
        //            SepaXML sepaXml = new SepaXML();
        //            sepaXml.State = 0;
        //            sepaXml.ViewID = (decimal)id;
        //            sepaXml.FileName = viewName;
        //            sepaXml.XmlFile = Convert.ToBase64String(System.IO.File.ReadAllBytes(xmlPath));
        //            sepaXml.Created = DateTime.Now;
        //            ctx.AddToSepaXML(sepaXml);

        //            int iRes = ctx.SaveChanges();
        //            return iRes > 0 ? true : false;
        //        }
        //        catch (Exception ex)
        //        {
        //            _errorMessageXml += "CreateXml::Error(" + id.ToString() + ")" + ":" + ex.ToString();
        //            return false;
        //        }
        //    }

        //    /// <summary>
        //    /// Utilizzato solo in TEST!
        //    /// </summary>
        //    /// <param name="connString"></param>
        //    /// <param name="viewName"></param>
        //    /// <param name="id"></param>
        //    /// <param name="xmlPath"></param>
        //    /// <returns></returns>
        //    public static bool CreateXmlModo2(string connString, string viewName, long id, string xmlPath)
        //    {
        //        try
        //        {
        //            SQLDBAccess sql = new SQLDBAccess();
        //            SQLDBAccess.ExternalConnectionString = connString;

        //            DataSet set = sql.ExecuteQuery("select * from " + viewName + " where pkOperazione = " + id.ToString());
        //            if (set == null || set.Tables.Count <= 0)
        //            {
        //                _errorMessageXml += "ExecuteQuery failed. (" + SQLDBAccess.ErrorMessage + ")";
        //                return false;
        //            }

        //            if (set.Tables[0].Rows.Count <= 0)
        //            {
        //                _errorMessageXml += "No data found for id " + id.ToString();
        //                return false;
        //            }

        //            SepaMessage message = null; SepaPaymentElement payment = null;
        //            foreach (DataRow row in set.Tables[0].Rows)
        //            {
        //                if (message == null)
        //                    message = new SepaMessage(row["CFIVA"].ToString(), row["OrgId"].ToString(), row["IBANOrd"].ToString(), row["CFIVA"].ToString(), row["CUC"].ToString(), row["RAGIONESOCIALE"].ToString());

        //                if (payment == null)
        //                {
        //                    DateTime reqDate = DateTime.Now;
        //                    DateTime.TryParse(row["DataOp"].ToString(), out reqDate);
        //                    payment = message.AddPayment(reqDate, row["RAGIONESOCIALE"].ToString(), "", "",
        //                        row["CFIVA"].ToString(), row["ABIOrd"].ToString(), row["IBANOrd"].ToString(), "", "", "");
        //                }

        //                payment.AddTransaction(row["Importo"].ToString(), "GEBA BE BB", row["DescrDest"].ToString(), row["ABIDest"].ToString(),
        //                    "", "", "", row["IBANDest"].ToString(),
        //                  "", "", row["Descr50"].ToString());
        //            }

        //            if (message == null)
        //            {
        //                _errorMessageXml += "SepaMessage object creation has failed. (" + id.ToString() + ")";
        //                return false;
        //            }

        //            long messageId = 0;
        //            if (!message.Store(ref messageId) || messageId <= 0)
        //            {
        //                _errorMessageXml += "SepaMessage store has failed. (" + id.ToString() + ")";
        //                _errorMessageXml += "\r\n" + SQLDBAccess.ErrorMessage;
        //                return false;
        //            }

        //            if (!SepaMessage.CreateXMLModo2(messageId, xmlPath))
        //            {
        //                _errorMessageXml += "SepaMessage XML creation has failed. (" + id.ToString() + ")";
        //                _errorMessageXml += "\r\n" + SQLDBAccess.ErrorMessage;
        //                return false;
        //            }

        //            sepaEntities ctx = new sepaEntities();
        //            SepaXML sepaXml = new SepaXML();
        //            sepaXml.State = 0;
        //            sepaXml.ViewID = (decimal)id;
        //            sepaXml.FileName = viewName;
        //            sepaXml.XmlFile = Convert.ToBase64String(System.IO.File.ReadAllBytes(xmlPath));
        //            sepaXml.Created = DateTime.Now;
        //            ctx.AddToSepaXML(sepaXml);

        //            int iRes = ctx.SaveChanges();
        //            return iRes > 0 ? true : false;
        //        }
        //        catch (Exception ex)
        //        {
        //            _errorMessageXml += "CreateXml::Error(" + id.ToString() + ")" + ":" + ex.ToString();
        //            return false;
        //        }
        //    }

        //    #endregion
    }
}
