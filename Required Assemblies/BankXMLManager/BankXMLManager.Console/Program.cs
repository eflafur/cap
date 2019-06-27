using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace BankXMLManager.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1 = CreateXML");
                Console.WriteLine("2 = GetXML");
                Console.WriteLine("3 = GetXMLString");
                Console.Write("what I do? ");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.KeyChar == '1')
                    CreateXML();
                if (key.KeyChar == '2')
                    GetXML();
                if (key.KeyChar == '3')
                    GetXMLString();
                else
                    Main(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE: " + ex.Message);
                Console.Read();
            }
        }
        static void CreateXML()
        {
            try
            {
                Console.WriteLine("Start - CreateXML");
                var proxy = new BankXMLManager.SepaService.BankXMLServiceClient();
                Console.WriteLine("Proxy ready");

                object state = "This can be whatever you want it to be";

                // Parameters that you pass to the BeginDoWork method go right after EndDoWork.
                var task = Task<long>.Factory.FromAsync(proxy.BeginCreateXml, proxy.EndCreateXml, "prova", "10", state);

                Console.WriteLine("Task Executed");

                // When you are done, access the Result.
                // If the call is not finished, it will block until the result is ready
                Console.WriteLine("Task result:" + task.Result);
                Console.Read();
                proxy.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE: " + ex.Message);
                Console.Read();
            }
            Main(null);
        }
        static void GetXMLString()
        {
            try
            {
                Console.WriteLine("Start - GetXMLString");
                var proxy = new BankXMLManager.SepaService.BankXMLServiceClient();
                Console.WriteLine("Proxy ready");

                object state = "This can be whatever you want it to be";

                // Parameters that you pass to the BeginDoWork method go right after EndDoWork.
                string fileXML = proxy.GetXmlStringDocument(10L);
                Console.WriteLine(fileXML);
                using (StreamWriter sw = new StreamWriter(@"C:\temp\file.txt"))
                {
                    sw.Write(fileXML);
                    sw.Flush();
                }
                XmlWriterSettings settings = new XmlWriterSettings();
                //settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                //settings.NewLineOnAttributes = true;
                settings.ConformanceLevel = ConformanceLevel.Document;

                using (XmlWriter xmlW = XmlWriter.Create(@"C:\Temp\temp.xml", settings))
                {
                    xmlW.WriteRaw(fileXML);
                    xmlW.Flush();
                }
                Console.WriteLine("Task Executed");

                // When you are done, access the Result.
                // If the call is not finished, it will block until the result is ready
                Console.WriteLine("Task result:" + fileXML.ToString());
                Console.Read();
                proxy.Close();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(@"C:\temp\log.txt"))
                {
                    sw.Write(ex.ToString());
                    sw.Flush();
                }
                Console.WriteLine("ERRORE: " + ex.Message);
                Console.Read();
            }
            Main(null);
        }
        static void GetXML()
        {
            try
            {
                Console.WriteLine("Start - GetXML");
                var proxy = new BankXMLManager.SepaService.BankXMLServiceClient();
                Console.WriteLine("Proxy ready");

                object state = "This can be whatever you want it to be";

                // Parameters that you pass to the BeginDoWork method go right after EndDoWork.
                XmlElement fileXML = proxy.GetXmlDocument(10L);
                Console.WriteLine(fileXML.ToString());
                using (StreamWriter sw = new StreamWriter(@"C:\temp\file.txt"))
                {
                    sw.Write(fileXML);
                    sw.Flush();
                }

                using (XmlWriter xmlW = XmlWriter.Create(@"C:\Temp\temp.xml"))
                {
                    fileXML.WriteTo(xmlW);
                }
                Console.WriteLine("Task Executed");

                // When you are done, access the Result.
                // If the call is not finished, it will block until the result is ready
                Console.WriteLine("Task result:" + fileXML.ToString());
                Console.Read();
                proxy.Close();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(@"C:\temp\log.txt"))
                {
                    sw.Write(ex.ToString());
                    sw.Flush();
                }
                Console.WriteLine("ERRORE: " + ex.Message);
                Console.Read();
            }
            Main(null);
        }
    }
}