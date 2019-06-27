using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace SepaManager.Base
{
    [Serializable]
    [DataContract]
    public class BankXMLOutput 
    {
        public BankXMLOutput(string fileName, string xmlStringDocument, XmlElement xmlDocument)
        {
            FileName = fileName;
            XmlStringDocument = xmlStringDocument;
            XmlDocument = xmlDocument;
        }
        [DataMember]
        public string FileName { get; private set; }
        [DataMember]
        public string XmlStringDocument { get; private set; }
        [DataMember]
        public XmlElement XmlDocument { get; private set; }
    }
}
