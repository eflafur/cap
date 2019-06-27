using GruppoCap.Core;
using GruppoCap.Core.Data;
using Oracle.ManagedDataAccess.Client;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruppoCap.DAL
{
    public class RepositoryBase
    {
        private Database _db;
        public Database db
        {
            get { return _db ?? (_db = new Database(Ambient.CurrentApplicationConnectionString(), new PetaPoco.OracleProvider())); }
        }

        public void SetSharedDatabase(PetaPoco.Database sharedDb)
        {
            _db = sharedDb;
        }
    }

    public class RepositoryBase<T>
        where T : class, IEntity, new()
    {
        //Identifica l'ambient per leggere la connection string del nodo ConnectionString di web.config
        //Se il paramentro non viene valorizzato, la stringa di connessione viene letta utilizzando il parametro Revo.Ambient
        //L'implementazione è stata utilizzata per creare un servizio di sincronizzazione che utilizza 2 stringhe di connessione differenti.
        //La property può essere impostata mediante configurazione di IOC, impostando il tag AmbientConnectionString come da esempio:
        //
        //<component id="componentName"
        //  service="GruppoCap.Core.IRepository`1[[SyncService.Laboratorio.Core.LaboratorioHeader, SyncService.Laboratorio]], GruppoCap.Core"
        //  type="SyncService.Laboratorio.Core.LaboratorioHeaderRepo, SyncService.Laboratorio"
        //  lifestyle="singleton" >
        //  <forwardedTypes>
        //    <add service = "SyncService.Laboratorio.Core.ILaboratorioHeaderRepo, SyncService.Laboratorio" />
        //  </ forwardedTypes >
        //  < parameters >
        //
        //    < AmbientConnectionString > source </ AmbientConnectionString >
        //
        //  </ parameters >
        //</ component >
        public string AmbientConnectionString { get; set; }
        private Database _db;
        public Database db
        {
            get { return _db ?? (_db = new Database(AmbientConnectionString.IsNullOrEmpty() ? Ambient.CurrentApplicationConnectionString() : Ambient.ApplicationConnectionString(AmbientConnectionString), new PetaPoco.OracleProvider())); }
        }

        public void SetSharedDatabase(PetaPoco.Database sharedDb)
        {
            _db = sharedDb;
        }

        private PetaPoco.Database.PocoData _pocoData;
        public PetaPoco.Database.PocoData PocoData
        {
            get { return _pocoData ?? (_pocoData = PetaPoco.Database.PocoData.ForType(typeof(T))); }
        }

        public String EntityTableName
        {
            get { return PocoData.TableInfo.TableName; }
        }

        public String EntityPrimaryKey
        {
            get { return PocoData.TableInfo.PrimaryKey; }
        }

        public T CreateEntityInstance()
        {
            return new T();
        }


        // GET BY ID
        public T GetById(Object id)
        {
            try
            {
                var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
                var tableName = pd.TableInfo.TableName;
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM {0} ".FormatWith(tableName));

                if (pd.Columns[entityIdColumnName].PropertyInfo.PropertyType == typeof(String))
                    sql.Append(" WHERE UPPER({0}) = @0 ".FormatWith(entityIdColumnName), id.ToString().ToUpperInvariant());
                else
                    sql.Append(" WHERE {0} = @0 ".FormatWith(entityIdColumnName), id);

                return db.SingleOrDefault<T>(sql);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // GET BY IDS
        public ISubCollection<T> GetByIds(Object[] ids)
        {
            try
            {
                var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
                var tableName = pd.TableInfo.TableName;
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM {0} ".FormatWith(tableName));

                sql.Append(" WHERE ");

                if (pd.Columns[entityIdColumnName].PropertyInfo.PropertyType == typeof(String))
                {
                    sql.Append(" UPPER({0}) IN (@ids)".FormatWith(entityIdColumnName), new { ids = ids.UpperizeStringArray() });
                }
                else
                {
                    sql.Append(" {0} IN (@ids) ".FormatWith(entityIdColumnName), new { ids = ids.UpperizeStringArray() });
                }

                if (typeof(IOrderableEntity).IsAssignableFrom(typeof(T)))
                    sql.Append(" ORDER BY POSITION ");
                else
                    sql.Append(" ORDER BY {0} ".FormatWith(entityIdColumnName));

                return db.Query<T>(sql).ToSubCollection<T>();
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // LIST
        public ISubCollection<T> List()
        {
            try
            {
                var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
                var tableName = pd.TableInfo.TableName;
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM {0} ".FormatWith(tableName));

                if (typeof(IOrderableEntity).IsAssignableFrom(typeof(T)))
                    sql.Append(" ORDER BY POSITION ");
                else
                    sql.Append(" ORDER BY {0} ".FormatWith(entityIdColumnName));

                return db.Query<T>(sql).ToSubCollection<T>();
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // LIST
        public ISubCollection<T> OrderedList(String orderField)
        {
            try
            {
                var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
                var tableName = pd.TableInfo.TableName;
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM {0} ".FormatWith(tableName));

                if (orderField.IsNullOrWhiteSpace())
                    sql.Append(" ORDER BY {0} ".FormatWith(entityIdColumnName));
                else
                    sql.Append(" ORDER BY {0} ".FormatWith(orderField));

                return db.Query<T>(sql).ToSubCollection<T>();
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }


        // GET LAST CREATED
        public ISubCollection<T> GetLastCreated(Int32 howMany = 5)
        {
            return GetLast<T>("CREATION_MOMENT", howMany);
        }

        // GET LAST UPDATED
        public ISubCollection<T> GetLastUpdated(Int32 howMany = 5)
        {
            return GetLast<T>("LAST_UPDATE_MOMENT", howMany);
        }

        // GET LAST (GENERIC)
        private ISubCollection<T> GetLast<E>(String orderField, Int32 howMany = 5)
        {
            if (typeof(ITrackedEntity).IsAssignableFrom(typeof(E)) == false)
            {
                return null;
            }

            var pd = PetaPoco.Database.PocoData.ForType(typeof(E));
            var tableName = pd.TableInfo.TableName;

            var sql = Sql.Builder.Append(" SELECT * FROM ( ");
            sql.Append(" SELECT * FROM {0} ".FormatWith(tableName));

            sql.Append(" ORDER BY NVL({0}, TO_DATE('1/1/1900', 'dd/MM/yyyy')) DESC ".FormatWith(orderField));
            sql.Append(" ) WHERE ROWNUM <= {0} ".FormatWith(howMany));

            IEnumerable<T> _list = null;
            _list = db.Query<T>(sql);

            return _list.ToSubCollection<T>();
        }


        public IInsertOperationResult Insert(T entity)
        {
            var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
            var tableName = pd.TableInfo.TableName;
            var entityIdColumnName = pd.TableInfo.PrimaryKey;
            try
            {
                db.Insert(entity);

                if (string.IsNullOrEmpty(entityIdColumnName) || entityIdColumnName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                {
                    return new InsertOperationResult(entity.EntityId, true);
                }
                else
                {
                    Dictionary<string, object> createdEntityKeys = null;
                    createdEntityKeys = entityIdColumnName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                       .ToDictionary(a => pd.Columns[a.Trim()].PropertyInfo.Name, a => (object)GetPropValue(entity, pd.Columns[a.Trim()].PropertyInfo.Name));
                    return new InsertOperationResult(createdEntityKeys, true);
                }
            }
            catch (Exception ex)
            {
                String[] _warnings = new String[] { "TableName = {0}".FormatWith(tableName), "PrimaryKey = {0}".FormatWith(entityIdColumnName) };
                return new InsertOperationResult(null, false, ex.Message, _warnings);
            }
        }
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public IUpdateOperationResult Update(T entity)
        {
            try
            {
                db.Update(entity);
                return new UpdateOperationResult(true);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return new UpdateOperationResult(false, ex.Message);
            }
        }

        public IDeleteOperationResult DeleteById(Object id)
        {
            try
            {
                var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                db.Delete<T>("WHERE {0} = @0".FormatWith(entityIdColumnName), id);
                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        public IBulkInsertOperationResult BulkInsert(List<T> entities)
        {

            if (entities == null)
                return null;

            var pd = PetaPoco.Database.PocoData.ForType(entities[0].GetType());
            var tableName = pd.TableInfo.TableName;
            try
            {
                db.OpenSharedConnection();

                try
                {
                    StringBuilder DbColumnsArray = new StringBuilder();
                    StringBuilder DbColumnParametersArray = new StringBuilder();
                    int idx = 0;
                    pd.Columns.ToList().ForEach(c =>
                    {
                        DbColumnsArray.AppendFormat("{0},", c.Value.ColumnName);
                        DbColumnParametersArray.AppendFormat(":{0},", c.Value.ColumnName);
                        idx++;
                    });
                    string commandText = string.Format("insert into {0} ({1}) values ({2})", tableName, DbColumnsArray.ToString().Substring(0, DbColumnsArray.Length - 1), DbColumnParametersArray.ToString().Substring(0, DbColumnParametersArray.Length - 1));
                    using (var cmd = db.CreateCommand(db.Connection, commandText))
                    {
                        pd.Columns.ToList().ForEach(c =>
                        {
                            cmd.Parameters.Add(new OracleParameter()
                            {
                                ParameterName = c.Value.ColumnName,
                                OracleDbType = c.Value.PropertyInfo.PropertyType.ToOracleType(),
                                Value = entities[0].GetType().GetProperties().Where(p => p.Name == c.Value.PropertyInfo.Name).Select(p => entities.Select(t => p.GetValue(t, null)).ToList()).ToList().FirstOrDefault().Cast<object>().ToList().ToArray()
                            });
                        });
                        ((OracleCommand)cmd).ArrayBindCount = entities.Count;
                        int numberOfRecords = cmd.ExecuteNonQuery();
                        return new BulkInsertOperationResult(numberOfRecords, true);
                    }
                }
                finally
                {
                    db.CloseSharedConnection();
                }

            }
            catch (Exception ex)
            {
                String[] _warnings = new String[] { string.Format("TableName = {0}", tableName) };
                return new BulkInsertOperationResult(0, false, ex.Message, _warnings);
            }
        }
    }
}