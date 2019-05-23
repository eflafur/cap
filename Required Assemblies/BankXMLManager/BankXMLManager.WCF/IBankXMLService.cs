using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using SepaManager.Base;

namespace BankXMLManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBankXMLService
    {

        [OperationContract]
        long CreateXml(string FileName, string PkOperazione, string Autore);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginCreateXml(string FileName, string PkOperazione, string Autore, AsyncCallback callback, object state);
        long EndCreateXml(IAsyncResult result);

        //[OperationContract]
        //string GetXmlStringDocument(long SepaHederID);
        [OperationContract]
        BankXMLOutput GetXmlDocument(long SepaHederID);
        [OperationContract]
        BankXMLOutput RegenerateXmlDocument(long pkOperazione);
        [OperationContract]
        List<String> CreateCsvDocument(long pkOperazione);

    }
}