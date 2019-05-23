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
    public class EccezioniFuoriStandardRepo : RepositoryBase<EccezioneFuoriStandard>, IEccezioniFuoriStandardRepo
    {
        public bool CheckException(int codStandard, string tipoStandard)
        {
            int _rows = 0;
            try
            {
                var sql = Sql.Builder.Append("select count(*) from ECCEZIONI_FUORI_STANDARD where codice_standard = @0 and tipo_standard = @1", codStandard, tipoStandard);
                _rows = db.FirstOrDefault<int>(sql);
                return true ? _rows > 0 : false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in EccezioniFuoriStandard->CheckException: " + ex.Message);
            }
        }
    }
}
