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
    public class TipologiaFuoriStandardService : ITipologiaFuoriStandardService
    {
        ITipologiaFuoriStandardRepo _tipologiaFuoriStandardRepo = null;

        public TipologiaFuoriStandardService(ITipologiaFuoriStandardRepo TipologiaFuoriStandardRepo)
        {
            _tipologiaFuoriStandardRepo = TipologiaFuoriStandardRepo;
        }

        public List<String> GetTipologieByGrouping(List<String> Grouping)
        {
            return _tipologiaFuoriStandardRepo.GetTipologieByGrouping(Grouping);
        }

        public ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard)
        {
            return _tipologiaFuoriStandardRepo.GetTipologieDesc(CodStandard);
        }

        public List<String> GetTipologieFilter()
        {
            return _tipologiaFuoriStandardRepo.GetTipologieFilter();
        }

        public TipologiaFuoriStandard GetTipologiaStandard(Int32 IdStandard)
        {
            return _tipologiaFuoriStandardRepo.GetTipologiaStandard(IdStandard);
        }
    }
}
