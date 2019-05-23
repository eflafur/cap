using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IFuoriStandardAllegatoService : IRevoService
    {
        ISubCollection<FuoriStandardAllegato> GetElencoAllegati(String IdFS);
        bool DeleteAllegato(String NomeFile, String ServerPath, String TipoFile);
    }
}
