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
    public class AnniBloccatiService : IAnniBloccatiService
    {
        IAnniBloccatiRepo _anniBloccatiRepo = null;

        public AnniBloccatiService(IAnniBloccatiRepo AnniBloccatiRepo)
        {
            _anniBloccatiRepo = AnniBloccatiRepo;
        }
        public ISubCollection<AnnoBloccato> GetAnniBloccati()
        {
            return _anniBloccatiRepo.GetAnniBloccati();
        }

        public AnnoBloccato GetAnnoBloccato()
        {
            return _anniBloccatiRepo.GetAnnoBloccato();
        }

        public void InsertDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente)
        {
            _anniBloccatiRepo.InsertDataBlocco(annoCompetenza, dataBlocco, utente);
        }

        public void UpdateDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente)
        {
            _anniBloccatiRepo.UpdateDataBlocco(annoCompetenza, dataBlocco, utente);
        }

        public AnnoBloccato GetLastAnnoBloccato()
        {
            return _anniBloccatiRepo.GetLastAnnoBloccato();
        }

        public bool CheckIsBlocked(String idFuoriStandard)
        {
            return _anniBloccatiRepo.CheckIsBlocked(idFuoriStandard);
        }

        public bool CheckIsBlockedApprovazione(String idFuoriStandard, String ErrDataInizio, String ErrDataFine)
        {
            return _anniBloccatiRepo.CheckIsBlockedApprovazione(idFuoriStandard, ErrDataInizio, ErrDataFine);
        }
    }
}
