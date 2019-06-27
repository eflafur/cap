using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IRimborsoService : IRevoService
    {
        GestioneRimborso GetRimborso(String CodCliente, String Utente, Int16 Anno, String NumeroDocumento);
        ISubCollection<GestioneRimborso> GetElencoRimborsi(String CodCliente, String Utente);
        ISubCollection<GestioneRimborso> GetRimborsiFiltered(String CodCliente, String Utente, List<String> Permission);
        ISubCollection<GestioneRimborso> GetRimborsiConfermabili(String Utente, bool Amministratore);
        GestioneRimborso GetRimborsoTestata(String AnnoDocumento, String NumeroDocumento);
        ISubCollection<DettaglioRimborso> GetRimborsoDettaglio(String AnnoDocumento, String NumeroDocumento);
        List<string> UsersOfRimborsi();
        ISubCollection<GestioneRimborso> GetRimborsiAnnullabili(String Utente, bool Amministratore);
        bool AnnullaRimborso(List<String> ClienteAnnoNumeroDocumento, String Utente);
        String GetTipoRimborso(String TipoRimborso);
        String GetTipoDocumento(String TipoDocumento);
        String GetStatoDocumento(String StatoDocumento);
        Double GetTotaleRimborso(String ImportoBolletta, String ImportoPagato, String ImpRimbNac, String ImpRimbBneg, String ImpRimbPagEcc);
        ISubCollection<GestioneRimborso> GetRimborsiConfermati(String CodCliente, String Utente);
        String ConfermaRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente, String NumeroProtocollo, String UtenteProtocollo, String DataProtocollo);
        string SalvaRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito, RecapitoClienteRimborso Cliente);
        string InserisciRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito);
        ISubCollection<GestioneRimborso> GetRimborsiTestataByClienteAnnoNumeroDocumento(List<string> ClienteAnnoNumeroDocumento, string Utente);
        ISubCollection<GestioneRimborso> GetRimborsiRistampaMassiva(DateTime DataInizio, DateTime DataFine, String Utente);
        ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento);
        String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente);
        bool DeleteFile(String NomeFile, String ServerPath, String TipoFile);
        String GetCodicePuntoFornitura(String CodiceCliente, String NumeroDocumento, String AnnoDocumento);
        ISubCollection<GestioneRimborso> GetTestataBozzaRimborsi(List<string> ClienteAnnoNumeroDocumento, string Utente);
        String GetManagerInfo(String CodGruppo);
        ClienteBonusIdrico GetClienteBonusIdricoRimborsi (string CodiceCliente);
        String GetManagerMail_Iban(String mainGroupingCode);
    }
}
