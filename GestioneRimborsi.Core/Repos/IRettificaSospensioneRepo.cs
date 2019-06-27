using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IRettificaSospensioneRepo : IRepository<RettificaSospensione>
    {
        ISubCollection<RettificaSospensione> GetSospensioniByIDFuoriStandard(String FuoriStandardID, bool edit);
        RettificaSospensione GetSospensioneByID(String RowID);
        IInsertOperationResult AggiornaSospensione(RettificaSospensione sospensione);
        List<CategoriaSospensione> GetCategorieSospensione(String NumeroPrestazione);
        List<TipoSospensione> GetTipiSospensione(String CodCategoria);
        IUpdateOperationResult AggiornaDurataSospensioni(List<RettificaSospensione> sospensioni);
        void RipristinaSospensioniCancellate(String idFuoriStandard);
        void AnnullaSospensione(int idIndennizzo, long idSospensione, long rowID);
        RettificaSospensione GetSospensioneCategoria(String RowID);
        RettificaSospensione GetSospensioneModifica(String RowID);
        void ConfermaSospensioniInLavorazione(int idIndennizzo);
        void SospendiModifiche(int idIndennizzo);
        ISubCollection<AllegatoSospensione> GetElencoAllegatiSosp(long IdSospensione);
        bool DeleteAllegatoSosp(String NomeFile, String ServerPath, String TipoFile);
        String AggiungiFileSosp(long IdSospensione, String IdFs, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente);
    }
}
