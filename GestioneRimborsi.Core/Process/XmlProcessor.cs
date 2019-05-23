using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GestioneRimborsi.Core.xsd;
using System.IO;
using System.Web;
using System.Xml;
using GestioneRimborsi.Core;
namespace GestioneRimborsi.Core.Process
{
    public class XmlProcessor
    {

        protected class errorCodeDescription
        {
            public int id { get; set; }
            public string description { get; set; }
        }

        public void ReadRequests(IBonusIdricoRepo bi, DateTime DataAcquisizione, DateTime DataCarico, DateTime DataScadenza, string Desc, string filePath)
        {
            tipoFileRichiesteSgate reqs = DeserializeRequests(filePath);

            Requests2Db(bi, DataAcquisizione, DataCarico, DataScadenza, Desc, reqs);
        }

        private void Requests2Db(IBonusIdricoRepo bi, DateTime DataAcquisizione, DateTime DataCarico, DateTime DataScadenza, string Desc, tipoFileRichiesteSgate requests)
        {
            // Create a new Lot from scratch
            var nodo = new BICapLotto();
            nodo.DataAcquisizione = DataAcquisizione;
            nodo.DataCarico = DataCarico; //G.Z.: Is it correct?? Hardcoded?!? /FAHD / la data in cui hai preso in carico la richiesta
            nodo.DataScadenza = DataScadenza;//new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2);
                                             //       nodo.DataInvioEsiti = new DateTime(2018, 10, 06); //G.Z.: Is it correct?? Hardcoded?!? /FAHD /  la dobbiamo valorizzare quando si fa l'invio 
            nodo.Desc = Desc;
            nodo.RichiesteAutoVal = 0;
            nodo.RichiesteVal = 0;
            nodo.Status = (int)stato.acq;
            nodo.RichiesteTotali = requests.messaggio.ToList().Count();

            //Insert into corresponding Lot Db Table
            //Get the last sequence number generated on <-- ERROR. Use the default PETAPOCO facilities (returns new ID)
            var idlotto = bi.InsertCapLotto(nodo);
            //Cycle over each request
            var count = 0;
            requests.messaggio.ToList().ForEach(x =>
            {
                //Prepare an object to perform request persistance
                var sgate = new SgateRichieste();

                //EA: it makes no sense in using a whole entity, when you need only the ID.
                sgate.lotCapId = idlotto;

                //General parameters
                //TODO: Manage "tipoSegnalazioneCessazione"  --NO e da verifcare 

                if (x.Item is tipoRichiestaIdrico)
                {
                    var richiesta = (tipoRichiestaIdrico)x.Item;

                    sgate.Allineamento = richiesta.bonus.allineamento;
                    sgate.DataFineAgev = richiesta.bonus.dataFineAgevolazione;
                    sgate.DataInizioAgev = richiesta.bonus.dataInizioAgevolazione;
                    sgate.DataPresentazione = richiesta.bonus.dataPresentazione;
                    sgate.DataAmmissione = richiesta.bonus.dataAmmissione;
                    sgate.DataDisponibilita = richiesta.bonus.dataDisponibilita;
                    sgate.CompFamigliaAnag = richiesta.bonus.componentiFamigliaAnagrafica.ToString("N0");
                    sgate.FruizioneCont = richiesta.bonus.fruizioneContinuativa;
                    sgate.ReqCap = richiesta.residenza.cap;
                    sgate.ReqCivico = richiesta.residenza.numeroCivico;
                    sgate.ReqEnteAreaCir = richiesta.residenza.areaDiCircolazione;
                    sgate.ReqCf = richiesta.richiedente.codiceFiscale;
                    sgate.ReqCognome = richiesta.richiedente.cognome;
                    sgate.ReqNome = richiesta.richiedente.nome;
                    sgate.TipoDomanda = richiesta.tipologiaDomanda;

                    if (richiesta.richiedente.documentoRiconoscimento != null)
                    {
                        if (richiesta.richiedente.documentoRiconoscimento.tipo != null)
                            sgate.ReqTipoDoc = richiesta.richiedente.documentoRiconoscimento.tipo.ToString();

                        sgate.ReqNumeroDoc = richiesta.richiedente.documentoRiconoscimento.numero;
                        sgate.ReqEnteRilsascioDoc = richiesta.richiedente.documentoRiconoscimento.enteRilascio;
                        sgate.ReqDataDoc = richiesta.richiedente.documentoRiconoscimento.data;
                    }
                    sgate.ProtRichiesta = Convert.ToInt32(richiesta.protocolloRichiesta);
                    sgate.ProtDomanda = Convert.ToInt32(richiesta.protocolloDomanda);

                    //Specific
                    var fornituraIndividuale = richiesta.fornitura.Item as tipoFornituraIndividualeIdrico;
                    if (fornituraIndividuale != null)
                    {
                        sgate.IndCap = fornituraIndividuale.indirizzo.cap;
                        sgate.IndCivico = fornituraIndividuale.indirizzo.numeroCivico;
                        sgate.IndAreaCirc = fornituraIndividuale.indirizzo.areaDiCircolazione;
                        sgate.IndIstatComune = fornituraIndividuale.indirizzo.comune;
                        sgate.IndCf = fornituraIndividuale.intestatario.codiceFiscale;
                        sgate.IndCognome = fornituraIndividuale.intestatario.cognome;
                        sgate.IndNome = fornituraIndividuale.intestatario.nome;
                        sgate.CodUtenteInd = fornituraIndividuale.codiceUtente;
                    }

                    var fornituraCentralizzata = richiesta.fornitura.Item as tipoFornituraCentralizzataIdrico;
                    if (fornituraCentralizzata != null)
                    {
                        sgate.CentrDenCondominio = fornituraCentralizzata.denominazioneCondominio;
                        sgate.CentrEdificioPlurifam = fornituraCentralizzata.edificioPlurifamiliare ? "S" : "N";
                        sgate.CodUtenteCentr = fornituraCentralizzata.codiceUtente;
                        sgate.CentrIban = fornituraCentralizzata.codiceIban;

                        var single = fornituraCentralizzata.indirizziCentralizzata.FirstOrDefault(); //Why a list??? TODO: Investigate please.
                        if (single != null)
                        {
                            sgate.CentrIstatComune = single.comune;
                            sgate.CentrAreaCircolazione = single.areaDiCircolazione;
                            sgate.CentrCivico = single.numeroCivico;
                            sgate.CentrCap = single.cap;
                        }
                    }
                }
                else if (x.Item is tipoSegnalazioneCessazione)
                {
                    //TODO; Manager this request type!!!!
                }

                ++count;
                bi.InsertSgateReq(sgate);
            });

        }


