using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SepaManager.Base.Entity.Sdd;

namespace BankXMLManager.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISddService" in both code and config file together.
    [ServiceContract]
    public interface ISddService : IBankXMLService
    {
        [OperationContract]
        SedaCbiSddReturn CreateSddXml(long BillingBatch);
        [OperationContract]
        SedaCbiSddReturn GetSddXml(long BillingBatch);
    }
}
