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
    public class FuoriStandardAllegatoService : IFuoriStandardAllegatoService
    {
        IFuoriStandardAllegatoRepo _fuoriStandardAllegatoRepo = null;

        public FuoriStandardAllegatoService(IFuoriStandardAllegatoRepo FuoriStandardAllegatoRepo)
        {
            _fuoriStandardAllegatoRepo = FuoriStandardAllegatoRepo;
        }

        public ISubCollection<FuoriStandardAllegato> GetElencoAllegati(String IdFS)
        {
            return _fuoriStandardAllegatoRepo.GetElencoAllegati(IdFS);
        }
        public bool DeleteAllegato(String NomeFile, String ServerPath, String TipoFile)
        {
            return _fuoriStandardAllegatoRepo.DeleteAllegato(NomeFile, ServerPath, TipoFile);
        }
    }
}
