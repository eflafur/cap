using GruppoCap.Core;
using GruppoCap.Core.Data;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace GruppoCap.DAL
{
    public class SqlRepositoryBase
    {
        private Database _db;
        public Database db
        {
            get { return _db ?? (_db = new Database(Ambient.CurrentApplicationConnectionString(), "System.Data.SqlClient")); }
        }
    }

    public class SqlRepositoryBase<T>
        where T : class, IEntity, new()
    {
        private Database _db;
        public Database db
        {
            get 
            {
                return _db ?? (_db = new Database(Ambient.CurrentApplicationConnectionString(), "System.Data.SqlClient")); 
            }
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
                    sql.Append(" WHERE {0} = @0 ".FormatWith(entityIdColumnName), id.ToString().ToUpperInvariant());
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
                    sql.Append(" {0} IN (@ids)".FormatWith(entityIdColumnName), new { ids = ids.UpperizeStringArray() } );
                }
                else
                {
                    sql.Append(" WHERE {0} IN (@ids) ".FormatWith(entityIdColumnName), new { ids = ids.UpperizeStringArray() } );
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

            var pd = PetaPoco.Database.PocoData.ForType(typeof(T));
            var tableName = pd.TableInfo.TableName;

            var sql = Sql.Builder.Append(" SELECT TOP {0} * FROM {1} ".FormatWith(howMany, tableName));
            sql.Append(" ORDER BY {0} DESC ".FormatWith(orderField));
            
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
                return new InsertOperationResult(entity.EntityId, true);
            }
            catch (Exception ex)
            {
                String[] _warnings = new String[] { "TableName = {0}".FormatWith(tableName), "PrimaryKey = {0}".FormatWith(entityIdColumnName) };
                return new InsertOperationResult(null, false, ex.Message, _warnings);
            }
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



        public IBulkInsertOperationResult BulkInsert(List<T> entities, IList<SqlBulkCopyColumnMapping> mappings)
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
                    Int32 idx = 0;
                    pd.Columns.ToList().ForEach(c =>
                    {
                        DbColumnsArray.AppendFormat("{0},", c.Value.ColumnName);
                        DbColumnParametersArray.AppendFormat("@{0},", idx);
                        idx++;
                    });

                    String commandText = String.Format("insert into {0} ({1}) values ({2})", tableName, DbColumnsArray.ToString().Substring(0, DbColumnsArray.Length - 1), DbColumnParametersArray.ToString().Substring(0, DbColumnParametersArray.Length - 1));
                    
                    SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)db.Connection);

                    foreach(SqlBulkCopyColumnMapping map in mappings)
                    {
                        bulk.ColumnMappings.Add(map);
                    }

                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("InternalId", "InternalId"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OperationId", "OperationId"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("UserName", "UserName"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OperationMoment", "OperationMoment"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("OperationDescription", "Operation"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IPAddress", "IPAddress"));
                    //bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ApplicationId", "ApplicationId"));

                    bulk.DestinationTableName = pd.TableInfo.TableName;
                    bulk.WriteToServer(entities.ToDataTable());

                    return new BulkInsertOperationResult(1, true);
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


        private static DataTable ToDataTable<O>(List<O> items)
        {
            DataTable dataTable = new DataTable(typeof(O).Name);

            // GET ALL THE PROPERTIES
            PropertyInfo[] Props = typeof(O).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                // DEFINING TYPE OF DATA COLUMN GIVES PROPER DATA TABLE 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                // SETTING COLUMN NAMES AS PROPERTY NAMES
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (O item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // INSERTING PROPERTY VALUES TO DATATABLE ROWS
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            // PUT A BREAKPOINT HERE AND CHECK DATATABLE
            return dataTable;
        }

    }
}
