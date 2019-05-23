using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IAnniBloccatiRepo : IRepository<AnnoBloccato>
    {
        ISubCollection<AnnoBloccato> GetAnniBloccati();
        AnnoBloccato GetAnnoBloccato();
        void InsertDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente);
        void UpdateDataBlocco(string annoCompetenza, DateTime dataBlocco, String utente);
        AnnoBloccato GetLastAnnoBloccato();
        bool CheckIsBlocked(String idFuoriStandard);
        bool CheckIsBlockedApprovazione(String idFuoriStandard, String ErrDataInizio, String ErrDataFine);
    }
}
