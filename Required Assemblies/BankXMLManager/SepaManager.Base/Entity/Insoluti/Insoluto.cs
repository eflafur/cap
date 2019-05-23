using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SepaManager.Base.Entity.Insoluti
{
    public class Insoluto
    {
        public Insoluto(string XmlString)
        {
            _InsolutiMessage = new CBISDDStsRptLogMsg000006();
            var textReader = new StringReader(XmlString);
            var serializer = new XmlSerializer(typeof(CBISDDStsRptLogMsg000006));
            _InsolutiMessage = (CBISDDStsRptLogMsg000006)serializer.Deserialize(textReader);

        }
        private CBISDDStsRptLogMsg000006 _InsolutiMessage;
        public CBISDDStsRptLogMsg000006 Insoluti
        {
            get
            {
                return _InsolutiMessage;
            }
        }



    }
}
