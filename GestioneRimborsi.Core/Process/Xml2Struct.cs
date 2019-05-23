using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GestioneRimborsi.Core.Process
{
    public class Xml2Struct
    {
        static List<string> ls = new List<string>();

        public static void UploadXml(IBonusIdricoRepo bi,string xmlfile)
        {
            string[] s1 = new string[] { "><" };
            string[] s2 = new string[] { "<xsd:" };
            string[] m = new string[] { ">" };
            string[] x = new string[] { "</xsd" };
            string[] t = new string[] { "tipo" };
            string lineXml;
            List<string> origin;
            StreamReader input;
            var range = 45;

            try
            {
                var f = new FileStream(string.Format(@"c:\users\ciemm\downloads\{0}", xmlfile), FileMode.Open, FileAccess.ReadWrite);
                input = new StreamReader(f);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var f1 = new FileStream(@"c:\users\ciemm\downloads\xml2row", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter output = new StreamWriter(f1);

            lineXml = input.ReadLine();
            lineXml = input.ReadLine();

            

            var listXml = lineXml.Split(s2, StringSplitOptions.None).ToList();

            origin = new List<string>(listXml);

            ls.Clear();

            while (origin.Count() >= 45)
            {
                try
                {
                    origin.RemoveRange(0, range);
                    listXml.RemoveRange(range, origin.Count());
                    Valuate(listXml, listXml.Count() - 1, s1, 0, 0, listXml);
                    listXml = new List<string>(origin);
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                }
            }

            if (origin.Count() < 45)
            {
                listXml = new List<string>(origin);
                Valuate(listXml, listXml.Count() - 1, s1, 0, 0, listXml);
            }

            foreach (var item in ls)
                output.WriteLine(item);

            output.Flush();

            input.Dispose();
            input.Close();
            output.Dispose();
            output.Close();

            CreateFinal(m, t, x);
            Xml2Text();
            Xml2Db(bi);
        }

        public static string Valuate(List<string> item, int x, string[] s1, int k, int x1, List<string> old)
        {

            var line = k == 0 ? item[x] : item[x] + '>';
            var tot = line.Contains("><");
            if (tot)
            {
                old = new List<string>(item);
                var w1 = line.Split(s1, StringSplitOptions.None);
                item = w1.ToList();

                x1 = x;
                x = item.Count();
                k = 1;
            }

            if (k == 0)
            {
                if (x == 0)
                    return line;
                x = x - 1;
            }

            if (k == 1)
            {
                if (x == 0)
                {
                    x = x1;
                    k = 0;
                    item = old.ToList();
                    if (x == 0)
                        return line;
                }
                x = x - 1;
            }

            ls.Add(Valuate(item, x, s1, k, x1, old));
            return line;
        }

        public static void CreateFinal(string[] m, string[] t, string[] x)
        {
            string line;
            var f1 = new FileStream(@"c:\users\ciemm\downloads\xml2row", FileMode.Open, FileAccess.ReadWrite);
            var f2 = new FileStream(@"c:\users\ciemm\downloads\final", FileMode.Create, FileAccess.ReadWrite);
            StreamReader input = new StreamReader(f1);
            StreamWriter output = new StreamWriter(f2);
            line = input.ReadLine();

            while ((line = input.ReadLine()) != null)
            {
                if (line.Contains("messaggio") || line == "" || line.Contains("xmlns"))
                    continue;

                var w = line.Split(m, StringSplitOptions.None);

                if (w.Count() == 2)
                {
                    if (w[0].Contains("tipo"))
                    {
                        output.WriteLine(w[0].Split(t, StringSplitOptions.None).First());
                        continue;
                    }
                    output.WriteLine(w[0]);
                    continue;
                }

                if (w.Count() == 3)
                {
                    output.Write(w[0] + " ");
                    output.WriteLine(w[1].Split(x, StringSplitOptions.None).First());
                }

            }
            output.Flush();
            output.Dispose();
            output.Close();
            input.Dispose();
            input.Close();

        }

        public static void Xml2Text()
        {
            var f = new FileStream(@"c:\users\ciemm\downloads\final", FileMode.Open, FileAccess.ReadWrite);
            var f1 = new FileStream(@"c:\users\ciemm\downloads\finaltext", FileMode.Create, FileAccess.ReadWrite);
            StreamReader input = new StreamReader(f);
            StreamWriter bioutput = new StreamWriter(f1);
            string[] s5 = new string[] { " " };
            int x = 0;
            string line1;
            var ls = new List<string>();
            //       int i = 0;

            while ((line1 = input.ReadLine()) != null)
            {
                var line = line1.Trim();
                var parole = line.Split(s5, StringSplitOptions.None);
                //    ++i;

                if (parole[0].Equals("/xsd:richiesta"))
                    bioutput.WriteLine("END");


                if (parole.Count() == 1 || line.Contains("xsd"))
                {
                    if (parole[0].Contains("/"))
                    {
                        try
                        {
                            ls.RemoveAt(ls.Count - 1);
                        }
                        catch (Exception e)
                        {
                            var t = e.Message;
                        }
                        continue;
                    }
                    ls.Add(parole.First());
                    continue;
                }

                var d = string.Join("", ls);
                bioutput.WriteLine(d + line);
            }

            bioutput.Flush();
            f1.Dispose();
            f.Dispose();
            f1.Close();
            f.Close();
        }


        public static void Xml2Db(IBonusIdricoRepo bi)
        {
            string line;
            var f = new FileStream(@"c:\users\ciemm\downloads\finaltext", FileMode.Open, FileAccess.ReadWrite);
            StreamReader xml = new StreamReader(f);
            SgateRichieste sgate = null;

            //var nodo = new BICapLotto();
            //nodo.DataAcquisizione = DateTime.Now;
            //nodo.DataCarico = new DateTime(2018, 10, 06);
            //nodo.DataScadenza = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2);
            //nodo.DataInvioEsiti = new DateTime(2018, 10, 06);
            //nodo.RichiesteTotali = 0;
            //nodo.RichiesteAutoVal = 0;
            //nodo.RichiesteVal = 0;
            //nodo.Status = (int)stato.acq;

            //bi.InsertCapLotto(nodo);
            //var idlotto = bi.getRequestLots(null).Last();

            sgate = new SgateRichieste();

            while ((line = xml.ReadLine()) != null)
            {
                if (line == "END")
                {
                    bi.InsertSgateReq(sgate);
                    sgate = new SgateRichieste();
                    continue;
                }

                var item = line.Split(' ');

                sgate.lotCapId = 0;
                sgate.Allineamento = item[0] == "richiestabonusallineamento" ? Convert.ToBoolean(item[1]) : sgate.Allineamento;
                sgate.DataFineAgev = item[0] == "richiestabonusdataFineAgevolazione" ? DateTime.Parse(item[1]) : sgate.DataFineAgev;
                sgate.DataInizioAgev = item[0] == "richiestabonusdataInizioAgevolazione" ? DateTime.Parse(item[1]) : sgate.DataInizioAgev;
                sgate.DataPresentazione = item[0] == "richiestabonusdataPresentazione" ? DateTime.Parse(item[1]) : sgate.DataAmmissione;
                sgate.DataAmmissione = item[0] == "richiestabonusdataAmmissione" ? DateTime.Parse(item[1]) : sgate.DataAmmissione;
                sgate.DataDisponibilita = item[0] == "richiestabonusdataDisponibilita" ? DateTime.Parse(item[1]) : sgate.DataDisponibilita;
                sgate.CompFamigliaAnag = item[0] == "richiestabonuscomponentiFamigliaAnagrafica" ? item[1] : sgate.CompFamigliaAnag;
                sgate.IndCap = item[0] == "richiestaforniturafornituraIndividualeindirizzocap" ? item[1] : sgate.IndCap;
                sgate.IndCivico = item[0] == "richiestaforniturafornituraIndividualeindirizzonumeroCivico" ? item[1] : sgate.IndCivico;
                sgate.IndAreaCirc = item[0] == "richiestaforniturafornituraIndividualeindirizzoareaDiCircolazione" ? item[1] : sgate.IndAreaCirc;
                sgate.IndIstatComune = item[0] == "richiestaforniturafornituraIndividualeindirizzocomune" ? item[1] : sgate.IndIstatComune;
                sgate.IndCf = item[0] == "richiestaforniturafornituraIndividualeintestatariocodiceFiscale" ? item[1] : sgate.IndCf;
                sgate.IndCognome = item[0] == "richiestaforniturafornituraIndividualeintestatariocognome" ? item[1] : sgate.IndCognome;
                sgate.IndNome = item[0] == "richiestaforniturafornituraIndividualeintestatarionome" ? item[1] : sgate.IndNome;
                sgate.CodUtenteInd = item[0] == "richiestaforniturafornituraIndividualecodiceUtente" ? item[1] : sgate.CodUtenteInd;
                sgate.ReqCap = item[0] == "richiestaresidenzacap" ? item[1] : sgate.ReqCap;
                sgate.ReqCivico = item[0] == "richiestaresidenzanumeroCivico" ? item[1] : sgate.ReqCivico;
                sgate.ReqEnteAreaCir = item[0] == "richiestaresidenzaareaDiCircolazione" ? item[1] : sgate.ReqEnteAreaCir;
                sgate.ReqCf = item[0] == "richiestarichiedentecodiceFiscale" ? item[1] : sgate.ReqCf;
                sgate.ReqCognome = item[0] == "richiestarichiedentecognome" ? item[1] : sgate.ReqCognome;
                sgate.ReqNome = item[0] == "richiestarichiedentenome" ? item[1] : sgate.ReqNome;
                sgate.ReqTipoDoc = item[0] == "richiestatipologiaDomanda" ? item[1] : sgate.ReqTipoDoc;
                sgate.ProtRichiesta = item[0] == "richiestaprotocolloRichiesta" ? int.Parse(item[1]) : sgate.ProtDomanda;
                sgate.ProtDomanda = item[0] == "richiestaprotocolloDomanda" ? int.Parse(item[1]) : sgate.ProtDomanda;

                sgate.CentrDenCondominio = item[0] == "richiestaforniturafornituraCentralizzatadenominazioneCondominio" ? item[1] : sgate.CentrDenCondominio;
                sgate.CentrEdificioPlurifam = item[0] == "richiestaforniturafornituraCentralizzataedificioPlurifamiliare" ? item[1] : sgate.CentrEdificioPlurifam; //bool
                sgate.CentrIstatComune = item[0] == "richiestaforniturafornituraCentralizzataindirizziCentralizzataindirizzoFornituraCentralizzatacomune" ? item[1] : sgate.CentrIstatComune;
                sgate.CentrAreaCircolazione = item[0] == "richiestaforniturafornituraCentralizzataindirizziCentralizzataindirizzoFornituraCentralizzataareaDiCircolazione" ? item[1] : sgate.CentrAreaCircolazione;
                sgate.CentrCivico = item[0] == "richiestaforniturafornituraCentralizzataindirizziCentralizzataindirizzoFornituraCentralizzatanumeroCivico" ? item[1] : sgate.CentrCivico;
                sgate.CentrCap = item[0] == "richiestaforniturafornituraCentralizzataindirizziCentralizzataindirizzoFornituraCentralizzatacap" ? item[1] : sgate.CentrCap;

            }

            xml.Dispose();
            xml.Close();


            //VERIFICA E TEST IN ASSENZA DI SCARICO SGATE

            //            var nodiT = nodi.Where(x => x.lotCapId == 0);
            //rilevo data piu vecchia
            //       var dt = nodiT.Where(x => x.Id > 0).OrderBy(x => x.ReqDataDoc).First();

        }
    }
}

