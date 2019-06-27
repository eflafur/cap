using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IRimborsoRepo : IRepository<GestioneRimborso>
    {
        void SetSharedDatabase(PetaPoco.Database sharedDb);
        GestioneRimborso GetRimborso(String CodCliente, String Utente, Int16 Anno, String NumeroDocumento);
        ISubCollection<GestioneRimborso> GetElencoRimborsi(String CodCliente, String Utente);
        ISubCollection<GestioneRimborso> GetRimborsiFiltered(String CodCliente, String Utente, List<String> Permission);
        ISubCollection<GestioneRimborso> GetRimborsiConfermabili(String Utente, bool Amministratore);       
        GestioneRimborso GetRimborsoTestata(String AnnoDocumento,String NumeroDocumento);
        ISubCollection<DettaglioRimborso> GetRimborsoDettaglio(String AnnoDocumento, String NumeroDocumento);
        List<string> UsersOfRimborsi();
        ISubCollection<GestioneRimborso> GetRimborsiAnnullabili(String Utente, bool Amministratore);
        ISubCollection<GestioneRimborso> GetRimborsiConfermati(String CodCliente, String Utente);
        bool AnnullaRimborso(List<String> ClienteAnnoNumeroDocumento, String Utente);
        ISubCollection<GestioneRimborso> GetRimborsiTestataByClienteAnnoNumeroDocumento(List<string> ClienteAnnoNumeroDocumento, string Utente);

        String ConfermaRimborsi(List<String> ClienteAnnoNumeroDocumento, String Utente, String NumeroProtocollo, String UtenteProtocollo, String DataProtocollo);
        string RegistraRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito, RecapitoClienteRimborso Cliente);
        string InserisciRimborso(GestioneRimborso RimborsoNativo, RimborsoGestito RimborsoGestito);
        ISubCollection<GestioneRimborso> GetRimborsiRistampaMassiva(DateTime DataInizio, DateTime DataFine, String Utente);
        ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento);
        String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente);
        bool DeleteFile(String NomeFile, String ServerPath, String TipoFile);
        String GetCodicePuntoFornitura(String CodiceCliente, String NumeroDocumento, String AnnoDocumento);
        ISubCollection<GestioneRimborso> GetTestataBozzaRimborsi(List<string> ClienteAnnoNumeroDocumento, string Utente);
        String GetManagerInfo(String CodGruppo);
        ClienteBonusIdrico GetClienteBonusIdricoRimborsi(string CodiceCliente);
        String GetManagerMail_Iban(String codGruppo);
    }
}