        public string CreateOutcomeFile(IBonusIdricoRepo bi, int lotId)
        {
            string codeUtente = null;
            BIRequestValidate reqVal = null;
            List<string> okCodes = new List<string> { "OK", "OK*", "OK**" };
            Dictionary<string, errorCodeDescription> errorCodes = new Dictionary<string, errorCodeDescription>()
            {
                {  "KO1", new errorCodeDescription{id =1, description="La fornitura non risulta presente nella rete del gestore."}},
                {  "KO2", new errorCodeDescription{id =2 , description="La fornitura non risulta attiva."}},
                {  "KO3", new errorCodeDescription{id =3 , description="Il CF del richiedente non coincide con quanto presente nella banca dati del gestore per la fornitura da agevolare."}},
                {  "KO4", new errorCodeDescription{id =4 , description="La tipologia di tariffa associata alla fornitura è uso diverso dal domestico o usi condominiali."}},
                {  "KO5", new errorCodeDescription{id =5 , description="La tipologia di tariffa associata alla fornitura è uso domestico non residente."}},
                {  "KO6", new errorCodeDescription{id =6 , description="Gli indirizzi di fornitura dichiarati dal richiedente e trasmessi da SGAte non corrispondono ad una fornitura centralizzata presente nella banca dati del gestore."}},
                {  "KO7", new errorCodeDescription{id =7 , description="La tipologia di tariffa associata alla fornitura non è riconducibile ad una utenza condominiale."}}
            };



            //Be aware: This is a security breach!
            var requestByLot = bi.getOriginalSGATeRequests(lotId);
            //Pay attention this is only a subset!
            var countOutcomes = requestByLot.Count();

            tipoFileEsitiGestore esiti = new tipoFileEsitiGestore();

            var listOutcome = new List<tipoEsitoMessaggio>();

            requestByLot.ToList().ForEach(h =>
            {
                var boundOutcome = bi.getCapRequestById(h.Id);
                bool okRequest = okCodes.Where(ee => ee.Equals(boundOutcome.EsitoManVal)).FirstOrDefault() != null;

                var outComeType = new tipoEsitoMessaggio();
                //TODO: Manage other type than "tipoEsitoAgevolazione" 
                var outcome = new tipoEsitoAgevolazione();
                //Mapping

                //TODO: manage all types 
                /*"tipoDifformita"          -> CANNOT BE MANAGED AT THE MOMENT
                 * "tipoErrore"             -> WILL NOT BE MANAGED
                 * "tipoCodiceDettaglio"    -> MANAGED
                 */
                if (okRequest)
                {

                    // if i have one between ok, ok*, ok** this is a positive outcome
                    //            outcome.esito = "OK";
                    //          modifica e sostituisce l'istruzione sopra assegnando all'esito il valore riscontrato nella validazione come da richiesta CAP
                    //                    outcome.esito = boundOutcome.EsitoManVal;
                    outcome.esito = "OK";
                    var dissimList = new List<tipoDifformita>();
                    var dissimalirity = new tipoDifformita();
                    // dissimalirity.anagraficaDifforme = new tipoDifformitaAnagrafica() { componentiFamigliaAnagrafica = 3, dataAggiornamento = DateTime.Today }; //TODO Use real data from DB this is a Mock!
                    int peopleCheckFlag;

                    bool addDissimilarity = false;
                    //add code difformity
                    var utenteSgate = bi.getOriginalSGATeRequestById(h.Id);
                    if (boundOutcome.TipoRichiesta == "INDIVIDUALE")
                    {
                        codeUtente = string.Format("000{0}", utenteSgate.CodUtenteInd) == boundOutcome.codCliente ? "0" : boundOutcome.codCliente;
                        if (codeUtente != "0")
                        {
                            addDissimilarity = true;
                            reqVal = bi.GetRequestValidateById(h.Id);
                            dissimalirity.codiceUtenteDifforme = reqVal.FornPointCheckFlag == 1 ? codeUtente : "1";
                        }
                        if (addDissimilarity && dissimalirity.codiceUtenteDifforme != "1")
                            dissimList.Add(dissimalirity);
                        if (addDissimilarity)
                            peopleCheckFlag = reqVal.PeopleCheckFlag;
                    }

                    if (dissimList.Count > 0)
                        outcome.Items = dissimList.ToArray();
                }
                else
                { //otherwise this is a KOed request.
                    outcome.esito = "KO";
                    var koList = new List<tipoCodiceDettaglio>();

                    var splittedOutcomes = boundOutcome.EsitoManVal.Split(';');
                    splittedOutcomes.ToList().ForEach(item =>
                    {
                        var ec = errorCodes.Where(ee => ee.Key.Equals(item)).First();
                        koList.Add(new tipoCodiceDettaglio()
                        {
                            codice = ec.Value.id,
                            dettaglio = ec.Value.description
                        });
                    });
                    outcome.Items = koList.ToArray();
                }
                outcome.protocolloRichiesta = h.ProtRichiesta.ToString();
                outComeType.Item = outcome;

                listOutcome.Add(outComeType);
            });

            esiti.esitoMessaggio = listOutcome.ToArray();

            return SerializeOutcome(esiti, lotId, bi);
        }

        private tipoFileRichiesteSgate DeserializeRequests(string filePath)
        {
            //// Create an instance of the XmlSerializer.
            XmlSerializer serializer = new XmlSerializer(typeof(tipoFileRichiesteSgate));

            // Declare an object variable of the type to be deserialized.
            tipoFileRichiesteSgate i;

            using (Stream reader = new FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                i = (tipoFileRichiesteSgate)serializer.Deserialize(reader);
            }

            return i;
        }

        private string SerializeOutcome(tipoFileEsitiGestore outcomeInstance, int lotId, IBonusIdricoRepo bi)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(typeof(tipoFileEsitiGestore));
            //TODO: Parametrize this
            //     var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/downloads"), string.Format("outc-{0}.xml", lotId));
            var lotto = bi.getRequestLotById(lotId);
            var data = DateTime.Today.ToString("ddMMyyyy");
            var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/downloads"), string.Format("{0}-{1}-{2}.xml", data, lotto.Desc, lotId));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, outcomeInstance);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(path);
            }

            return path;
        }
    }
}
