using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.Authentication.Core
{
    public class UserRepo : RepositoryBase<User>, IUserRepo
    {
        // GET BY ACCOUNT
        public User GetUserByAccount(String userId, String domain)
        {
            try
            {
                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM REVO_AUTH_USERS WHERE UPPER(USER_ID) = @0 ", userId.ToUpperInvariant());

                if (domain.IsNullOrWhiteSpace() == false)
                    sql.Append(" AND UPPER(DOMAIN )= @0 ", domain.ToUpperInvariant());

                return db.SingleOrDefault<User>(sql);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION

                throw ex;
            }
        }

        // GET BY IDs (OVERRIDE)
        public ISubCollection<User> GetByIds(Object[] ids, Boolean onlyActive = false, Boolean includePrivileged = false, Boolean upperizeParameters = false)
        {
            try
            {
                var sql = PetaPoco.Sql.Builder.Append(" SELECT * FROM REVO_AUTH_USERS ");

                sql.Append(" WHERE ");

                if (upperizeParameters)
                    sql.Append(" UPPER(USER_ID) IN (@ids)", new { ids = ids.UpperizeStringArray() });
                else
                    sql.Append(" USER_ID IN (@ids) ", new { ids = ids });

                if (includePrivileged == false)
                    sql.Append(" AND IS_PRIVILEGED = 0 ");

                if (onlyActive)
                    sql.Append(" AND IS_ACTIVE = 1 ");

                sql.Append(" ORDER BY USER_ID ");

                return db.Query<User>(sql).ToSubCollection<User>();
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // FILTER
        public ISubCollection<User> Filter(String text = "", Boolean onlyActive = false, Boolean includePrivileged = false, Int32? companyId = null, Int32? pageNumber = null, Int32? pageSize = null, Boolean upperizeParameters = false)
        {
            var sql = Sql.Builder.Append(" SELECT USER_ID, FIRST_NAME, LAST_NAME, DOMAIN, COMPANY_ID, IS_ACTIVE, IS_PRIVILEGED, CREATION_MOMENT, CREATION_USER_ID, LAST_UPDATE_MOMENT, LAST_UPDATE_USER_ID FROM REVO_AUTH_USERS WHERE 1 = 1 ");

            if (text.IsNullOrWhiteSpace() == false)
            {
                if(upperizeParameters)
                    sql.Append(" AND (UPPER(USER_ID) LIKE @0)", "%{0}%".FormatWith(text.ToUpper()));
                else
                    sql.Append(" AND (USER_ID LIKE @0)", "%{0}%".FormatWith(text));
            }

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            if (onlyActive)
                sql.Append(" AND IS_ACTIVE = 1 ");

            if (companyId.HasValue)
                sql.Append(" AND COMPANY_ID = @0 ", companyId.Value);

            sql.OrderBy("USER_ID");

            IEnumerable<User> _users = null;
            Page<User> _pagedUsers = null;

            if (pageNumber.HasValue && pageNumber > 0)
            {
                _pagedUsers = db.Page<User>(pageNumber.Value.CoerceTo<Int64>(), pageSize.HasValue ? pageSize.CoerceTo<Int64>() : 15, sql);
            }
            else
            {
                _users = db.Query<User>(sql);
            }

            if (_pagedUsers != null)
                return _pagedUsers.ToSubCollection<User>();

            return _users.ToSubCollection<User>();
        }

        // GET LAST CREATED
        public ISubCollection<User> GetLastCreated(Boolean includePrivileged = false, Int32 howMany = 5)
        {
            return GetLast("CREATION_MOMENT", includePrivileged, howMany);
        }

        // GET LAST UPDATED
        public ISubCollection<User> GetLastUpdated(Boolean includePrivileged = false, Int32 howMany = 5)
        {
            return GetLast("LAST_UPDATE_MOMENT", includePrivileged, howMany);
        }

        // GET LAST (GENERIC)
        private ISubCollection<User> GetLast(String orderField, Boolean includePrivileged = false, Int32 howMany = 5)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM ( ");
            sql.Append(" SELECT * FROM REVO_AUTH_USERS ");

            sql.Append(" WHERE 1 = 1 ");

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");
            
            sql.Append(" ORDER BY NVL({0}, TO_DATE('1/1/1900', 'dd/MM/yyyy')) DESC ".FormatWith(orderField));
            sql.Append(" ) WHERE ROWNUM <= {0} ".FormatWith(howMany));
       
            IEnumerable<User> _users = null;
            _users = db.Query<User>(sql);

            return _users.ToSubCollection<User>();
        }




        // COMPANIES ASSOCIATIONS

        // COMPANY IDs RELATED TO USER ID
        public IEnumerable<Int32> GetAllCompanyIdsForUser(String userId)
        {
            try
            {
                var sql = Sql.Builder
                    .Append(" SELECT DISTINCT COMPANY_ID FROM REVO_AUTH_USERS_COMPANIES ")
                    .Append(" WHERE UPPER(USER_ID) = @0 ", userId.ToUpper())
                ;

                IEnumerable<Int32> _ids = db.Query<Int32>(sql);

                return _ids;
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                throw ex;
            }
        }

        // INSERT USER GROUPING
        public IInsertOperationResult InsertUserCompany(Int32 companyId, String userId, Boolean isMain = false)
        {
            try
            {
                var sql = Sql.Builder
                    .Append(" INSERT INTO REVO_AUTH_USERS_COMPANIES ")
                    .Append(" ( USER_ID, COMPANY_ID, FLAG_MAIN ) ")
                    .Append(" VALUES (@0, @1, @2)", userId, companyId, isMain)
                ;

                Int32 result = db.Execute(sql);

                return new InsertOperationResult(result, true);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return new InsertOperationResult(null, false, ex.Message);
            }
        }

        // REMOVE USER GROUPING
        public IDeleteOperationResult RemoveUserCompany(Int32 companyId, String userId)
        {
            try
            {
                var sql = Sql.Builder
                                .Append(" DELETE FROM REVO_AUTH_USERS_COMPANIES ")
                                .Append(" WHERE COMPANY_ID = @0 AND USER_ID = @1 ", companyId, userId)
                            ;

                Int32 result = db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // EXISTs USER - COMPANY RELATION
        public Boolean IsUserEnabledForCompany(Int32 companyId, String userId)
        {
            var sql = Sql.Builder
                .Append(" SELECT COMPANY_ID FROM REVO_AUTH_USERS_COMPANIES ")
                .Append(" WHERE UPPER(USER_ID) = @0 AND COMPANY_ID = @1 ", userId.ToUpper(), companyId)
            ;

            IEnumerable<Int32> result = db.Query<Int32>(sql);

            return result.HasValues();
        }

        // MAIN GROUPING ID FOR USER AND APPLICATION 
        public Int32 MainCompanyIdForUser(String userId)
        {
            var sql = Sql.Builder
                .Append(" SELECT COMPANY_ID FROM REVO_AUTH_USERS_COMPANIES ")
                .Append(" WHERE UPPER(UG.USER_ID) = @0 ", userId.ToUpper())
                .Append(" AND UG.FLAG_MAIN = 1 ")
            ;

            return db.SingleOrDefault<Int32>(sql);
        }

        // SET USER COMPANY AS MAIN
        public IUpdateOperationResult SetUserCompanyAsMain(Int32 companyId, String userId, String applicationId)
        {
            try
            {
                var sql = Sql.Builder.Append(" UPDATE REVO_AUTH_USERS_COMPANIES ")
                            .Append(" SET REVO_AUTH_USERS_COMPANIES.FLAG_MAIN = 0 ")
                            .Append(" WHERE UPPER(USER_ID) = @0 ", userId.ToUpper())
                ;

                Int32 result = db.Execute(sql);

                sql = Sql.Builder.Append(" UPDATE REVO_AUTH_USERS_COMPANIES ")
                                .Append(" SET FLAG_MAIN = 1 ")
                                .Append(" WHERE COMPANY_ID = @0 AND UPPER(USER_ID) = @1 ", companyId, userId.ToUpper());

                result = db.Execute(sql);

                return new UpdateOperationResult(true);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return new UpdateOperationResult(false, ex.Message);
            }
        }
    }
}
