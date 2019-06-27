using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SepaManager.Base.Entity.Sdd
{
    /// <summary>
    /// Aggiunge metodi alla classe CBISDDReqLogMsg000006 (contenuta in SedaCbiSdd.cs)  generata automanticamente da XSD.EXE
    /// 
    /// </summary>
    public partial class CBISDDReqLogMsg000006
    {
        /// <summary>
        /// Restituisce l'istanza corrente in formato stringa XML
        /// </summary>
        /// <returns></returns>
        public string ToXmlString()
        {
            var Serializer = new XmlSerializer(typeof(CBISDDReqLogMsg000006));
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 };
            using (StringWriter stream = new Utf8StringWriter())
            using (var Writer = XmlWriter.Create(stream, settings))
            {
                Serializer.Serialize(Writer, this, null);
                return stream.ToString();
            }
        }
    }
}
