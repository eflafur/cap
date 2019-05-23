using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestioneRimborsi.Core;
using PetaPoco;

namespace GestioneRimborsi.Core
{
    public class RimborsoService : IRimborsoService
    {
        IRimborsoRepo _rimborsoRepo = null;

        public RimborsoService(IRimborsoRepo RimborsoRepo)
        {
            _rimborsoRepo = RimborsoRepo;
        }

        public GestioneRimborso GetRimborso(String CodCliente, String Utente, Int16 Anno, String NumeroDocumento)
        {
            return _rimborsoRepo.GetRimborso(CodCliente, Utente, Anno, NumeroDocumento);
        }
        public ISubCollection<GestioneRimborso> GetElencoRimborsi(String CodCliente, String Utente)
        {
            return _rimborsoRepo.GetElencoRimborsi(CodCliente, Utente);
        }
        public ISubCollection<GestioneRimborso> GetRimborsiFiltered(String CodCliente, String Utente, List<String> Permission)
        {
            return _rimborsoRepo.GetRimborsiFiltered(CodCliente, Utente, Permission);
        }
        public ISubCollection<GestioneRimborso> GetRimborsiConfermabili(String Utente, bool Amministratore)
        {
            return _rimborsoRepo.GetRimborsiConfermabili(Utente, Amministratore);
        }

        public GestioneRimborso GetRimborsoTestata(String AnnoDocumento,String NumeroDocumento)
        {
            return _rimborsoRepo.GetRimborsoTestata(AnnoDocumento, NumeroDocumento);
        }

        public ISubCollection<DettaglioRimborso> GetRimborsoDettaglio(String AnnoDocumento, String NumeroDocumento)
        {
            return _rimborsoRepo.GetRimborsoDettaglio(AnnoDocumento,NumeroDocumento );
        }

        List<string> IRimborsoService.UsersOfRimborsi()
        {
            return _rimborsoRepo.UsersOfRimborsi();
        }

        public ISubCollection<GestioneRimborso> GetRimborsiAnnullabili(String Utente, bool Amministratore)
        {
            return _rimborsoRepo.GetRimborsiAnnullabili(Utente, Amministratore);
        }

        public String GetTipoRimborso(String TipoRimborso)
        {
            String result = String.Empty;
            if (!String.IsNullOrEmpty(TipoRimborso))
            {
                if (TipoRimborso.Equals("ASS", StringComparison.InvariantCultureIgnoreCase))
                    return result = "Assegno";
                else if (TipoRimborso.Equals("BON", StringComparison.InvariantCultureIgnoreCase))
                    return result = "Bonifico";
                else if (TipoRimborso.Equals("PRB", StringComparison.InvariantCultureIgnoreCase))
                    return result = "Prossima bolletta";
                else if (TipoRimborso.Equals("BOD", StringComparison.InvariantCultureIgnoreCase))
                    return result = "Bonifico in circolarita";
            }
            return result;
        }

        public String GetTipoDocumento(String TipoDocumento)
        {
            String result = String.Empty;
            if (!String.IsNullOrEmpty(TipoDocumento))
            {
                if (TipoDocumento.Equals("NACC", StringComparison.InvariantCultureIgnoreCase))
                    return result = "NOTA DI ACCREDITO";
                else if (TipoDocumento.Equals("BNEG", StringComparison.InvariantCultureIgnoreCase))
                    return result = "BOLLETTA NEGATIVA";
                else if (TipoDocumento.Equals("PGEN", StringComparison.InvariantCultureIgnoreCase))
                    return result = "PAGAMENTO ECCEDENTE";
                else if (TipoDocumento.Equals("BONU", StringComparison.InvariantCultureIgnoreCase) || TipoDocumento.Equals("BIIN", StringComparison.InvariantCultureIgnoreCase))
                    return result = "BONUS IDRICO";
                else if (TipoDocumento.Equals("INDE", StringComparison.InvariantCultureIgnoreCase))
                    return result = "INDENNIZZO";
            }
            return result;
        }

        public bool AnnullaRimborso(List<String> ClienteAnnoNumeroDocumento, String Utente)
        {
            return _rimborsoRepo.AnnullaRimborso(ClienteAnnoNumeroDocumento, Utente);
        }

