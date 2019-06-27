using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap.Core;
using GruppoCap.Core.Data;

namespace GestioneRimborsi.Core
{
    public interface IAllegatoRimborsoService : IRevoService
    {
        ISubCollection<AllegatoRimborso> GetElencoDocumenti(String AnnoDocumento, String NumeroDocumento);
        String AggiungiFile(System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String AnnoDocumento, String NumeroDocumento, String FileDescription, String Utente);
        bool DeleteFile(String NomeFile, String ServerPath, String TipoFile);
    }
}
