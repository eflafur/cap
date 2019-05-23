using SepaManager.Base.Entity.Insoluti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BankXMLManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInsolutiService" in both code and config file together.
    [ServiceContract]
    public interface IInsolutiService
    {
        [OperationContract]
        FileInsoluti SaveInsoluti(string XmlString, DateTime DataRegistrazione, string Utente);

    }
}
