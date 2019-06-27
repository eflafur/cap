using SepaManager.Base;
using SepaManager.Base.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace SepaManager.Base.Worker
{


    public class SepaXMLManager : IDisposable
    {
        public MemoryStream sbXml { get; private set; }
        private XmlWriter writer = null;
        static long id = 0;
        static long instrID = 0;
        private const string XmlSepaCBIBdyNamespace = "urn:CBI:xsd:CBIBdyPaymentRequest.00.04.00";
        private const string XmlSepaCBIPaymentNamespace = "urn:CBI:xsd:CBIPaymentRequest.00.04.00";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string msgId = string.Empty;
        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        public string CreateXMLInMemory(SepaHeader header)
        {
            if (!CreateMessageInMemory(header))
                return null;

            return Encoding.UTF8.GetString(sbXml.ToArray());
        }

        protected bool CreateMessageInMemory(SepaHeader message)
        {
           

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                sbXml = new MemoryStream();

                settings.CloseOutput = true;
                settings.Indent = true;
                settings.IndentChars = "\t";
                settings.OmitXmlDeclaration = false;
                settings.Encoding = new UTF8Encoding(false);
                settings.ConformanceLevel = ConformanceLevel.Document;

                writer = XmlWriter.Create(sbXml, settings);

#if NEWVERSION
                //writer.WriteStartElement("Document", "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03");
                writer.WriteStartElement("Document", "urn:CBI:xsd:CBIPaymentRequest.00.03.09");              
                                  
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                //writer.WriteAttributeString("xsi", "schemaLocation", null, "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03 pain.001.001.03");
                writer.WriteAttributeString("xsi", "schemaLocation", null, "urn:CBI:xsd:CBIPaymentRequest.00.03.09 CBIPaymentRequest.00.03.09");
#else
                //writer.WriteStartElement("Document", "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03");
                //writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                //writer.WriteAttributeString("xsi", "schemaLocation", null, "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03 pain.001.001.03");
#endif
                //<Document xsi:schemaLocation="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03 pain.001.001.03" xmlns="urn:iso:std:iso:20022:tech:xsd:pain.001.001.03" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">                                                                


                writer.WriteStartElement("ns2","CBIBdyPaymentRequest", XmlSepaCBIBdyNamespace);

                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns",  null, XmlSepaCBIPaymentNamespace);


                string prefix = writer.LookupPrefix(XmlSepaCBIBdyNamespace);

                //Questa funzione non aveva previsto di creare piu tipi di file (PaymentRequest)
                //Per mantenere una compatibilità ho suddivo in 2 diversi oggetti l'oggetto generico [message] 



                List <SepaCreditTransferTransaction> _sepaCreditTransferTransactions =message.SepaPaymentElements
                                                            .FirstOrDefault()
                                                            .SepaCreditTransferTransactions.ToList();



                SepaHeader messageForTRAPayment = DeepClone<SepaHeader>(message);
                //messageForTRAPayment = message;
                messageForTRAPayment.SepaPaymentElements
                    .FirstOrDefault()
                    .SepaCreditTransferTransactions = _sepaCreditTransferTransactions
                                                            .Where(w => w.IsAssegno.Equals(false))
                                                            .ToList();


                if (messageForTRAPayment.SepaPaymentElements.FirstOrDefault().SepaCreditTransferTransactions.Any())

                    foreach (SepaPaymentElement _sepaTRAPaymentElement in messageForTRAPayment.SepaPaymentElements)
                    {

                        
                        writer.WriteStartElement(prefix, "CBIEnvelPaymentRequest", XmlSepaCBIBdyNamespace);
                        writer.WriteStartElement(prefix, "CBIPaymentRequest", XmlSepaCBIBdyNamespace);


                        if (!CreateGroupHeader(messageForTRAPayment))
                            return false;
                        if (!CreatePaymentInformationElements(messageForTRAPayment))
                            return false;
                        writer.WriteEndElement();//CBIPaymentRequest
                        writer.WriteEndElement();//CBIEnvelPaymentRequest


                    }


                SepaHeader messageForCHKPayment = DeepClone<SepaHeader>(message);
               
                messageForCHKPayment.SepaPaymentElements
                    .FirstOrDefault()
                    .SepaCreditTransferTransactions = _sepaCreditTransferTransactions
                                                              .Where(w => w.IsAssegno)
                                                              .ToList();

                                if (messageForCHKPayment.SepaPaymentElements.FirstOrDefault().SepaCreditTransferTransactions.Any())

                    foreach (SepaPaymentElement _sepaCHKPaymentElement in messageForCHKPayment.SepaPaymentElements)
                    {

                        writer.WriteStartElement(prefix, "CBIEnvelPaymentRequest", XmlSepaCBIBdyNamespace);
                        writer.WriteStartElement(prefix, "CBIPaymentRequest", XmlSepaCBIBdyNamespace);


                        if (!CreateGroupHeader(messageForCHKPayment))
                            return false;
                        if (!CreatePaymentInformationElements(messageForCHKPayment))
                            return false;
                        writer.WriteEndElement();//CBIPaymentRequest
                        writer.WriteEndElement();//CBIEnvelPaymentRequest


                    }











                writer.WriteEndElement();//CBIBdyPaymentRequest

                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                writer.Close();
                return false;
            }
            return true;
        }


        /// <summary>
        /// per adesso cosi, poi è da spostare sul db;
        /// </summary>
        /// <returns></returns>
        protected string GetNextIDValue()
        {
            return (++id).ToString();
        }

        /// <summary>
        /// per adesso cosi, poi è da spostare sul db;
        /// </summary>
        /// <returns></returns>
        protected string GetNextInstrIDValue()
        {
            return (++instrID).ToString();
        }

        protected bool CreateGroupHeader(SepaHeader message)
        {
            writer.WriteStartElement("GrpHdr");

            //writer.WriteAttributeString("xmlns", null,  XmlSepaCBIPaymentNamespace);

            try
            {


                log.DebugFormat("CreateGroupHeader msgId {0}/{1}/{2}", GetNextIDValue(), message.CompanyNumber, DateTime.Now.ToString("yyyyMMddHHmmss"));


                msgId = string.Format("{0}/{1}/{2}", GetNextIDValue(), message.CompanyNumber, DateTime.Now.ToString("yyyyMMddHHmmss")); //+ nextNumber;
                if (msgId.Length > 35)
                    msgId = msgId.Substring(0, 35);

                writer.WriteStartElement("MsgId");
                writer.WriteValue(msgId);
                writer.WriteEndElement();

                writer.WriteStartElement("CreDtTm");
                string dateValue = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                dateValue = dateValue.Replace(".", ":");
                writer.WriteValue(dateValue);
                writer.WriteEndElement();

                log.DebugFormat("CreateGroupHeader NbOfTxs {0}", message.GetTransactionsNo().ToString());

                writer.WriteStartElement("NbOfTxs");
                writer.WriteValue(message.GetTransactionsNo().ToString());
                writer.WriteEndElement();

                log.DebugFormat("CreateGroupHeader CtrlSum {0}", String.Format("{0:0.00}", message.GetTotalAmount()).Replace(",", "."));

                writer.WriteStartElement("CtrlSum");
                writer.WriteValue(String.Format("{0:0.00}", message.GetTotalAmount()).Replace(",", "."));
                writer.WriteEndElement();

                log.DebugFormat("CreateGroupHeader InitgPty Nm {0}", message.InitiatingPartyName);

                writer.WriteStartElement("InitgPty");

                writer.WriteStartElement("Nm");
                writer.WriteValue(message.InitiatingPartyName);
                writer.WriteEndElement();

                CreateOrgId(message);

                writer.WriteEndElement(); //InitgPty

                writer.WriteEndElement();
                return true;

            }
            catch (Exception ex)
            {
                log.ErrorFormat(string.Format("CreateGroupHeader Exception : {0} ", ex.Message), "SepaHeader");
                return false;
            }
        }


        protected bool CreateOrgId(SepaHeader message)
        {
            /*<Id>
          <OrgId>
            <Othr>
              <Id>0468651441</Id>
              <Issr>KBO-BCE</Issr>
            </Othr>
          </OrgId>
        </Id>*/

            if (!string.IsNullOrEmpty(message.OrgId) && !string.IsNullOrEmpty(message.Issr))
            {
                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");

                bool firstTime = false;
                if (firstTime)
                {
                    writer.WriteStartElement("Id");
                    writer.WriteValue(message.Issr);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Issr");
                    writer.WriteValue("CBI");
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement("Id");
                    writer.WriteValue(message.CompanyOrgId /*corretto il 17/06/2014*/);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Issr");
                    writer.WriteValue("CBI");
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            return true;
        }

        protected bool CreatePaymentInformationElements(SepaHeader message)
        {

            try
            {


                foreach (SepaPaymentElement element in message.SepaPaymentElements)
                {
                    /*
                     * <PmtInf>
                          <PmtInfId> ABC/4560/2008-09-25</PmtInfId>
                          <PmtMtd>TRF</PmtMtd>
                          <BtchBookg>false</BtchBookg>*/



                    writer.WriteStartElement("PmtInf");

                    //writer.WriteAttributeString("xmlns", null,  XmlSepaCBIPaymentNamespace);

                    writer.WriteStartElement("PmtInfId");
                    //string msgId = string.Format("{0}/{1}/{2}", message.CompanyNumber, message.BankAccountNumber, DateTime.Now.ToOADate().ToString().Replace(",", ".")); //+ nextNumber;
                    //if (msgId.Length > 35)
                    //    msgId = msgId.Substring(0, 35);

                    log.DebugFormat("CreatePaymentInformationElements PmtInfId {0}", msgId);

                    writer.WriteValue(msgId);
                    writer.WriteEndElement();

                    string PaymentInformationPaymentMethod = "TRA";

                    if (element.SepaCreditTransferTransactions.FirstOrDefault().IsAssegno)
                        PaymentInformationPaymentMethod = "CHK";

                    if (element.IsBonificoDomiciliato)
                        PaymentInformationPaymentMethod = "CIR";

                    writer.WriteStartElement("PmtMtd");
                    writer.WriteValue(PaymentInformationPaymentMethod);
                    writer.WriteEndElement();

                    if (!string.IsNullOrEmpty(element.PaymentInformationBatchBooking))
                    {
                        writer.WriteStartElement("BtchBookg");
                        writer.WriteValue("true");
                        writer.WriteEndElement();
                    }                  

                    if (!element.SepaCreditTransferTransactions.FirstOrDefault().IsAssegno && !element.IsBonificoDomiciliato)
                        CreatePmtTpInf();

                    writer.WriteStartElement("ReqdExctnDt");
                    DateTime date = DateTime.FromOADate(element.RequestedExecutionDate);
                    writer.WriteValue(date.ToString("yyyy-MM-dd"));
                    writer.WriteEndElement();

                    CreateDebitor(element);
                    CreateDebtorAccount(element);
                    CreateDebtorAgent(element);

                    CreateUltimateDebtor(element);
                    CreateChargeBearer();

                    foreach (SepaCreditTransferTransaction transaction in element.SepaCreditTransferTransactions)
                        CreateTransferTransactionInformation(transaction, message, PaymentInformationPaymentMethod, element.DebtorTaxID);

                    writer.WriteEndElement();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat(string.Format("CreatePaymentInformationElements Exception : {0} ", ex.Message), "SepaHeader");
                return false;
            }
        }

        protected bool CreateTransferTransactionInformation(SepaCreditTransferTransaction transaction, SepaHeader header, string paymentInfoMethod, string DebtorTaxID)
        {

            try
            {


                //InstrId: bankAccountNumber/id/date
                //EndToEndId: bankAccountNumber/id/date
                //<CdtTrfTxInf><PmtId>
                writer.WriteStartElement("CdtTrfTxInf");
                writer.WriteStartElement("PmtId");

                string id = GetNextInstrIDValue();
                string instrIDValue = string.Format("{0}/{1}/{2:00}{3:00}{4:00}", header.BankAccountNumber, id, DateTime.Now.Date.Day, DateTime.Now.Date.Month, DateTime.Now.Date.Year);
                if (instrIDValue.Length > 35)
                    instrIDValue = instrIDValue.Substring(0, 35);

                string value = transaction.Purpose ?? "";
                if (value.Length > 35)
                    value = value.Substring(0, 35);

                writer.WriteStartElement("InstrId");
                writer.WriteValue(value);
                writer.WriteEndElement();

                writer.WriteStartElement("EndToEndId");
                writer.WriteValue(value);
                writer.WriteEndElement(); //EndToEndId

                writer.WriteEndElement(); //PmtId


                /*    <PmtTpInf>
                  <SvcLvl>
                    <Cd>SEPA</Cd>
                  </SvcLvl>
                </PmtTpInf>
                    */

                

                if (!string.IsNullOrEmpty(transaction.PaymentTypeInformationServiceLevel))
                {
                    writer.WriteStartElement("PmtTpInf");
                    writer.WriteStartElement("SvcLvl");
                    writer.WriteStartElement("Cd");
                    writer.WriteValue(transaction.PaymentTypeInformationServiceLevel);
                    writer.WriteEndElement(); //Cd
                    writer.WriteEndElement(); //SvcLvl
                    writer.WriteEndElement(); //PmtTpInf
                }
                if (!string.IsNullOrEmpty(transaction.PaymentTypeInformationCategoryPurpose))
                {
                    writer.WriteStartElement("PmtTpInf");
                    writer.WriteStartElement("CtgyPurp");
                    writer.WriteStartElement("Cd");
                    writer.WriteValue(transaction.PaymentTypeInformationCategoryPurpose);//verificare presenza del codice(SUPP o CASH)
                    writer.WriteEndElement(); //Cd
                    writer.WriteEndElement(); //SvcLvl
                    writer.WriteEndElement(); //PmtTpInf
                }


               

                /*
                 * <Amt>
                    <InstdAmt Ccy="EUR">535.25</InstdAmt>
                   </Amt>
                 * */

                writer.WriteStartElement("Amt");
                writer.WriteStartElement("InstdAmt");
                writer.WriteAttributeString("Ccy", "EUR");
                writer.WriteValue(transaction.InstructedAmount.Replace(",", "."));
                writer.WriteEndElement(); //InstdAmt
                writer.WriteEndElement(); //Amt


                if (!string.IsNullOrEmpty(transaction.ChequeInstructionChequeType))
                {
                    writer.WriteStartElement("ChqInstr");
                    writer.WriteStartElement("ChqTp");
                    writer.WriteValue(transaction.ChequeInstructionChequeType);
                    writer.WriteEndElement(); //ChqInstr
                    writer.WriteEndElement(); //ChqTp
                }

                /*
                 * <CdtrAgt>
              <FinInstnId>
                <BIC>CRBABE22</BIC>
              </FinInstnId>
            </CdtrAgt>*/
                //if (!string.IsNullOrEmpty(transaction.MmbId_ABIDest))
                //{
                //    writer.WriteStartElement("CdtrAgt");
                //    writer.WriteStartElement("FinInstnId");
                //    writer.WriteStartElement("ClrSysMmbId");
                //    writer.WriteStartElement("MmbId");

                //    writer.WriteValue(transaction.MmbId_ABIDest/*"ITNCC0123456"*/); //abi

                //    writer.WriteEndElement(); //ClrSysMmbId
                //    writer.WriteEndElement(); //MmbId

                //    /*writer.WriteStartElement("BIC");
                //    writer.WriteValue(transaction.CreditorBIC);
                //    writer.WriteEndElement(); //BIC*/
                //    writer.WriteEndElement(); //FinInstnId
                //    writer.WriteEndElement(); //CdtrAgt
                //}
                if (!string.IsNullOrEmpty(transaction.CreditorBIC))
                {
                    writer.WriteStartElement("CdtrAgt");
                    writer.WriteStartElement("FinInstnId");
                    writer.WriteStartElement("BIC");

                    writer.WriteValue(transaction.CreditorBIC/*"ITNCC0123456"*/); //abi

                    writer.WriteEndElement(); //BIC
                    writer.WriteEndElement(); //FinInstnId
                    writer.WriteEndElement(); //CdtrAgt
                }

                CreateCreditor(transaction, paymentInfoMethod);

                CreateCreditorAccountIDIBAN(transaction);

                CreateUltimateCreditor(transaction);

                CreatePurpose(transaction);

                if(!transaction.IsAssegno && paymentInfoMethod != "CIR")
                    CreateRelatedRemittanceInformation(transaction);

                if (paymentInfoMethod == "CHK" || paymentInfoMethod == "CIR")
                    CreateRemittanceInformation(transaction, paymentInfoMethod, DebtorTaxID);


                writer.WriteEndElement(); //CdtTrfTxInf
                return true;

            }
            catch (Exception ex)
            {

                log.ErrorFormat(string.Format("CreateTransferTransactionInformation Exception {0}", ex.Message), "Sepa Object");
                return false;
            }
        }

        protected bool CreatePurpose(SepaCreditTransferTransaction transaction)
        {
            try
            {
                writer.WriteStartElement("Purp");

                //string value = transaction.CreditorTaxID ?? "";
                //if (value.Length > 35)
                //    value = value.Substring(0, 35);
                //writer.WriteStartElement("Prtry");
                //writer.WriteValue(value);
                //writer.WriteEndElement();


                if (!string.IsNullOrEmpty(transaction.PurposeProprietary))
                {
                    writer.WriteStartElement("Prtry");
                    writer.WriteValue(transaction.PurposeProprietary);
                    writer.WriteEndElement();
                }
                if (!string.IsNullOrEmpty(transaction.PurposeCode))
                {
                    writer.WriteStartElement("Cd");
                    writer.WriteValue(transaction.PurposeCode);//Utilizzare il codice(es.SUPP o CASH)
                    writer.WriteEndElement();
                }


                writer.WriteEndElement();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Purp Prtry Exception {0}", ex.Message);
                return false;
            }

        }
        protected bool CreateRelatedRemittanceInformation(SepaCreditTransferTransaction transaction)
        {

            try
            {
                writer.WriteStartElement("RltdRmtInf");

                string value = transaction.Purpose ?? "";
                if (value.Length > 35)
                    value = value.Substring(0, 35);
                //writer.WriteStartElement("Cd");


                writer.WriteStartElement("RmtId");
                writer.WriteValue(value);
                writer.WriteEndElement();
                writer.WriteEndElement();
                return true;

            }
            catch (Exception ex)
            {
                log.ErrorFormat("RltdRmtInf RmtId Exception {0}", ex.Message);
                return false;
            }



        }
        protected bool CreateRemittanceInformation(SepaCreditTransferTransaction transaction, string paymentInfoMethod, string DebtorTaxID)
        {

            try
            {
                writer.WriteStartElement("RmtInf");

                string value = transaction.RemittanceInformationUnstructured ?? "";
                if (value.Length > 35)
                    value = value.Substring(0, 35);                
                
                if (paymentInfoMethod == "CIR")
                {
                    var dataNascita = GetDateFromFiscalCode(transaction.CreditorCF);
                    writer.WriteStartElement("Strd");
                    writer.WriteStartElement("AddtlRmtInf");                    
                    writer.WriteValue(String.Format("/DTSC/{0}/DTBN/{1}//DESCRIZIONE PER BONIFICO IN CIRCOLARITA'", DateTime.Now.AddDays(90).ToString("yyyy-MM-dd"), dataNascita));
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    // TODO: qui si fa (nel caso in cui isBonificoDomiciliato): writestart(Strd), writestart(AddtlRmtInf), writevalue(/DTSC/2019-02-20/DTBN/2018-12-20//DESCRIZIONE DI TEST PER UN BONIFICO IN CIRCOLARITA')
                    // con valore formattato così: /DTSC/{data del mandato + 60gg (devono confermare)}/DTBN/{data del mandato}//{descrizione operazione da concordare, tipo "Emissione rimborso bonus idrico cliente 800000001"}
                    // si chiudono i due tag, e fine della modifica
                    // /DTBN/2018-12-20// --> è la data di nascita del beneficiario
                }
                else
                {
                    writer.WriteStartElement("Ustrd");
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                                
                writer.WriteEndElement();
                return true;

            }
            catch (Exception ex)
            {
                log.ErrorFormat("RltdRmtInf RmtId Exception {0}", ex.Message);
                return false;
            }



        }
        protected bool CreateUltimateCreditor(SepaCreditTransferTransaction transaction)
        {
            /*
             * <UltmtCdtr>
      <Nm>XXXXX</Nm>
      <Id>
        <OrgId>
          <Othr>
            <Id>0468651441</Id>
          </Othr>
        </OrgId>
      </Id>
    <UltmtCdtr>
             * */
            if (!string.IsNullOrEmpty(transaction.UltimateCreditorName) && !string.IsNullOrEmpty(transaction.UltimateCreditorTaxID))
            {
                writer.WriteStartElement("UltmtCdtr");
                writer.WriteStartElement("Nm");
                writer.WriteValue(transaction.UltimateCreditorName);
                writer.WriteEndElement();

                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                writer.WriteStartElement("Id");
                writer.WriteValue(transaction.UltimateCreditorTaxID);//codice fiscale?
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            return true;
        }

        protected bool CreateCreditorAccountIDIBAN(SepaCreditTransferTransaction transaction)
        {
            //Creditor account ID IBAN
            /*
             <CdtrAcct>
                  <Id>
                    <IBAN>BE43187123456701</IBAN>
                  </Id>
             </CdtrAcct>
             * */

            if (!string.IsNullOrEmpty(transaction.CreditorIBAN))
            {

                if (transaction.CreditorIBAN == "ASSEGNO" || transaction.CreditorIBAN == "BONIFICO_DOMICILIATO")
                    return true;

                writer.WriteStartElement("CdtrAcct");
                writer.WriteStartElement("Id");

                writer.WriteStartElement("IBAN");
                writer.WriteValue(transaction.CreditorIBAN);
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            return true;
        }

        protected bool CreateCreditor(SepaCreditTransferTransaction transaction, string paymentMethod)
        {
            /*<Cdtr>
              <Nm>SocMetal</Nm>
              <PstlAdr>
                <Ctry>BE</Ctry>
                <AdrLine>Hoogstraat 156</AdrLine>
                <AdrLine>2000 Antwerp</AdrLine>
              </PstlAdr>
              <Id>
                <OrgId>
                  <Othr>
                    <Id>0468651441</Id> 
                  </Othr>
                </OrgId>
              </Id>
            </Cdtr>
           */

            writer.WriteStartElement("Cdtr");
            writer.WriteStartElement("Nm");
            writer.WriteValue(transaction.CreditorName);
            writer.WriteEndElement(); //Nm




            if (!string.IsNullOrEmpty(transaction.CreditorPostalAddressPostalCode)
                && !string.IsNullOrEmpty(transaction.CreditorPostalAddressStreet)
                && !string.IsNullOrEmpty(transaction.CreditorPostalAddressTownName))
            {
                writer.WriteStartElement("PstlAdr");

                if (paymentMethod == "CIR")
                {
                    // TODO <AdrTp> 
                    //-"ADDR" L'indirizzo specificato è il completo indirizzo postale.
                    //-"BIZZ" L'indirizzo specificato è l'indirizzo d'ufficio (business address)
                    //-"DLVY" L'indirizzo specificato è quello a cui normalmente si consegna
                    //-"HOME" L'indirizzo specificato è quello residenziale
                    //-"MLTO" L'indirizzo specificato è quello cui è inviata normalmente la posta
                    //-"PBOX" L'indirizzo specificato è una casella postale (PO Box).
                    writer.WriteStartElement("AdrTp");
                    writer.WriteValue("ADDR");
                    writer.WriteEndElement();//AdrTp
                }

                writer.WriteStartElement("StrtNm");
                writer.WriteValue(transaction.CreditorPostalAddressStreet);
                writer.WriteEndElement(); //StreetName
                writer.WriteStartElement("PstCd");
                writer.WriteValue(transaction.CreditorPostalAddressPostalCode);
                writer.WriteEndElement(); //PstCd
                writer.WriteStartElement("TwnNm");
                writer.WriteValue(transaction.CreditorPostalAddressTownName);
                writer.WriteEndElement(); //TwnNm

                if (paymentMethod == "CIR")
                {
                    writer.WriteStartElement("CtrySubDvsn");
                    writer.WriteValue((String.IsNullOrEmpty(transaction.CreditorPostalAddressProvince) ? "XX" : transaction.CreditorPostalAddressProvince));
                    writer.WriteEndElement(); //CtrySubDvsn
                }

                writer.WriteEndElement(); //PstlAdr
            }


            //if (!string.IsNullOrEmpty(transaction.CreditorPostalAddressCountryCode))
            //{


            //    writer.WriteStartElement("PstlAdr");
            //    writer.WriteStartElement("Ctry");
            //    writer.WriteValue(transaction.CreditorPostalAddressCountryCode);
            //    writer.WriteEndElement(); //Ctry

            //    string[] token = transaction.CreditorPostalAddress.Split(new char[] { '|', '\r' });
            //    foreach (string addressLine in token)
            //    {

            //        writer.WriteStartElement("AdrLine");
            //        writer.WriteValue(addressLine);
            //        writer.WriteEndElement(); //AdrLine
            //        //<AdrLine>XXXXXX </AdrLine>
            //    }

            //    writer.WriteEndElement(); //PstlAdr
            //}
            /*<Id>
                <OrgId>
                  <Othr>
                    <Id>0468651441</Id> 
                  </Othr>
                </OrgId>
              </Id>*/
            if (!string.IsNullOrEmpty(transaction.CreditorTaxID))
            {

                writer.WriteStartElement("Id");
                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");
                writer.WriteStartElement("Id");
                if (!String.IsNullOrEmpty(transaction.CreditorCF) && paymentMethod == "CIR")               
                    writer.WriteValue(transaction.CreditorCF);               
                else
                    writer.WriteValue(transaction.CreditorTaxID);//codice fiscale?ID_cliente
                                    
                writer.WriteEndElement(); //Id
                if (!String.IsNullOrEmpty(transaction.CreditorCF) && paymentMethod == "CIR")
                {
                    writer.WriteStartElement("Issr");
                    writer.WriteValue("ADE");
                    writer.WriteEndElement(); //Issr   
                }
                writer.WriteEndElement(); //Othr
                writer.WriteEndElement(); //OrgId
                writer.WriteEndElement(); //Id
            }

            writer.WriteEndElement(); //Cdtr
            return true;
        }

        /// <summary>
        /// è segnato opzionale, e invece è richiesto da questo servizio di internet banking!!
        /// </summary>
        /// <returns></returns>
        protected bool CreateChargeBearer()
        {
            writer.WriteStartElement("ChrgBr");
            writer.WriteValue("SLEV");
            writer.WriteEndElement();

            /*
             CBIChargeBearerTypeCode
             * This element is optional. Recommended to be used at Payment Information level. Allowed also to be at Credit Transfer Transaction level with crossborder payments.
            Values according to ISO20022:
                DEBT (debtor)
                CRED (creditor) 
                SHAR (shared)
                SLEV (service level)

                Default value (Nordea):
                Shared charges (SHAR). Use only SLEV with SEPA credit transfers.*/
            return true;
        }


        protected bool CreateDebitor(SepaPaymentElement element)
        {
            /*
             <Dbtr>
        <Nm>XXXXX</Nm>
        <PstlAdr>
          <Ctry>CC</Ctry>
          <AdrLine>XXXXXX </AdrLine>
          <AdrLine>XXXXXX </AdrLine>
        </PstlAdr>
        <Id>
          <OrgId>
            <Othr>
              <Id>0468651441</Id>
            </Othr>
          </OrgId>
        </Id>
      </Dbtr>*/


            writer.WriteStartElement("Dbtr");

            writer.WriteStartElement("Nm");
            writer.WriteValue(element.DebtorName);
            writer.WriteEndElement();








            if (!string.IsNullOrEmpty(element.DebtorCountry))
            {


                writer.WriteStartElement("PstlAdr");

                writer.WriteStartElement("Ctry");
                writer.WriteValue(element.DebtorCountry);
                writer.WriteEndElement(); //Ctry

                string[] token = element.DebtorPostalAddressLines.Split(new char[] { '|', '\r' });
                foreach (string addressLine in token)
                {

                    writer.WriteStartElement("AdrLine");
                    writer.WriteValue(addressLine);
                    writer.WriteEndElement(); //AdrLine
                    //<AdrLine>XXXXXX </AdrLine>
                }

                writer.WriteEndElement(); //PstlAdr
            }







            /*
             * <Id>
          <OrgId>
            <Othr>
              <Id>0468651441</Id>
            </Othr>
          </OrgId>
        </Id>*/


            writer.WriteStartElement("Id");
            writer.WriteStartElement("OrgId");
            writer.WriteStartElement("Othr");

            writer.WriteStartElement("Id");
            writer.WriteValue(element.DebtorTaxID);//codice fiscale?Si
            writer.WriteEndElement();
            //<Issr>ADE</Issr> solo se presente il CF/P.IVA

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndElement(); //Dbtr
            return true;
        }

        protected bool CreateDebtorAccount(SepaPaymentElement element)
        {
            /*<DbtrAcct>
        <Id>
          <IBAN>BE68539007547034</IBAN>
        </Id>
        <Ccy>EUR</Ccy>
      </DbtrAcct>*/


            writer.WriteStartElement("DbtrAcct");
            writer.WriteStartElement("Id");
            writer.WriteStartElement("IBAN");
            writer.WriteValue(element.DebtorIBAN);
            writer.WriteEndElement(); //IBAN
            writer.WriteEndElement(); //Id
            writer.WriteEndElement(); //DbtrAcct

            return true;
        }

        protected bool CreateDebtorAgent(SepaPaymentElement element)
        {
            /*<DbtrAgt>
                <FinInstnId>
                    <BIC>AAAABE33</BIC>
                </FinInstnId>
            </DbtrAgt>*/

            //if (!string.IsNullOrEmpty(element.DebtorBIC))
            {
                writer.WriteStartElement("DbtrAgt");
                writer.WriteStartElement("FinInstnId");

                writer.WriteStartElement("ClrSysMmbId");
                writer.WriteStartElement("MmbId");
                writer.WriteValue(element.DebtorABI/*"ITNCC0123456"*/); //abi ord

                writer.WriteEndElement(); //ClrSysMmbId
                writer.WriteEndElement(); //MmbId

                /*writer.WriteStartElement("BIC");

                if (string.IsNullOrEmpty(element.DebtorBIC))
                    element.DebtorBIC = "GEBABEBB";

                writer.WriteValue(element.DebtorBIC);
                writer.WriteEndElement(); //BIC*/

                writer.WriteEndElement(); //FinInstnId
                writer.WriteEndElement(); //DbtrAgt
            }

            return true;
        }
        //http://docs.oracle.com/cd/E16582_01/doc.91/e15104/fields_sepa_pay_file_appx.htm


        protected bool CreateUltimateDebtor(SepaPaymentElement element)
        {
            /*
             <UltmtDbtr>
                <Nm>XXXXX</Nm>
                    <Id>
                        <OrgId>
                          <Othr>
                            <Id>0468651441</Id> 
                          </Othr>
                        </OrgId>
                      </Id>
              </UltmtDbtr>
             **/
            if (!string.IsNullOrEmpty(element.UltimateDebtorName) && !string.IsNullOrEmpty(element.UltimateDebtorTaxID))
            {
                writer.WriteStartElement("UltmtDbtr");

                writer.WriteStartElement("Nm");
                writer.WriteValue(element.UltimateDebtorName);
                writer.WriteEndElement(); //Nm

                writer.WriteStartElement("Id");

                writer.WriteStartElement("OrgId");
                writer.WriteStartElement("Othr");

                writer.WriteStartElement("Id");
                writer.WriteValue(element.UltimateDebtorTaxID);//codice fiscale?
                writer.WriteEndElement(); //Othr

                writer.WriteEndElement(); //Othr

                writer.WriteEndElement(); //OrgId

                writer.WriteEndElement(); //Id

                writer.WriteEndElement(); //UltmtDbtr
            }
            return true;
        }
        protected bool CreatePmtTpInf()
        {
            /*<PmtTpInf>
        <InstrPrty>HIGH </InstrPrty>
        <SvcLvl>
          <Cd>SEPA</Cd>
        </SvcLvl>
        <LclInstrm>
          <Cd>
            TRF</Cd>
      </LclInstrm>
        <CtgyPurp>
          <Cd>
            SUPP</Cd>
      </CtgyPurp>
      </PmtTpInf>*/
            
            writer.WriteStartElement("PmtTpInf");

            writer.WriteStartElement("InstrPrty");
            writer.WriteValue("NORM");
            writer.WriteEndElement();
            
            writer.WriteStartElement("SvcLvl");
            writer.WriteStartElement("Cd");
            writer.WriteValue("SEPA");
            writer.WriteEndElement(); //cd
            writer.WriteEndElement(); //SvcLvl

            //writer.WriteStartElement("LclInstrm");
            //writer.WriteStartElement("Cd");
            //writer.WriteValue("TRF");
            //writer.WriteEndElement(); //cd
            //writer.WriteEndElement(); //LclInstrm

            //writer.WriteStartElement("CtgyPurp");

            //writer.WriteStartElement("Cd");
            //writer.WriteValue("SUPP"); //vedi file Payment_ExternalCodeLists_09June09_v4.xls in scheda 3-CategoryPurpose
            //writer.WriteEndElement(); //Cd

            /*string causale = "Causale generica";
            if (message.SepaPaymentElements.ToList().Count>0)
                if (message.SepaPaymentElements.ToList()[0].SepaCreditTransferTransaction.ToList().Count>0)
                    causale = message.SepaPaymentElements.ToList()[0].SepaCreditTransferTransaction.ToList()[0].Purpose;
            if (causale.Length >= 35)
                causale = causale.Substring(0, 35);

            writer.WriteStartElement("Prtry");
            writer.WriteValue(causale); 
            writer.WriteEndElement(); //Prtry*/

            //writer.WriteEndElement(); //CtgyPurp
            /*<CtgyPurp>
          <Cd>
            SUPP</Cd>
      </CtgyPurp>*/

            writer.WriteEndElement();
            
            return true;
        }

        protected bool CreateRemittanceInformation()
        {


            return true;
        }


        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        private string GetDateFromFiscalCode(string fiscalCode)
        {
            try
            {
                Dictionary<string, string> month = new Dictionary<string, string>();
                // To Upper
                fiscalCode = fiscalCode.ToUpper();
                month.Add("A", "01");
                month.Add("B", "02");
                month.Add("C", "03");
                month.Add("D", "04");
                month.Add("E", "05");
                month.Add("H", "06");
                month.Add("L", "07");
                month.Add("M", "08");
                month.Add("P", "09");
                month.Add("R", "10");
                month.Add("S", "11");
                month.Add("T", "12");
                // Get Date
                string date = fiscalCode.Substring(6, 5);
                int y = int.Parse(date.Substring(0, 2));
                string yy = ((y < 9) ? "20" : "19") +y.ToString("00");
                string m = month[date.Substring(2, 1)];
                int d = int.Parse(date.Substring(3, 2));
                if (d > 31)
                    d -= 40;
                // Return Date
                return string.Format("{2}-{1}-{0}", d.ToString("00"), m, yy);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
