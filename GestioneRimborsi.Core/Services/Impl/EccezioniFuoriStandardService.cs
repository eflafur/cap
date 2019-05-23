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
    public class EccezioniFuoriStandardService : IEccezioniFuoriStandardService
    {
        IEccezioniFuoriStandardRepo _eccezioniFuoriStandardRepo = null;

        public EccezioniFuoriStandardService(IEccezioniFuoriStandardRepo EccezioniFuoriStandardRepo)
        {
            _eccezioniFuoriStandardRepo = EccezioniFuoriStandardRepo;
        }

        public bool CheckException(int codStandard, string tipoStandard)
        {
            return _eccezioniFuoriStandardRepo.CheckException(codStandard, tipoStandard);
        }       
    }
}
