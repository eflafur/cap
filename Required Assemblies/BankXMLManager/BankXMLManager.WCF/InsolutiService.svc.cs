using SepaManager.Base.Entity.Insoluti;
using SepaManager.Base.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BankXMLManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InsolutiService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select InsolutiService.svc or InsolutiService.svc.cs at the Solution Explorer and start debugging.
    public class InsolutiService : IInsolutiService
    {
        public FileInsoluti SaveInsoluti(string XmlString, DateTime DataRegistrazione, string Utente)
        {
            var worker = new InsolutiWorkerManager();
            return worker.SaveXml(XmlString, DataRegistrazione, Utente);
        }

    }
}
