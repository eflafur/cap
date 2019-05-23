using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core
{
    public class RettificaSospensioneService : IRettificaSospensioneService
    {
        IRettificaSospensioneRepo _rettificaSospensioneRepo = null;

        public RettificaSospensioneService(IRettificaSospensioneRepo RettificaSospensioneRepo)
        {
            _rettificaSospensioneRepo = RettificaSospensioneRepo;
        }

        public RettificaSospensione GetSospensioneByID(String RowID)
        {
            return _rettificaSospensioneRepo.GetSospensioneByID(RowID);
        }
        public ISubCollection<RettificaSospensione> GetSospensioniByIDFuoriStandard(String FuoriStandardID, bool edit)
        {
            return _rettificaSospensioneRepo.GetSospensioniByIDFuoriStandard(FuoriStandardID, edit);
        }
        public IInsertOperationResult AggiornaSospensione(RettificaSospensione sospensione)
        {
            return _rettificaSospensioneRepo.AggiornaSospensione(sospensione);
        }
        public List<CategoriaSospensione> GetCategorieSospensione(String NumeroPrestazione)
        {
            return _rettificaSospensioneRepo.GetCategorieSospensione(NumeroPrestazione);
        }
        public List<TipoSospensione> GetTipiSospensione(String CodCategoria)
        {
            return _rettificaSospensioneRepo.GetTipiSospensione(CodCategoria);
        }
        public IUpdateOperationResult AggiornaDurataSospensioni(List<RettificaSospensione> sospensioni)
        {
            return _rettificaSospensioneRepo.AggiornaDurataSospensioni(sospensioni);
        }
        public void RipristinaSospensioniCancellate(String idFuoriStandard)
        {
            _rettificaSospensioneRepo.RipristinaSospensioniCancellate(idFuoriStandard);
        }
        public void AnnullaSospensione(int idIndennizzo, long idSospensione, long rowID)
        {
            _rettificaSospensioneRepo.AnnullaSospensione(idIndennizzo, idSospensione, rowID);
        }
        public RettificaSospensione GetSospensioneCategoria(String RowID)
        {
            return _rettificaSospensioneRepo.GetSospensioneCategoria(RowID);
        }
        public RettificaSospensione GetSospensioneModifica(String RowID)
        {
            return _rettificaSospensioneRepo.GetSospensioneModifica(RowID);
        }
        public void ConfermaSospensioniInLavorazione(int idIndennizzo)
        {
            _rettificaSospensioneRepo.ConfermaSospensioniInLavorazione(idIndennizzo);
        }
        public void SospendiModifiche(int idIndennizzo)
        {
            _rettificaSospensioneRepo.SospendiModifiche(idIndennizzo);
        }
        public ISubCollection<AllegatoSospensione> GetElencoAllegatiSosp(long IdSospensione)
        {
            return _rettificaSospensioneRepo.GetElencoAllegatiSosp(IdSospensione);
        }
        public bool DeleteAllegatoSosp(String NomeFile, String ServerPath, String TipoFile)
        {
            return _rettificaSospensioneRepo.DeleteAllegatoSosp(NomeFile, ServerPath, TipoFile);
        }
        public String AggiungiFileSosp(long IdSospensione, String IdFs, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente)
        {
            return _rettificaSospensioneRepo.AggiungiFileSosp(IdSospensione, IdFs, file, NomefileOriginale, Extension, ServerPath, Utente);
        }
    }
}
