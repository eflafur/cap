using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SepaManager.Base.Worker;
using SepaManager.Base.Entity.Sdd;

namespace BankXMLManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SddService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SddService.svc or SddService.svc.cs at the Solution Explorer and start debugging.
    public class SddService : ISddService
    {
        public long CreateXml(string FileName, string PkOperazione, string Autore)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginCreateXml(string FileName, string PkOperazione, string Autore, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public long EndCreateXml(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public SepaManager.Base.BankXMLOutput GetXmlDocument(long SepaHederID)
        {
            throw new NotImplementedException();
        }

        public SepaManager.Base.BankXMLOutput RegenerateXmlDocument(long pkOperazione)
        {
            throw new NotImplementedException();
        }

        public List<String> CreateCsvDocument(long pkOperazione)
        {
            throw new NotImplementedException();
        }

        public SedaCbiSddReturn CreateSddXml(long BillingBatch)
        {
            return SepaSddWorkerManager.CreateXml(BillingBatch);
        }
        public SedaCbiSddReturn GetSddXml(long BillingBatch)
        {
            return SepaSddWorkerManager.GetXml(BillingBatch);
        }
    }
}
