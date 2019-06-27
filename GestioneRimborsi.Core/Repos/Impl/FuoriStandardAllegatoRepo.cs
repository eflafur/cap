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
    public class FuoriStandardAllegatoRepo : RepositoryBase<FuoriStandardAllegato>, IFuoriStandardAllegatoRepo
    {
        public ISubCollection<FuoriStandardAllegato> GetElencoAllegati(String IdFS)
        {
            ISubCollection<FuoriStandardAllegato> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gri_fuori_standard_allegati where IDFS = @0", IdFS);
                _list = db.Query<FuoriStandardAllegato>(sql).ToSubCollection<FuoriStandardAllegato>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetElencoAllegati: " + ex.Message);
            }
        }
        public bool DeleteAllegato(String NomeFile, String ServerPath, String TipoFile)
        {
            try
            {
                db.BeginTransaction();

                var sql = Sql.Builder.Append("DELETE FROM gri_fuori_standard_allegati WHERE NOME_FILE = @0", NomeFile);
                db.Execute(sql);

                System.IO.File.Delete(ServerPath + NomeFile + TipoFile);

                db.CompleteTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                return false;
            }
        }
    }
}
