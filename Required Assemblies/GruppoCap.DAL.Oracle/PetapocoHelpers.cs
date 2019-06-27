using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using GruppoCap;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace GruppoCap.DAL
{
    public static class PetapocoHelpers
    {
        // TO SUB COLLECTION
        public static ISubCollection<T> ToSubCollection<T>(this Page<T> pageResult)
        {
            if (pageResult.Items.HasValues() == false)
            {
                return SubCollection<T>.CreateEmptyCollection();
            }

            return new SubCollection<T>(
                pageResult.Items,
                new SubCollectionInfo(
                    currentPage: pageResult.CurrentPage.CoerceTo<Int32>(),
                    itemsPerPage: pageResult.ItemsPerPage.CoerceTo<Int32>(),
                    totalPages: pageResult.TotalPages.CoerceTo<Int32>(),
                    totalItems: pageResult.TotalItems.CoerceTo<Int32>()
                )
            );
        }

        // TO SUB COLLECTION
        public static ISubCollection<T> ToSubCollection<T>(this IEnumerable<T> result)
        {
            if (result.HasValues() == false)
            {
                return SubCollection<T>.CreateEmptyCollection();
            }

            return new SubCollection<T>(
                result,
                new SubCollectionInfo(
                    currentPage: 1,
                    itemsPerPage: result.Count(),
                    totalPages: 1,
                    totalItems: result.Count()
                )
            );
        }
        public static List<PetaPocoOutputParameter> ToOutputPetaPocoParameterList(this IDataParameterCollection parameters)
        {
            if (parameters == null)
                return null;

            List<PetaPocoOutputParameter> _res = new List<PetaPocoOutputParameter>();

            parameters.Cast<IDbDataParameter>().Where<IDbDataParameter>(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToList<IDbDataParameter>().ForEach(p =>
            {
                _res.Add(new PetaPocoOutputParameter(p.ParameterName, p.DbType, p.Value, p.Size));
            });

            return _res;
        }
        public static IDbDataParameter ToIDataParameter(this PetaPocoParameter petaPocoParameter, IDbCommand cmd)
        {
            if (petaPocoParameter == null)
                return null;
            IDbDataParameter _par = cmd.CreateParameter();
            _par.ParameterName = petaPocoParameter.ParameterName;
            _par.DbType = petaPocoParameter.DbType;
            _par.Direction = petaPocoParameter.Direction;
            _par.Size = petaPocoParameter.Size;
            _par.Value = petaPocoParameter.Value;
            return _par;
        }
        public static PetaPocoPrecedureResult ExecuteProcedure(this Database db, string ProcedureName, params PetaPocoParameter[] parameters)
        {
            if (db == null)
                return null;
            db.OpenSharedConnection();
            try
            {
                using (IDbCommand cmd = db.Connection.CreateCommand())
                {
                    cmd.Connection = db.Connection;
                    cmd.CommandText = ProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    parameters.ToList<PetaPocoParameter>().ForEach(p => { cmd.Parameters.Add(p.ToIDataParameter(cmd)); });
                    int _affectedRows = cmd.ExecuteNonQuery();

                    return new PetaPocoPrecedureResult(true, string.Empty, (cmd.Parameters == null ? null : cmd.Parameters.ToOutputPetaPocoParameterList()), _affectedRows);
                }
            }
            catch (Exception ex)
            {
                return new PetaPocoPrecedureResult(false, ex.ToString(), null, 0);
            }
            finally
            {
                db.CloseSharedConnection();
            }
        }

        public static OracleDbType ToOracleType(this Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(GetUnderlyingType(type));
            switch (typeCode)
            {
                case TypeCode.Int32:
                    return OracleDbType.Int32;
                case TypeCode.Int16:
                    return OracleDbType.Int16;
                case TypeCode.Int64:
                    return OracleDbType.Int64;
                case TypeCode.DateTime:
                    return OracleDbType.Date;
                case TypeCode.String:
                    return OracleDbType.Varchar2;
                case TypeCode.Boolean:
                    return OracleDbType.Byte;
                default:
                    return OracleDbType.Varchar2;
            }
        }

        public static SqlDbType ToSqlServerType(this Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(GetUnderlyingType(type));
            switch (typeCode)
            {
                case TypeCode.Int32:
                    return SqlDbType.Int;
                case TypeCode.Int16:
                    return SqlDbType.SmallInt;
                case TypeCode.Int64:
                    return SqlDbType.BigInt;
                case TypeCode.DateTime:
                    return SqlDbType.DateTime;
                case TypeCode.String:
                    return SqlDbType.NVarChar;
                case TypeCode.Boolean:
                    return SqlDbType.Bit;
                default:
                    return SqlDbType.NVarChar;
            }
        }

        private static Type GetUnderlyingType(this Type source)
        {
            if (source.IsGenericType
                && (source.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                // source is a Nullable type so return its underlying type
                return Nullable.GetUnderlyingType(source);
            }

            // source isn't a Nullable type so just return the original type
            return source;
        }
    }
    public class PetaPocoOutputParameter : PetaPocoParameter
    {
        internal PetaPocoOutputParameter(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            Direction = ParameterDirection.Output;
        }
        public PetaPocoOutputParameter(string parameterName, DbType dbType)
        {
            if (dbType == DbType.String)
                throw new ApplicationException("Output parameters string MUST have size value greater than 0");
            ParameterName = parameterName;
            DbType = dbType;
            Direction = ParameterDirection.Output;
        }
        public PetaPocoOutputParameter(string parameterName, DbType dbType, int size)
        {
            if (dbType == DbType.String && size == 0)
                throw new ApplicationException("Output parameters string MUST have size value greater than 0");
            ParameterName = parameterName;
            DbType = dbType;
            Direction = ParameterDirection.Output;
            Size = size;
        }
        internal PetaPocoOutputParameter(string parameterName, DbType dbType, object value, int size)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            Direction = ParameterDirection.Output;
            Size = size;
        }
    }
    public class PetaPocoInputParameter : PetaPocoParameter
    {
        public PetaPocoInputParameter(string parameterName, DbType dbType, object value)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            Direction = ParameterDirection.Input;
        }
        public PetaPocoInputParameter(string parameterName, DbType dbType, object value, int size)
        {
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            Direction = ParameterDirection.Input;
            Size = size;
        }
        //public PetaPocoInputParameter(string parameterName, DbType dbType, int size)
        //{
        //    ParameterName = parameterName;
        //    DbType = dbType;
        //    Direction = ParameterDirection.Input;
        //    Size = size;
        //}

    }
    public abstract class PetaPocoParameter
    {
        public int Size { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
    public class PetaPocoPrecedureResult
    {
        internal PetaPocoPrecedureResult(bool result, string errorMessage, List<PetaPocoOutputParameter> outputParams, int affectedRows)
        {
            Result = result;
            ErrorMessage = errorMessage;
            OutputParams = outputParams;
            AffectedRows = affectedRows;
        }
        public bool Result { get; private set; }
        public string ErrorMessage { get; private set; }
        public List<PetaPocoOutputParameter> OutputParams { get; private set; }
        public int AffectedRows { get; private set; }
    }
}
