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
    public class TipologiaFuoriStandardRepo : RepositoryBase<TipologiaFuoriStandard>, ITipologiaFuoriStandardRepo
    {
        public List<String> GetTipologieByGrouping(List<String> Grouping)
        {
            List<String> _listaByGrouping = new List<String>();
            List<String> _listaCompleta = new List<String>();
            try
            {
                foreach (var item in Grouping)
                {
                    var sql = Sql.Builder.Append("select DESC_PRESTAZIONE from GRI_CAPGROUPING_ON_STANDARD where CAPGROUPING_CODE = @0", item);
                    _listaByGrouping = db.Query<String>(sql).ToList<String>();
                    foreach (var items in _listaByGrouping)
                    {
                        _listaCompleta.Add(items);
                    }
                    _listaByGrouping.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieByGrouping: " + ex.Message);
            }
            return _listaCompleta;
        }

        public ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard)
        {
            ISubCollection<TipologiaFuoriStandard> _tipologie = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gin_standard_v");
                _tipologie = db.Query<TipologiaFuoriStandard>(sql).ToSubCollection<TipologiaFuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieDesc: " + ex.Message);
            }
            return _tipologie;
        }

        public List<String> GetTipologieFilter()
        {
            List<String> _tipologie = null;
            List<String> _tipValidazione = new List<String>();
            try
            {
                var sql = Sql.Builder.Append("select distinct cod_gruppo from GIN_FUORI_STANDARD_DA_VALID_V");
                _tipValidazione = db.Query<String>(sql).ToList<String>();
                foreach (var item in _tipValidazione)
                {
                    var getDatiTipologie = Sql.Builder.Append("select * from gri_capgrouping_on_standard where desc_prestazione = @0", item);
                    _tipologie.Add(db.SingleOrDefault<String>(getDatiTipologie));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieFilter: " + ex.Message);
            }
            return _tipologie;
        }

        public TipologiaFuoriStandard GetTipologiaStandard(Int32 IdStandard)
        {
            try
            {
                var sql = Sql.Builder.Append("select * from gin_standard_v where id_standard = @0 ", IdStandard);
                return db.FirstOrDefault<TipologiaFuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologiaStandard: " + ex.Message);
            }
        }

    }
}
