using GruppoCap.Core;
using GruppoCap.Core.Data;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.DAL.SqlServer
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
        private IMapper _standardMapper = new StandardMapper();

        private IDatabase _db;
        public IDatabase db
        {
            get 
            {
                //return _db ?? (_db = DatabaseConfiguration.Build()
                //                    .UsingConnectionString(Ambient.CurrentApplicationConnectionString)
                //                    .UsingProvider<SqlServerDatabaseProvider>()
                //                    .UsingDefaultMapper<ConventionMapper>(m =>
                //                    {
                //                        m.InflectTableName = (inflector, s) => inflector.Pluralise(inflector.Underscore(s));
                //                        m.InflectColumnName = (inflector, s) => inflector.Underscore(s);
                //                    })
                //                    .Create()
                //);

                return _db ?? (_db = new Database(Ambient.CurrentApplicationConnectionString(), "System.Data.SqlClient")); 


            }
        }

        private PocoData _pocoData;
        public PocoData PocoData
        {
            get { return _pocoData ?? (_pocoData = PocoData.ForType(typeof(T), _standardMapper)); }
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
                var pd = PocoData.ForType(typeof(T), _standardMapper);
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
                var pd = PocoData.ForType(typeof(T), _standardMapper);
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
                var pd = PocoData.ForType(typeof(T), _standardMapper);
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
                var pd = PocoData.ForType(typeof(T), _standardMapper);
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

            var pd = PocoData.ForType(typeof(E), _standardMapper);
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
            var pd = PocoData.ForType(typeof(T), _standardMapper);
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
                var pd = PocoData.ForType(typeof(T), _standardMapper);
                var entityIdColumnName = pd.TableInfo.PrimaryKey;

                db.Delete<T>("WHERE {0} = @0".FormatWith(entityIdColumnName), id);
                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }
    }
}
