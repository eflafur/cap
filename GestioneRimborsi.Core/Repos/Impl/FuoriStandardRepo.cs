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
    public class FuoriStandardRepo : RepositoryBase<FuoriStandard>, IFuoriStandardRepo
    {
        public ISubCollection<FuoriStandard> GetIndennizziAperti(String Utente)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V ");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndennizziAperti: " + ex.Message);
            }
            return _list;
        }

        public List<String> GetTipologieByGrouping(List<String> Grouping)
        {
            List<String> _listaByGrouping = new List<String>();
            //List<String> _listaCompleta = new List<String>();
            try
            {
                //foreach (var item in Grouping)
                //{
                Grouping.Add("");
                var sql = Sql.Builder.Append(string.Format("SELECT DESC_PRESTAZIONE from GRI_CAPGROUPING_ON_STANDARD where CAPGROUPING_CODE IN ({0})", string.Join(",", Grouping.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                _listaByGrouping = db.Query<String>(sql).ToList<String>();
                //foreach (var items in _listaByGrouping)
                //{
                //    _listaCompleta.Add(items);
                //}
                //_listaByGrouping.Clear();
                //}
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieByGrouping: " + ex.Message);
            }
            return _listaByGrouping;
        }

        public ISubCollection<FuoriStandard> GetFuoriStandardByTipologia(String CodGruppo, String Utente)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V val where val.cod_gruppo LIKE @0 and val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0)", CodGruppo + "%");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndennizziAperti: " + ex.Message);
            }
            return _list;
        }

        public Dictionary<string, int> GetCountFuoriStandard(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select TRIM(cod_gruppo) Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where TRIM(val.cod_gruppo) IN ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by TRIM(val.cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountFuoriStandard: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetCountFuoriStandardDaValidare(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select TRIM(cod_gruppo) Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where TRIM(val.cod_gruppo) IN ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by TRIM(val.cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountFuoriStandardDaValidare: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetCountFuoriStandardNonIndennizzabili(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select TRIM(cod_gruppo) Key, count(*) Value from GIN_FUORI_STANDARD_STORICO_V where TIPO_STANDARD = 'Specifico' AND TRIM(cod_gruppo) IN ({0}) AND FLG_UTE_NON_INDENNIZZABILE = 'S' group by TRIM(cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountFuoriStandard: " + ex.Message);
            }
            return result;
        }

        public ISubCollection<FuoriStandard> GetStoricoByTipologia(String CodGruppo, String Utente)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_STORICO_V where cod_gruppo LIKE @0", CodGruppo + "%");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStoricoByTipologia: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardDaApprovareByFilters(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append("SELECT val.* FROM GIN_FUORI_STD_DA_VALID_FS_V val left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric)
                { sql.Append("OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL)", (!String.IsNullOrEmpty(codCliente) ? ("%" + (codCliente.ToUpper()) + "%") : "")); }
                sql.Append(")) cli2 on cli2.cod_cliente = val.cod_cliente")
                .Append(string.Format(" WHERE TRIM(val.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                if (!String.IsNullOrEmpty(indicatore))
                {
                    sql.Append("AND val.ID_STANDARD = @0", indicatore);
                }
                if (!String.IsNullOrEmpty(codRintracciabilita)) { sql.Append("AND val.COD_RINTRACCIABILITA = @0", codRintracciabilita); }
                if (!String.IsNullOrEmpty(codCliente))
                { sql.Append("AND(val.COD_CLIENTE = @0 OR val.cod_cliente in (cli2.cod_cliente))", codCliente); }
                if (!(dataInizio == DateTime.MinValue || dataInizio == DateTime.MaxValue))
                { sql.Append("AND val.DT_DECORRENZA_INDENNIZZO >= @0", dataInizio); }
                if (!(dataFine == DateTime.MinValue || dataFine == DateTime.MaxValue))
                { sql.Append(" AND val.DT_DECORRENZA_INDENNIZZO <= @0", dataFine); }
                if (inStandard == "S")
                    sql.Append(" AND val.FLAG_ORIGINE = 'I'");
                else if (inStandard == "FS")
                    sql.Append(" AND val.FLAG_ORIGINE = 'F'");
                if (stato == "Rifiutate")
                    sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 2)");
                else if (stato == "daApprovare")
                    if (isProcessOwner)
                        sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 0 and flg_stato = 1)");
                    else 
                        sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 0 and flg_stato = 2)");
                else if (stato == "daApprovareSolaLettura")
                        sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 0)");
                else if (stato == "rettificheTutte")
                    if (isProcessOwner)
                        sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs ret where storico = 0 and (flg_stato = 1 or flg_stato = 3))");
                    else
                        sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs ret where storico = 0 and (flg_stato = 2 or flg_stato = 3))");
                else sql.Append(" AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0)");
                sql.Append(" AND rownum <= 500");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaFuoriStandardByFilter: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, String stato, bool isProcessOwner)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append("SELECT val.* FROM GIN_FUORI_STANDARD_DA_VALID_V val left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric)
                { sql.Append(" OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL)", (!String.IsNullOrEmpty(codCliente) ? ("%" + (codCliente.ToUpper()) + "%") : "")); }
                sql.Append(")) cli2 on cli2.cod_cliente = val.cod_cliente")
                .Append(string.Format(" WHERE TRIM(val.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                if (!String.IsNullOrEmpty(indicatore))
                {
                    sql.Append("AND val.ID_STANDARD = @0", indicatore);
                }
                if (!String.IsNullOrEmpty(codRintracciabilita)) { sql.Append("AND val.COD_RINTRACCIABILITA = @0", codRintracciabilita); }
                if (!String.IsNullOrEmpty(codCliente))
                { sql.Append("AND(val.COD_CLIENTE = @0 OR val.cod_cliente in (cli2.cod_cliente))", codCliente); }
                if (!(dataInizio == DateTime.MinValue || dataInizio == DateTime.MaxValue))
                { sql.Append("AND val.DT_DECORRENZA_INDENNIZZO >= @0", dataInizio); }
                if (!(dataFine == DateTime.MinValue || dataFine == DateTime.MaxValue))
                { sql.Append(" AND val.DT_DECORRENZA_INDENNIZZO <= @0", dataFine); }
                if (inStandard == "S")
                    sql.Append(" AND val.FLAG_ORIGINE = 'I'");
                else if (inStandard == "FS")
                    sql.Append(" AND val.FLAG_ORIGINE = 'F'");
                if (stato == "Rifiutate")
                    sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 2)");
                else if (stato == "daApprovare")
                    sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs where storico = 0 and esito = 0)");
                else if (stato == "rettificheTutte")
                    sql.Append(" AND val.ID_INDENNIZZO in(select distinct id_fuori_standard from gri_rettifiche_fs ret where storico = 0)");
                else sql.Append(" AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0)");
                sql.Append(" AND rownum <= 500");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaFuoriStandardByFilter: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardStoricoByFilter(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append("SELECT sto.* FROM (select * from GIN_FUORI_STANDARD_STORICO_V union select * from GIN_FUORI_STD_DA_VALID_FS_V) sto left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric)
                { sql.Append(" OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL)", (!String.IsNullOrEmpty(codCliente) ? ("%" + (codCliente.ToUpper()) + "%") : "")); };
                                     sql.Append(" )) cli2 on cli2.cod_cliente = sto.cod_cliente ")
                                     .Append(string.Format(" WHERE TRIM(sto.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())))
                                     .Append(@"AND (sto.ID_STANDARD = @0 OR @0 IS NULL)
                                     AND (sto.COD_RINTRACCIABILITA = @1 OR @1 IS NULL)
                                     AND ((sto.COD_CLIENTE = @2 OR @2 IS NULL) OR (sto.cod_cliente in (cli2.cod_cliente) OR @2 IS NULL))
                                     AND (sto.DT_DECORRENZA_INDENNIZZO >= @4 OR @4 IS NULL OR TO_CHAR(@4, 'YYYY') = '0001')
                                     AND (sto.DT_DECORRENZA_INDENNIZZO <= @5 OR @5 IS NULL OR TO_CHAR(@5, 'YYYY') = '9999') AND rownum < 2000", indicatore, codRintracciabilita, codCliente, "%" + ((codCliente != null) ? codCliente.ToUpper() : "") + "%", dataInizio, dataFine);
                if (inStandard == "S")
                    sql.Append(" AND sto.FLAG_ORIGINE = 'I'");                
                else if (inStandard == "FS")
                    sql.Append(" AND sto.FLAG_ORIGINE = 'F'");                

                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaFuoriStandardStoricoByFilter: " + ex.Message);
            }
            return _list;
        }

        public ISubCollection<FuoriStandard> CercaFuoriStandardNonIndennizzabili(List<String> tipologie, String indicatore, String codRintracciabilita, String codCliente, DateTime dataInizio, DateTime dataFine, String inStandard, bool checkCliente, bool checkIndennizzabile)
        {
            ISubCollection<FuoriStandard> _list = null;
            //ISubCollection<FuoriStandard> listaSenzaCodCliente = null;
            try
            {
                int n;
                bool isNumeric = int.TryParse(codCliente, out n);
                tipologie.Add("");
                var sql = Sql.Builder.Append("SELECT sto.* FROM GIN_FUORI_STANDARD_STORICO_V sto left join (select cli.cod_cliente, cli.des_ragione_sociale from gin_clienti_v cli where (cli.cod_cliente = @0 ", codCliente);
                if (!isNumeric)
                { sql.Append("OR cli.DES_RAGIONE_SOCIALE LIKE NVL(@0,NULL) ", (!String.IsNullOrEmpty(codCliente) ? ("%" + (codCliente.ToUpper()) + "%") : "")); }
                sql.Append(" )) cli2 on cli2.cod_cliente = sto.cod_cliente")
                .Append(" WHERE (sto.ID_STANDARD = @0 OR @0 IS NULL)", indicatore)
                .Append(string.Format(" AND TRIM(sto.cod_gruppo) IN ({0}) ", string.Join(",", tipologie.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                if (checkIndennizzabile)
                    sql.Append(" AND sto.FLG_UTE_NON_INDENNIZZABILE = 'S'");
                else sql.Append(" AND sto.FLG_UTE_NON_INDENNIZZABILE = 'N'");
                sql.Append(@" AND (sto.COD_RINTRACCIABILITA = @1 OR @1 IS NULL)
                                     AND ((sto.COD_CLIENTE = @2 OR @2 IS NULL) OR (sto.cod_cliente in (cli2.cod_cliente) OR @2 IS NULL))
                                     AND (sto.DT_DECORRENZA_INDENNIZZO >= @4 OR @4 IS NULL OR TO_CHAR(@4, 'YYYY') = '0001')
                                     AND (sto.DT_DECORRENZA_INDENNIZZO <= @5 OR @5 IS NULL OR TO_CHAR(@5, 'YYYY') = '9999')                                                               
                                     AND sto.TIPO_STANDARD = 'Specifico' AND ROWNUM < 500", indicatore, codRintracciabilita, codCliente, "%" + ((codCliente != null) ? codCliente.ToUpper() : "") + "%", dataInizio, dataFine);
                if (inStandard == "S")
                    sql.Append(" AND sto.FLAG_ORIGINE = 'I'");
                else if (inStandard == "FS")
                    sql.Append(" AND sto.FLAG_ORIGINE = 'F'");
                if (checkCliente)
                    sql.Append(" AND sto.COD_CLIENTE = '-1'");
                else if (!checkCliente && !checkIndennizzabile)
                    sql.Append(" AND 1=0");

                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
                //if (checkCliente && !checkIndennizzabile)
                //{
                //    listaSenzaCodCliente = _list.Items.Where(c => c.CodCliente == "-1").ToSubCollection<FuoriStandard>();
                //    return listaSenzaCodCliente;
                //}
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in CercaFuoriStandardNonIndennizzabili: " + ex.Message);
            }
            return _list;
        }

        public Dictionary<string, int> GetCountStoricoFuoriStandard(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select TRIM(cod_gruppo) Key, count(*) Value from GIN_FUORI_STANDARD_STORICO_V where TRIM(cod_gruppo) IN ({0}) group by TRIM(cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountStoricoFuoriStandard: " + ex.Message);
            }
            return result;

        }

        public Dictionary<string, int> GetCountFuoriStandardTutti(List<String> CodiciGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (CodiciGruppo == null || CodiciGruppo.Count() == 0)
                return result;
            try
            {
                CodiciGruppo.Add("");
                //var sql = Sql.Builder.Append(string.Format(@"select TRIM(cod_gruppo) Key, count(*) Value from (select * from GIN_FUORI_STANDARD_STORICO_V UNION select * from GIN_FUORI_STANDARD_DA_VALID_V) tot
                //                                             where TRIM(cod_gruppo) IN ({0}) AND tot.FLAG_ORIGINE = 'F' group by TRIM(cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var sql = Sql.Builder.Append(string.Format(@"select TRIM(cod_gruppo) Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where TRIM(val.cod_gruppo) IN ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by TRIM(val.cod_gruppo) order by TRIM(cod_gruppo)", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCountStoricoFuoriStandard: " + ex.Message);
            }
            return result;

        }

        public ISubCollection<FuoriStandard> GetIndennizziPagabili(String Utente)
        {
            ISubCollection<FuoriStandard> _list = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_PAGARE_V");
                _list = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndennizziPagabili: " + ex.Message);
            }
            return _list;
        }

        public FuoriStandard GetFuoriStandardByID(String FuoriStandardID)
        {
            FuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V where ID_INDENNIZZO = @0 order by DT_DECORRENZA_INDENNIZZO", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardByID: " + ex.Message);
            }
            return _indennizzo;
        }

        public FuoriStandard GetFuoriStandardByIdwh(String FuoriStandardID)
        {
            FuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STD_DA_VALID_FS_V WHERE ID_INDENNIZZO_DWH = @0 order by DT_DECORRENZA_INDENNIZZO", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardByIdwh: " + ex.Message);
            }
            return _indennizzo;
        }

        public FuoriStandard GetFuoriStandardStoricoByID(String FuoriStandardID)
        {
            FuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_STORICO_V where ID_INDENNIZZO = @0 order by DT_DECORRENZA_INDENNIZZO", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardStoricoByID: " + ex.Message);
            }
            return _indennizzo;
        }

        public ClienteFuoriStandard GetCliFuoriStandard(String CodiceCliente)
        {
            //ClienteFuoriStandard _cliente = new ClienteFuoriStandard();
            ClienteFuoriStandard _cliente = null;
            try
            {
                var sql = Sql.Builder.Append("select * from gin_clienti_v where cod_cliente = @0 ", CodiceCliente);
                _cliente = db.FirstOrDefault<ClienteFuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCliFuoriStandard: " + ex.Message);
            }
            return _cliente;
        }
        public ISubCollection<String> GetContrattoByCliente(String CodiceCliente)
        {
            List<String> _contratti = new List<String>();
            try
            {
                var sql = Sql.Builder.Append("select cod_id_contratto from GIN_CONTRATTI_V where cod_intestatario = @0 ", CodiceCliente);
                _contratti = db.Query<String>(sql).ToList<String>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetRagioneSocialeCliIndennizzo: " + ex.Message);
            }
            return _contratti.ToSubCollection<String>();
        }

        public String GetPUFByContratto(String CodiceContratto)
        {
            String _puf = null;
            try
            {
                var sql = Sql.Builder.Append("select cod_punto_fornitura from GIN_CONTRATTI_V where cod_id_contratto = @0 and rownum = 1 ", CodiceContratto);
                _puf = db.SingleOrDefault<String>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetPUFByContratto: " + ex.Message);
            }
            return _puf;
        }

        public ISubCollection<FuoriStandard> GetFuoriStandardStorico(String Tipologia)
        {
            ISubCollection<FuoriStandard> _indennizzi = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GIN_FUORI_STANDARD_STORICO_V WHERE COD_GRUPPO LIKE @0 UNION ALL (SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V WHERE COD_GRUPPO LIKE @0)", Tipologia + "%");
                _indennizzi = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetFuoriStandardStorico: " + ex.Message);
            }
            return _indennizzi;
        }

        public ISubCollection<FuoriStandard> GetStoricoChiusi(String Tipologia)
        {
            ISubCollection<FuoriStandard> _indennizzi = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_STORICO_V where cod_gruppo LIKE @0", Tipologia + "%");
                _indennizzi = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStoricoChiusi: " + ex.Message);
            }
            return _indennizzi;
        }

        public ISubCollection<FuoriStandard> GetStoricoTutti(String Tipologia)
        {
            ISubCollection<FuoriStandard> _indennizzi = null;
            try
            {
                var sql = Sql.Builder.Append("SELECT * FROM GIN_FUORI_STANDARD_STORICO_V WHERE COD_GRUPPO LIKE @0 UNION ALL (SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V WHERE COD_GRUPPO LIKE @0)", Tipologia + "%");
                _indennizzi = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStoricoTutti: " + ex.Message);
            }
            return _indennizzi;
        }

        public ISubCollection<FuoriStandard> GetStoricoPendenti(String Tipologia)
        {
            ISubCollection<FuoriStandard> _indennizzi = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_VALID_V where cod_gruppo LIKE @0", Tipologia + "%");
                _indennizzi = db.Query<FuoriStandard>(sql).ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetStoricoPendenti: " + ex.Message);
            }
            return _indennizzi;
        }

        public FuoriStandard GetIndennizzoPagabileByID(String FuoriStandardID)
        {
            FuoriStandard _indennizzo = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_FUORI_STANDARD_DA_PAGARE_V where ID_INDENNIZZO = @0", FuoriStandardID);
                _indennizzo = db.FirstOrDefault<FuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndennizzoPagabileByID: " + ex.Message);
            }
            return _indennizzo;
        }

        public ISubCollection<TipologiaFuoriStandard> GetTipologieDesc(List<String> CodStandard)
        {
            if (CodStandard == null)
            {
                CodStandard = new List<string>();
                CodStandard.Add("");
            }
            ISubCollection<TipologiaFuoriStandard> _tipologie = null;
            try
            {
                CodStandard.Add("");
                var sql = Sql.Builder.Append(string.Format("select * from gin_standard_v where gruppo in ({0}) order by DESC_STANDARD DESC", string.Join(",", CodStandard.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                _tipologie = db.Query<TipologiaFuoriStandard>(sql).ToSubCollection<TipologiaFuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieDesc: " + ex.Message);
            }
            return _tipologie;
        }

        public ISubCollection<TipologiaFuoriStandard> GetIndicatoreByIds(List<String> idStandard)
        {
            ISubCollection<TipologiaFuoriStandard> result = null;
            if (idStandard.Count > 0)
            {
                try
                {
                    idStandard.Add("");
                    var sql = Sql.Builder.Append(string.Format("select * from gin_standard_v where id_standard IN ({0}) order by DESC_STANDARD", string.Join(",", idStandard.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                    result = db.Query<TipologiaFuoriStandard>(sql).ToSubCollection<TipologiaFuoriStandard>();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoreByIds: " + ex.Message);
                }
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoreByGroup(String CodGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                var sql = Sql.Builder.Append("select id_standard Key, count(*) Value from (select * from GIN_FUORI_STANDARD_STORICO_V union select * from GIN_FUORI_STANDARD_DA_VALID_V) fs where fs.cod_gruppo = @0 group by fs.id_standard", CodGruppo);
                //var sql = Sql.Builder.Append("select id_standard Key, 0 Value from gin_standard_v where gruppo = @0 group by id_standard", CodGruppo);                
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoreByGroup: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoreByGroups(List<String> CodGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                CodGruppo.Add("");
                //var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from (select * from GIN_FUORI_STANDARD_STORICO_V union select * from GIN_FUORI_STANDARD_DA_VALID_V) fs where fs.cod_gruppo in({0}) group by fs.id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where val.cod_gruppo in ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoreByGroups: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoriForRettifiche(List<String> CodGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                CodGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where val.cod_gruppo in ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoriForRettifiche: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoriValidazione(List<String> CodGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                CodGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from GIN_FUORI_STANDARD_DA_VALID_V val where val.cod_gruppo in ({0}) AND val.ID_INDENNIZZO not in(select distinct id_fuori_standard from gri_rettifiche_fs where esito != 2 and storico = 0) AND val.FLAG_ORIGINE = 'F' group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoriValidazione: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoriForApprover(List<String> CodGruppo, bool isProcessOwner)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                CodGruppo.Add("");
                ////               var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from (select tot.* from GIN_FUORI_STANDARD_DA_VALID_V tot right join gri_rettifiche_fs ret on ret.ID_FUORI_STANDARD = tot.ID_INDENNIZZO where ret.storico = 0 and ret.esito = 0) where cod_gruppo in ({0}) group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                //        var sql = Sql.Builder.Append(string.Format("select ID_STANDARD Key, 0  Value  from gin_standard_V where gruppo in ({0}) group by ID_STANDARD", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from (select tot.* from GIN_FUORI_STD_DA_VALID_FS_V tot right join gri_rettifiche_fs ret on ret.ID_FUORI_STANDARD = tot.ID_INDENNIZZO where ret.storico = 0 and ret.esito = 0 and ret.flg_stato != {1}) where cod_gruppo in ({0}) group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray()), (isProcessOwner ? "2" : "1")));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoriForApprover: " + ex.Message);
            }
            return result;
        }

        public Dictionary<string, int> GetIndicatoriNonIndennizzabili(List<String> CodGruppo)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            try
            {
                CodGruppo.Add("");
                var sql = Sql.Builder.Append(string.Format("select id_standard Key, count(*) Value from GIN_FUORI_STANDARD_STORICO_V where cod_gruppo in ({0}) and FLG_UTE_NON_INDENNIZZABILE = 'S' and TIPO_STANDARD = 'Specifico' group by id_standard", string.Join(",", CodGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                var groupedResult = db.Query<GestioneRimborsi.Core.Entities.KeyValueEntity<int>>(sql);
                groupedResult.ToList().ForEach(ee =>
                {
                    result.Add(ee.Key, ee.Value);
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetIndicatoriNonIndennizzabili: " + ex.Message);
            }
            return result;
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

        public List<String> GetTipologieStoricoFilter()
        {
            List<String> _tipologie = null;
            List<String> _tipValidazione = new List<String>();
            try
            {
                var sql = Sql.Builder.Append("select distinct cod_gruppo from GIN_FUORI_STANDARD_STORICO_V");
                _tipValidazione = db.Query<String>(sql).ToList<String>();
                foreach (var item in _tipValidazione)
                {
                    var getDatiTipologie = Sql.Builder.Append("select * from gri_capgrouping_on_standard where desc_prestazione = @0", item);
                    _tipologie.Add(db.SingleOrDefault<String>(getDatiTipologie));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetTipologieStoricoFilter: " + ex.Message);
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

        public ISubCollection<CausaRitardoFuoriStandard> GetListaCategorie()
        {
            ISubCollection<CausaRitardoFuoriStandard> _categorie = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_RESPONSABILITA_RITARDO_V order by descrizione");
                _categorie = db.Query<CausaRitardoFuoriStandard>(sql).ToSubCollection<CausaRitardoFuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetListaCategorie: " + ex.Message);
            }
            return _categorie;
        }

        public ISubCollection<SottoCausaRitardoFS> GetListaSottoCategorieByCategoria(String CategoriaId)
        {
            ISubCollection<SottoCausaRitardoFS> _sottocategorie = null;
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_MOTIVO_RITARDO_V where CODICE_CAUSA = @0 order by descrizione", CategoriaId);
                _sottocategorie = db.Query<SottoCausaRitardoFS>(sql).ToSubCollection<SottoCausaRitardoFS>();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetListaSottoCategorieByCategoria: " + e.Message);
            }
            return _sottocategorie;
        }

        public CausaRitardoFuoriStandard GetCategoriaByCod(String CategoriaID)
        {
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_RESPONSABILITA_RITARDO_V where CODICE_CAUSA = @0 ", CategoriaID);
                return db.FirstOrDefault<CausaRitardoFuoriStandard>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetCategoriaByCod: " + ex.Message);
            }
        }

        public SottoCausaRitardoFS GetSottoCategoriaByCod(String SottocategoriaID)
        {
            try
            {
                var sql = Sql.Builder.Append("select * from GIN_MOTIVO_RITARDO_V where CODICE_SOTTOCAUSA = @0 ", SottocategoriaID);
                return db.FirstOrDefault<SottoCausaRitardoFS>(sql);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in GetSottoCategoriaByCod: " + e.Message);
            }
        }

        public bool IsApproved(String CategoriaId)
        {
            var approvato = 0;
            try
            {
                var sql = Sql.Builder.Append("select FUORI_STANDARD_APPROVATO from GIN_RESPONSABILITA_RITARDO_V where CODICE_CAUSA = @0", CategoriaId);
                approvato = db.FirstOrDefault<Int32>(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in IsApproved: " + ex.Message);
            }
            return approvato == 1 ? true : false;
        }

        public String FirstValidation(String FuoriStandardDataCliente, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();

                FuoriStandard _indennizzo = GetFuoriStandardByID(FuoriStandardDataCliente.ToString());

                PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, _indennizzo.IDFS),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, _indennizzo.CodCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, _indennizzo.CodPuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, _indennizzo.CodContratto),
                                                     new PetaPocoInputParameter("p_UTE_VALIDAZIONE", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_DT_VALIDAZIONE", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_UTE_APP_PO", System.Data.DbType.String, String.Empty),
                                                     new PetaPocoInputParameter("p_DT_APP_PO", System.Data.DbType.Date, null),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note),
                                                     new PetaPocoInputParameter(" p_CODICE_CAUSA", System.Data.DbType.String, CodiceCausa),
                                                     new PetaPocoInputParameter("p_CODICE_SOTTOCAUSA", System.Data.DbType.String, CodiceSottocausa),
                                                     new PetaPocoInputParameter("p_TIPO_STANDARD", System.Data.DbType.String, _indennizzo.TipoStandard),
                                                     new PetaPocoInputParameter("p_FLG_UTE_NON_INDENNIZZABILE", System.Data.DbType.String, NonIndennizzabile),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_VALIDAZIONE", conferma);

                /**********************/
                if (resultConferma == null)
                {
                    throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                }
                if (resultConferma.Result)
                    if (resultConferma.OutputParams != null)
                    {
                        PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                        if (_p != null)
                        {
                            if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                            {
                                _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                if (_p != null)
                                {
                                    throw new ApplicationException(_p.Value.ToString());
                                }
                                else
                                {
                                    throw new ApplicationException("Valore di Output non trovato");
                                }
                            }
                        }
                        else
                        {
                            throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                    }
                else
                {
                    throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                }
                /**********************/
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Validazione Fuori Standard: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public String ValidazioneNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf, String CodiceCausa, String CodiceSottocausa, String Utente, String Note, String NonIndennizzabile)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();
                FuoriStandard _indennizzo = GetFuoriStandardByID(IdFS.ToString());

                PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(IdFS)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, CodiceCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, CodicePuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, CodiceContratto),
                                                     new PetaPocoInputParameter("p_UTE_VALIDAZIONE", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_DT_VALIDAZIONE", System.Data.DbType.Date, DateTime.Now),
                                                     new PetaPocoInputParameter("p_UTE_APP_PO", System.Data.DbType.String, String.Empty),
                                                     new PetaPocoInputParameter("p_DT_APP_PO", System.Data.DbType.Date, null),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note),
                                                     new PetaPocoInputParameter(" p_CODICE_CAUSA", System.Data.DbType.String, CodiceCausa),
                                                     new PetaPocoInputParameter("p_CODICE_SOTTOCAUSA", System.Data.DbType.String, CodiceSottocausa),
                                                     new PetaPocoInputParameter("p_TIPO_STANDARD", System.Data.DbType.String, _indennizzo.TipoStandard),
                                                     new PetaPocoInputParameter("p_FLG_UTE_NON_INDENNIZZABILE", System.Data.DbType.String, NonIndennizzabile),
                                                     new PetaPocoInputParameter("p_FLAG_ORIGINE", System.Data.DbType.String, _indennizzo.FlagOrigine),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_VALIDAZIONE", conferma);

                /**********************/
                if (resultConferma == null)
                {
                    throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                }
                if (resultConferma.Result)
                    if (resultConferma.OutputParams != null)
                    {
                        PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                        if (_p != null)
                        {
                            if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                            {
                                _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                if (_p != null)
                                {
                                    throw new ApplicationException(_p.Value.ToString());
                                }
                                else
                                {
                                    throw new ApplicationException("Valore di Output non trovato");
                                }
                            }
                        }
                        else
                        {
                            throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                    }
                else
                {
                    throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                }
                /**********************/
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Validazione Nuovo Cliente: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public String AssociaNuovoCliente(String IdFS, String CodiceCliente, String CodiceContratto, String CodicePuf)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();
                //FuoriStandard _indennizzo = GetFuoriStandardByID(IdFS.ToString());

                PetaPocoParameter[] associa = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, Convert.ToInt32(IdFS)),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, CodiceCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, CodicePuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, CodiceContratto),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_MODIFICA_CLIENTE", associa);

                /**********************/
                if (resultConferma == null)
                {
                    throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                }
                if (resultConferma.Result)
                    if (resultConferma.OutputParams != null)
                    {
                        PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                        if (_p != null)
                        {
                            if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                            {
                                _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                if (_p != null)
                                {
                                    throw new ApplicationException(_p.Value.ToString());
                                }
                                else
                                {
                                    throw new ApplicationException("Valore di Output non trovato");
                                }
                            }
                        }
                        else
                        {
                            throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                    }
                else
                {
                    throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                }
                /**********************/
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Associa Nuovo Cliente: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public String RifiutaFuoriStandard(List<String> FuoriStandardDataCliente, String Utente, String Note)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();
                foreach (var item in FuoriStandardDataCliente)
                {
                    var items = item.Split(';');
                    FuoriStandard _indennizzo = GetFuoriStandardByID(items[0].ToString());
                    PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, items[0]),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, items[2].ToString()),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, _indennizzo.CodPuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, _indennizzo.CodContratto),
                                                     new PetaPocoInputParameter("p_UTE_ANN", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                    var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_ANNULLAMENTO", conferma);

                    /**********************/
                    if (resultConferma == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultConferma.Result)
                        if (resultConferma.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                    if (_p != null)
                                    {
                                        throw new ApplicationException(_p.Value.ToString());
                                    }
                                    else
                                    {
                                        throw new ApplicationException("Valore di Output non trovato");
                                    }
                                }
                            }
                            else
                            {
                                throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                        }
                    else
                    {
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                    }
                    /**********************/
                }
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Rifiuta Fuori Standard: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public String IndennizziPagabili(List<String> FuoriStandardDataCliente, String Utente, String Note)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                db.BeginTransaction();
                foreach (var item in FuoriStandardDataCliente)
                {
                    var items = item.Split(';');
                    FuoriStandard _indennizzo = GetIndennizzoPagabileByID(items[0].ToString());
                    PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_ID_INDENNIZZO", System.Data.DbType.Int32, items[0]),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, items[2].ToString()),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, _indennizzo.CodPuf),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, _indennizzo.CodContratto),
                                                     new PetaPocoInputParameter("p_UTE_PAG", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                    var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_PAGABILE", conferma);

                    /**********************/
                    if (resultConferma == null)
                    {
                        throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                    }
                    if (resultConferma.Result)
                        if (resultConferma.OutputParams != null)
                        {
                            PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                            if (_p != null)
                            {
                                if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                                {
                                    _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                    if (_p != null)
                                    {
                                        throw new ApplicationException(_p.Value.ToString());
                                    }
                                    else
                                    {
                                        throw new ApplicationException("Valore di Output non trovato");
                                    }
                                }
                            }
                            else
                            {
                                throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                            }
                        }
                        else
                        {
                            throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                        }
                    else
                    {
                        throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                    }
                    /**********************/
                }
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Liquidazione Indennizzi: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public String RegistrazioneFuoriStandard(String CodiceCliente, String CodicePUF, String Tipologia, DateTime DecorrenzaIndennizzo, String FuoriStandard, String IdSAFO, String Utente, String Note, String contratti, String CodGruppo, String ValStandard)
        {
            Boolean _hasTransactionRolledBack = false;
            try
            {
                if (String.IsNullOrEmpty(ValStandard))
                    ValStandard = "-1";

                db.BeginTransaction();

                PetaPocoParameter[] conferma = { new PetaPocoInputParameter("p_PROVENIENZA", System.Data.DbType.String, "GIN"),
                                                     new PetaPocoInputParameter("p_COD_CLIENTE", System.Data.DbType.String, CodiceCliente),
                                                     new PetaPocoInputParameter("p_COD_PUF", System.Data.DbType.String, CodicePUF),
                                                     new PetaPocoInputParameter("p_COD_CONTRATTO", System.Data.DbType.String, contratti),
                                                     new PetaPocoInputParameter("p_COD_PRESTAZIONE", System.Data.DbType.String, GetTipologiaStandard(Convert.ToInt32(Tipologia)).CodicePrestazione),
                                                     new PetaPocoInputParameter("p_DT_DECORRENZA_INDENNIZZO", System.Data.DbType.Date, DecorrenzaIndennizzo),
                                                     new PetaPocoInputParameter("p_TEMPO_LAVORAZIONE", System.Data.DbType.Decimal, FuoriStandard.Replace(",", ".")),
                                                     new PetaPocoInputParameter("p_UTE_INS", System.Data.DbType.String, Utente),
                                                     new PetaPocoInputParameter("p_ID_SAFO", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_Note", System.Data.DbType.String, Note),
                                                     new PetaPocoInputParameter("p_ID_INDENNIZZO_DWH", System.Data.DbType.Int32, null),
                                                     new PetaPocoInputParameter("p_NUMERO_PRESTAZIONE", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_DESCRIZIONE_PRESTAZIONE", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_ID_STANDARD", System.Data.DbType.Int32, GetTipologiaStandard(Convert.ToInt32(Tipologia)).IDStandard),
                                                     new PetaPocoInputParameter("p_DATA_INIZIO", System.Data.DbType.Date, null),
                                                     new PetaPocoInputParameter("p_DATA_FINE", System.Data.DbType.Date, null),
                                                     new PetaPocoInputParameter("p_TIPO", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_ESITO", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_DETTAGLIO_ESITO", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_NOTE_ESITO", System.Data.DbType.String, null),
                                                     new PetaPocoInputParameter("p_COD_STANDARD", System.Data.DbType.Int32, Convert.ToInt32(Tipologia)),
                                                     new PetaPocoInputParameter("p_TIPO_STANDARD", System.Data.DbType.String, GetTipologiaStandard(Convert.ToInt32(Tipologia)).TipoStandard),
                                                     new PetaPocoInputParameter("p_COD_GRUPPO", System.Data.DbType.String, CodGruppo),
                                                     new PetaPocoInputParameter("p_COD_RINTRACCIABILITA", System.Data.DbType.String, IdSAFO),
                                                     new PetaPocoInputParameter("p_VAL_STANDARD", System.Data.DbType.Int32, Convert.ToInt32(ValStandard)),
                                                     new PetaPocoOutputParameter("p_Retc", System.Data.DbType.Int32),
                                                     new PetaPocoOutputParameter("p_Msg", System.Data.DbType.String, 2500) };

                var resultConferma = db.ExecuteProcedure("PKG_INDENNIZZI.GIN_INSERIMENTO", conferma);

                /**********************/
                if (resultConferma == null)
                {
                    throw new ApplicationException("1. Errore generico durante l'esecuzione della procedura");
                }
                if (resultConferma.Result)
                    if (resultConferma.OutputParams != null)
                    {
                        PetaPocoOutputParameter _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Retc", StringComparison.CurrentCultureIgnoreCase));
                        if (_p != null)
                        {
                            if ((_p.Value == DBNull.Value ? 99 : (int)_p.Value) != 0)
                            {
                                _p = resultConferma.OutputParams.SingleOrDefault<PetaPocoOutputParameter>(x => string.Equals(x.ParameterName, "p_Msg", StringComparison.CurrentCultureIgnoreCase));
                                if (_p != null)
                                {
                                    throw new ApplicationException(_p.Value.ToString());
                                }
                                else
                                {
                                    throw new ApplicationException("Valore di Output non trovato");
                                }
                            }
                        }
                        else
                        {
                            throw new ApplicationException("2. Errore generico durante l'esecuzione della procedura");
                        }
                    }
                    else
                    {
                        throw new ApplicationException("4. Errore generico durante l'esecuzione della procedura. Nessun parametro di output");
                    }
                else
                {
                    throw new ApplicationException(String.Format("3. Errore generico durante l'esecuzione della procedura: {0} ", resultConferma.ErrorMessage));
                }
                /**********************/
            }
            catch (Exception Ex)
            {
                _hasTransactionRolledBack = true;
                db.AbortTransaction();
                return ("Impossibile eseguire l'istruzione in Registrazione Fuori Standard: " + Ex.Message);
            }
            if (_hasTransactionRolledBack == false)
                db.CompleteTransaction();
            return String.Empty;
        }

        public ISubCollection<FuoriStandard> RicercaAvanzata(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            List<FuoriStandard> _list = new List<FuoriStandard>();
            try
            {
                if (TuttiPendChiusi == "T")
                {
                    var sql = Sql.Builder.Append(@"SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V val LEFT JOIN GIN_CLIENTI_V cli ON cli.COD_CLIENTE = val.COD_CLIENTE
                                                   WHERE (val.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((val.COD_CLIENTE = @4 OR @4 IS NULL) OR (cli.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) AND val.COD_GRUPPO LIKE @3 ", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                else if (TuttiPendChiusi == "A")
                {
                    var sql = Sql.Builder.Append(@"select val.* from GIN_FUORI_STANDARD_DA_VALID_V val inner join gri_rettifiche_fs ret on val.ID_INDENNIZZO = ret.ID_FUORI_STANDARD LEFT JOIN GIN_CLIENTI_V cli ON cli.COD_CLIENTE = val.COD_CLIENTE
                                                   where (val.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((val.COD_CLIENTE = @4 OR @4 IS NULL) OR (cli.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) and ret.storico = 0 and ret.esito = 0 and val.cod_gruppo LIKE @3", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                else if (TuttiPendChiusi == "R")
                {
                    var sql = Sql.Builder.Append(@"select val.* from GIN_FUORI_STANDARD_DA_VALID_V val inner join gri_rettifiche_fs ret on val.ID_INDENNIZZO = ret.ID_FUORI_STANDARD LEFT JOIN GIN_CLIENTI_V cli ON cli.COD_CLIENTE = val.COD_CLIENTE 
                                                   where (val.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (val.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((val.COD_CLIENTE = @4 OR @4 IS NULL) OR (cli.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) and ret.storico = 0 and ret.esito = 2 and val.cod_gruppo LIKE @3", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                return _list.ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in RicercaAvanzataStorico: " + ex.Message);
            }
        }

        public ISubCollection<FuoriStandard> RicercaAvanzataStorico(DateTime DataInizio, DateTime DataFine, String CodiceRintracciabilita, String Cliente, String CodGruppo, String TuttiPendChiusi)
        {
            List<FuoriStandard> _list = new List<FuoriStandard>();
            try
            {
                if (TuttiPendChiusi == "Tutti")
                {
                    var sql = Sql.Builder.Append(@"SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V A LEFT JOIN (SELECT COD_CLIENTE AS CODICE_CLIENTE, DES_RAGIONE_SOCIALE, COD_CLIENTE_INTEGRA, DES_CELLULARE FROM GIN_CLIENTI_V) B ON CODICE_CLIENTE = A.COD_CLIENTE
                                                    WHERE (A.COD_RINTRACCIABILITA = @0 OR @0 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO >= @1 OR @1 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO <= @2 OR @2 IS NULL) AND ((A.COD_CLIENTE = @3 OR @3 IS NULL) OR (B.DES_RAGIONE_SOCIALE LIKE @5 OR @3 IS NULL)) AND A.COD_GRUPPO LIKE @4 
                                                    UNION ALL (SELECT * FROM GIN_FUORI_STANDARD_STORICO_V A LEFT JOIN (SELECT COD_CLIENTE AS CODICE_CLIENTE, DES_RAGIONE_SOCIALE, COD_CLIENTE_INTEGRA, DES_CELLULARE FROM GIN_CLIENTI_V) B ON CODICE_CLIENTE = A.COD_CLIENTE
                                                    WHERE (A.COD_RINTRACCIABILITA = @0 OR @0 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO >= @1 OR @1 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO <= @2 OR @2 IS NULL) AND ((A.COD_CLIENTE = @3 OR @3 IS NULL) OR (B.DES_RAGIONE_SOCIALE LIKE @5 OR @3 IS NULL)) AND A.COD_GRUPPO LIKE @4)", CodiceRintracciabilita, DataInizio, DataFine, Cliente, CodGruppo + "%", "%" + Cliente + "%");
                    _list = _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                else if (TuttiPendChiusi == "Pend")
                {
                    var sql = Sql.Builder.Append(@"SELECT * FROM GIN_FUORI_STANDARD_DA_VALID_V A LEFT JOIN GIN_CLIENTI_V B ON B.COD_CLIENTE = A.COD_CLIENTE
                                                    WHERE (A.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((A.COD_CLIENTE = @4 OR @4 IS NULL) OR (B.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) AND A.COD_GRUPPO LIKE @3 ", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                else if (TuttiPendChiusi == "Chiusi")
                {
                    var sql = Sql.Builder.Append(@"SELECT * FROM GIN_FUORI_STANDARD_STORICO_V A LEFT JOIN GIN_CLIENTI_V B ON B.COD_CLIENTE = A.COD_CLIENTE
                                                    WHERE (A.COD_RINTRACCIABILITA = @2 OR @2 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO >= @0 OR @0 IS NULL) AND (A.DT_DECORRENZA_INDENNIZZO <= @1 OR @1 IS NULL) AND ((A.COD_CLIENTE = @4 OR @4 IS NULL) OR (B.DES_RAGIONE_SOCIALE LIKE @5 OR @4 IS NULL)) AND A.COD_GRUPPO LIKE @3 AND A.STADIO_INDENNIZZO = 'N' ", DataInizio, DataFine, CodiceRintracciabilita, CodGruppo + "%", Cliente, "%" + Cliente + "%");
                    _list = db.Query<FuoriStandard>(sql).ToList<FuoriStandard>();
                }
                return _list.ToSubCollection<FuoriStandard>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile eseguire l'istruzione in RicercaAvanzataStorico: " + ex.Message);
            }
        }

        public String AggiungiFile(String IdFS, System.IO.Stream file, String NomefileOriginale, String Extension, String ServerPath, String Utente)
        {
            try
            {
                db.BeginTransaction();

                //select PROGRESSIVO con SELECT MAX
                var sqlProgressivo = Sql.Builder.Append("SELECT nvl(max(Progressivo),0)+1 AS PROGRESSIVO FROM GRI_FUORI_STANDARD_ALLEGATI WHERE NOME_ORIGINALE = @0", NomefileOriginale);
                var Progressivo = db.SingleOrDefault<Int16>(sqlProgressivo);

                String NomeFile = String.Format("{0}_{1}", NomefileOriginale, Progressivo);

                var sqlAllegatiFS = Sql.Builder.Append("INSERT INTO GRI_FUORI_STANDARD_ALLEGATI(PROGRESSIVO,NOME_FILE,ESTENSIONE,DIMENSIONE,DATA_INSERIMENTO,UTENTE_INSERIMENTO,IDFS,NOME_ORIGINALE)")
                .Append(" VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", Progressivo, NomeFile, Extension, file.Length, DateTime.Now, Utente, IdFS, NomefileOriginale);
                db.Execute(sqlAllegatiFS);

                string percorso = ServerPath + NomeFile + Extension;
                byte[] bytesInStream = new byte[file.Length];
                file.Read(bytesInStream, 0, bytesInStream.Length);

                var sr1 = new System.IO.FileStream(percorso, System.IO.FileMode.Create);
                sr1.Write(bytesInStream, 0, bytesInStream.Length);
                sr1.Close();
                sr1.Dispose();

                db.CompleteTransaction();
                return String.Empty;
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Impossibile eseguire l'istruzione in AggiungiFile: " + ex.Message);
            }
        }

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

        public String ClienteCensitoInCom(String CodCliente)
        {
            String _cliente = null;
            try
            {
                var sql = Sql.Builder.Append("select cod_cliente from gin_clienti_v where cod_cliente = @0 or cod_cliente_integra = @0", CodCliente);
                _cliente = db.Query<String>(sql).FirstOrDefault();
                if (_cliente != null)
                    return "censito";
                else return String.Empty;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in ClienteCensitoInCom: " + ex.Message);
            }
        }

        public ISubCollection<ReportFuoriStandard> ExportReportPrestazioni(List<String> CodiciGruppo)
        {
            ISubCollection<ReportFuoriStandard> _list = null;
            try
            {
                CodiciGruppo.Add("");
                var gruppi = string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray());
                var sql = Sql.Builder.Append(string.Format("select * from GIN_REPORT_FUORI_STANDARD_V where cod_gruppo in ({0})", string.Join(",", CodiciGruppo.Select(ee => string.Format("'{0}'", ee)).ToArray())));
                _list = db.Query<ReportFuoriStandard>(sql).ToSubCollection<ReportFuoriStandard>();
                return _list;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Impossibile completare l'operazione in ClienteCensitoInCom: " + ex.Message);
            }
        }

    }
}
