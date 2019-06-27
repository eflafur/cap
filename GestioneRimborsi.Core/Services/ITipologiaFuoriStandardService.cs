using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface ITipologiaFuoriStandardService : IRevoService
    {
        List<String> GetTipologieByGrouping(List<String> Grouping);
        ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard);
        List<String> GetTipologieFilter();
        TipologiaFuoriStandard GetTipologiaStandard(Int32 IdStandard);
    }
}
