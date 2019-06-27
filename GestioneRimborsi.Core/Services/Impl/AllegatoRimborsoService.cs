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
    public class AllegatoRimborsoService : IAllegatoRimborsoService
    {
        IAllegatoRimborsoRepo _allegatoRimborsoRepo = null;

        public AllegatoRimborsoService(IAllegatoRimborsoRepo AllegatoRimborsoRepo)
        {
            _allegatoRimborsoRepo = AllegatoRimborsoRepo;
        }
        public ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento)
        {
            return _allegatoRimborsoRepo.GetElencoDocumenti(AnnoDocumento, NumeroDocumento);
        }

        public String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente)
        {
            return _allegatoRimborsoRepo.AggiungiFile(file, NomefileOriginale, Extension, ServerPath, AnnoDocumento, NumeroDocumento, FileDescription, Utente);
        }

        public bool DeleteFile(String NomeFile, String ServerPath, String TipoFile)
        {
            return _allegatoRimborsoRepo.DeleteFile(NomeFile, ServerPath, TipoFile);
        }
    }
}
