using GruppoCap.Utils.Xml;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GruppoCap
{

    public static class XmlUtils
    {
        // SERIALIZE TO TEXT WRITER
        public static void SerializeToTextWriter(Object o, TextWriter tw, Boolean indent = true)
        {
            XmlSerializerNamespaces ns;
            XmlSerializer serializer;
            XmlWriterSettings settings;

            // CUSTOM NAMESPACES FOR SERIALIZATION
            ns = new XmlSerializerNamespaces();

            // EMPTY NAMESPACE AND EMPTY VALUE
            ns.Add("", "");

            // CREATE THE SERIALIZER FOR TYPE T
            serializer = new XmlSerializer(o.GetType());

            // SETTINGS TO REMOVE THE <?xml version="1.0" encoding="utf-8"?>
            settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(tw, settings))
            {
                // SERIALIZE THE OBJECT WITH OUR OWN NAMESPACES (NOTICE THE OVERLOAD)
                serializer.Serialize(xmlWriter, o, ns);
            }
        }

        // SERIALIZE TO STRING
        public static String SerializeToXmlString(Object obj, Boolean indent = true)
        {
            StringBuilder sb;

            sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                SerializeToTextWriter(obj, sw, indent);
            }

            return sb.ToString();
        }

        // TO XELEMENT
        public static XElement ToXElement(this Object obj)
        {
            XElement xe;

            xe = XElement.Parse(SerializeToXmlString(obj));

            return xe;
        }

        // DESERIALIZE FROM TEXT READER
        public static Object DeserializeFromTextReader(TextReader tr, Type objType)
        {
            if (tr == null)
                return null;

            XmlSerializerNamespaces ns;
            XmlSerializer serializer;
            XmlReaderSettings settings;
            Object o;

            // CUSTOM NAMESPACES FOR SERIALIZATION
            ns = new XmlSerializerNamespaces();

            // EMPTY NAMESPACE AND EMPTY VALUE
            ns.Add("", "");

            // CREATE THE SERIALIZER FOR TYPE T
            serializer = new XmlSerializer(objType);

            // SETTINGS TO REMOVE THE <?xml version="1.0" encoding="utf-8"?>
            settings = new XmlReaderSettings();
            //xrSettings.OmitXmlDeclaration = true;
            //xrSettings.Indent = true;

            using (XmlReader xr = XmlReader.Create(tr, settings))
            {
                // SERIALIZE THE OBJECT WITH OUR OWN NAMESPACES (NOTICE THE OVERLOAD)
                o = serializer.Deserialize(xr);
            }

            return o;
        }

        // DESERIALIZE FROM TEXT READER
        public static T DeserializeFromTextReader<T>(TextReader tr)
            where T : class
        {
            return DeserializeFromTextReader(tr, typeof(T)) as T;
        }

        // DESERIALIZE FROM STRING
        public static Object DeserializeFromString(String xmlSource, Type objType)
        {
            if (String.IsNullOrEmpty(xmlSource))
                return null;

            Object o;

            using (StringReader sr = new StringReader(xmlSource))
            {
                o = DeserializeFromTextReader(sr, objType);
            }

            return o;
        }

        // DESERIALIZE FROM STRING
        public static T DeserializeFromString<T>(String xmlSource)
            where T : class
        {
            return DeserializeFromString(xmlSource, typeof(T)) as T;
        }

        // DESERIALIZE INTO
        public static Object DeserializeInto(this XElement xe, Type objType)
        {
            if (xe == null)
                return null;

            return DeserializeFromString(xe.ToString(SaveOptions.DisableFormatting), objType);
        }

        // DESERIALIZE INTO
        public static T DeserializeInto<T>(this XElement xe)
            where T : class
        {
            return xe.DeserializeInto(typeof(T)) as T;
        }

        // TO STRING INDENTED
        public static String ToStringIndented(this XmlReader xmlReader, String indentChars = null)
        {
            //Ensure.Arg(() => xmlReader).IsNotNull();

            StringBuilder sb;

            sb = new StringBuilder();

            StringWriter sw = null;

            try
            {
                sw = new StringWriter(sb);

                if (xmlReader.Read())
                {
                    XmlWriterSettings settings;

                    settings = new XmlWriterSettings();
                    settings.Indent = true;
                    if (indentChars != null) settings.IndentChars = indentChars;
                    settings.OmitXmlDeclaration = (xmlReader.NodeType != XmlNodeType.XmlDeclaration);

                    using (XmlWriter xmlWriter = XmlWriter.Create(sw, settings))
                    {
                        sw = null;
                        do
                        {
                            xmlWriter.WriteNode(xmlReader, false);
                        } while (xmlReader.Read());
                    }
                }
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }

            return sb.ToString();
        }

        // TO STRING INDENTED
        public static String ToStringIndented(this XmlNode xmlNode, String indentChars = null)
        {
            //Ensure.Arg(() => xmlNode).IsNotNull();

            using (var xnr = new XmlNodeReader(xmlNode))
            {
                return xnr.ToStringIndented(indentChars);
            }
        }

        // REINDENT XML STRING
        public static String ReindentXmlString(String xmlSource, String indentChars = null)
        {
            //Ensure.Arg(() => xmlSource).IsNotNull();

            StringReader sr = null;

            try
            {
                sr = new StringReader(xmlSource);

                using (var xr = XmlReader.Create(sr))
                {
                    sr = null;
                    return xr.ToStringIndented(indentChars);
                }
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
        }

        // GET ATTR VALUE OR DEFAULT
        public static String GetAttrValueOrDefault(this XElement xe, String attributeName, String defaultValue = null)
        {
            // CHECKs
            //Ensure.Arg(() => attributeName).IsNotNullOrWhiteSpace();

            if (xe == null)
                return defaultValue;

            XAttribute xa;

            xa = xe.Attribute(attributeName);

            if (xa == null)
                return defaultValue;

            return xa.Value;
        }

        //// PARSE AND GET ENCODING OR
        //public static Encoding ParseAndGetEncodingOr(String xmlSource, Encoding defaultEncoding)
        //{
        //   try
        //   {
        //      return Encoding.GetEncoding(XDocument.Parse(xmlSource).Declaration.Encoding);
        //   }
        //   catch (Exception)
        //   {
        //      return defaultEncoding;
        //   }
        //}

        //// PARSE AND GET ENCODING OR UTF8
        //public static Encoding ParseAndGetEncodingOrUTF8(String xmlSource)
        //{
        //   return ParseAndGetEncodingOr(xmlSource, Encoding.UTF8);
        //}

        // GET ENCODING FROM XDOCUMENT DECLARATION OR
        public static Encoding GetEncoding(this XDocument xdoc, Encoding defaultEncoding = null)
        {
            try { return Encoding.GetEncoding(xdoc.Declaration.Encoding); }
            catch { return defaultEncoding ?? Encoding.UTF8; }
        }

        // LOAD XDOCUMENT WITH ENCODING AUTO DETECTION
        public static XDocument LoadXDocumentWithEncodingAutoDetection(String path, Encoding suggestedEncoding = null)
        {
            String xmlSource;
            XDocument xdoc;
            Encoding parsedEncoding;

            // CHECK - SUGGESTED ENCODING
            if (suggestedEncoding == null)
            {
                suggestedEncoding = Encoding.UTF8;
            }

            // READ ALL TEXT
            xmlSource = File.ReadAllText(path, suggestedEncoding);

            // PARSE THE XDOCUMENT
            xdoc = XDocument.Parse(xmlSource);

            // GET THE PARSED ENCODING
            parsedEncoding = xdoc.GetEncoding();

            // COMPARE THE SUGGESTED ENCODING AND THE PARSED ENCODING
            if (String.Equals(parsedEncoding.WebName, suggestedEncoding.WebName, StringComparison.InvariantCultureIgnoreCase))
            {
                // THE SUGGESTED ENCODING WAS CORRECT -> PROCEED
                return xdoc;
            }

            return XDocument.Parse(File.ReadAllText(path, parsedEncoding));
        }

        // TO XDOCUMENT
        public static XDocument ToXDocument(this XmlDocument document)
        {
            return document.ToXDocument(LoadOptions.None);
        }

        // TO XDOCUMENT
        public static XDocument ToXDocument(this XmlDocument document, LoadOptions options)
        {
            using (XmlNodeReader reader = new XmlNodeReader(document))
            {
                return XDocument.Load(reader, options);
            }
        }

        // TO STRING WITH DECLARATION
        public static String ToStringWithDeclaration(this XDocument doc)
        {
            if (doc == null)
                return String.Empty;

            Encoding _encoding = doc.GetEncoding(Encoding.UTF8);

            StringBuilder builder = new StringBuilder();
            using (TextWriter writer = new EncodingStringWriter(builder, _encoding))
            {
                doc.Save(writer);
            }

            return builder.ToString();
        }
    }

}