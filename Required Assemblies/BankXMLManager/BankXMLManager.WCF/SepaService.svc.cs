using System;
using System.Threading.Tasks;

namespace BankXMLManager.WCF
{
    using SepaManager.Base.Worker;
    using System.Xml;
    using SepaManager.Base;
    using System.Collections.Generic;
    public class SepaService : IBankXMLService
    {
        public long CreateXml(string FileName, string PkOperazione, string Autore)
        {
            return SepaWorkerManager.CreateXml(FileName, PkOperazione, Autore);
        }

        public IAsyncResult BeginCreateXml(string FileName, string PkOperazione, string Autore, AsyncCallback callback, object state)
        {
            var task = Task<long>.Factory.StartNew(x => SepaWorkerManager.CreateXml(FileName, PkOperazione, Autore), state);
            return task.ContinueWith(res => callback(task));
        }

        public long EndCreateXml(IAsyncResult result)
        {
            return ((Task<long>)result).Result;
        }
        //public string GetXmlStringDocument(long SepaHederID)
        //{
        //    return SepaWorkerManager.GetXmlDocument(SepaHederID);
        //}
        public BankXMLOutput GetXmlDocument(long SepaHederID)
        {
            return SepaWorkerManager.GetXmlDocument(SepaHederID);
        }

        public SepaManager.Base.BankXMLOutput RegenerateXmlDocument(long pkOperazione)
        {
            return SepaWorkerManager.RegenerateXmlDocument(pkOperazione);            
        }

        public List<String> CreateCsvDocument(long pkOperazione)
        {
            return SepaWorkerManager.CreateCsvDocument(pkOperazione);
        }
    }
}