        public ISubCollection<GestioneRimborso> GetRimborsiTestataByClienteAnnoNumeroDocumento(List<string> ClienteAnnoNumeroDocumento, string Utente)
        {
            return _rimborsoRepo.GetRimborsiTestataByClienteAnnoNumeroDocumento(ClienteAnnoNumeroDocumento, Utente);
        }
        public String GetStatoDocumento(String StatoDocumento)
        {
            String result = String.Empty;
            if (StatoDocumento == "0")
                return result = "Bozza";
            else if (StatoDocumento == "1")
                return result = "Da Confermare";
            else if (StatoDocumento == "2")
                return result = "Confermata";
            return result;
        }

        public Double GetTotaleRimborso(String ImportoBolletta, String ImportoPagato, String ImpRimbNac, String ImpRimbBneg, String ImpRimbPagEcc)
        {
            Double tot = (Convert.ToDouble(ImportoBolletta) - (Convert.ToDouble(ImportoPagato) + Convert.ToDouble(ImpRimbNac) + Convert.ToDouble(ImpRimbBneg) + Convert.ToDouble(ImpRimbPagEcc)));
            return tot;
        }

        public ISubCollection<GestioneRimborso> GetRimborsiConfermati(String CodCliente, String Utente)
        {
            return _rimborsoRepo.GetRimborsiConfermati(CodCliente, Utente);
        }

        public String ConfermaRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente, String NumeroProtocollo, String UtenteProtocollo, String DataProtocollo)
        {
            return _rimborsoRepo.ConfermaRimborsi(ClienteAnnoNumeroDocumento, Utente, NumeroProtocollo, UtenteProtocollo, DataProtocollo);
        }

        public string SalvaRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito, RecapitoClienteRimborso Cliente)
        {

            if (RimborsoNativo.ImportoTotaleRimborso != RimborsoGestito.RigheRimborso.Sum(x => x.Importo))
            {
                throw new ApplicationException("Importo rimborso non coincidente con importo gestito");
            }
            return _rimborsoRepo.RegistraRimborso(RimborsoNativo, RimborsoGestito, Cliente);
        }

        public string InserisciRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito)
        {
            if (RimborsoNativo.ImportoTotaleRimborso != RimborsoGestito.RigheRimborso.Sum(x => x.Importo))
            {
                throw new ApplicationException("Importo rimborso non coincidente con importo gestito");
            }
            return _rimborsoRepo.InserisciRimborso(RimborsoNativo, RimborsoGestito);
        }

        public ISubCollection<GestioneRimborso> GetRimborsiRistampaMassiva(DateTime DataInizio, DateTime DataFine, String Utente)
        {
            return _rimborsoRepo.GetRimborsiRistampaMassiva(DataInizio, DataFine, Utente);
        }

        public ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento)
        {
            return _rimborsoRepo.GetElencoDocumenti(AnnoDocumento, NumeroDocumento);
        }

        public String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente)
        {
            return _rimborsoRepo.AggiungiFile(file, NomefileOriginale, Extension, ServerPath, AnnoDocumento, NumeroDocumento, FileDescription, Utente);
        }

        public bool DeleteFile(String NomeFile, String ServerPath, String TipoFile)
        {
            return _rimborsoRepo.DeleteFile(NomeFile, ServerPath, TipoFile);
        }

        public String GetCodicePuntoFornitura (String CodiceCliente, String NumeroDocumento, String AnnoDocumento)
        {
            return _rimborsoRepo.GetCodicePuntoFornitura(CodiceCliente, NumeroDocumento, AnnoDocumento);
        }
        public ISubCollection<GestioneRimborso> GetTestataBozzaRimborsi(List<string> ClienteAnnoNumeroDocumento, string Utente)
        {
            return _rimborsoRepo.GetTestataBozzaRimborsi(ClienteAnnoNumeroDocumento, Utente);
        }
        public String GetManagerInfo(String CodGruppo)
        {
            return _rimborsoRepo.GetManagerInfo(CodGruppo);
        }
        public String GetManagerMail_Iban(String CodGruppo)
        {
            return _rimborsoRepo.GetManagerMail_Iban(CodGruppo);
        }
        public ClienteBonusIdrico GetClienteBonusIdricoRimborsi(string CodiceCliente)
        {
            return _rimborsoRepo.GetClienteBonusIdricoRimborsi(CodiceCliente);
        }
    }
}
