using PetaPoco;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;
using GruppoCap.Core;
using System.Collections.Generic;
using GruppoCap.DAL;
using GruppoCap.Core.Data;

namespace GruppoCap.Authentication.Core
{
    public class ApplicationRepo : RepositoryBase<Application>, IApplicationRepo
    {
        // FILTER
        public ISubCollection<Application> Filter(String text = "", Boolean onlyActive = false, Int32? pageNumber = null, Int32? pageSize = null, Boolean upperizeParameters = false)
        {
            var sql = Sql.Builder.Append(" SELECT APPLICATION_ID, DESCRIPTION, IS_ACTIVE, IS_IN_MAINTENANCE, MAINTENANCE_WARNING, CREATION_MOMENT, CREATION_USER_ID, LAST_UPDATE_MOMENT, LAST_UPDATE_USER_ID FROM REVO_AUTH_APPLICATIONS WHERE 1 = 1 ");

            if (text.IsNullOrWhiteSpace() == false)
            {
                if (upperizeParameters)
                    sql.Append(" AND ((UPPER(APPLICATION_ID) LIKE @0) OR (UPPER(DESCRIPTION) LIKE @0))", "%{0}%".FormatWith(text.ToUpper()));
                else
                    sql.Append(" AND ((USER_ID LIKE @0) OR (DESCRIPTION LIKE @0))", "%{0}%".FormatWith(text));
            }

            if (onlyActive)
                sql.Append(" AND IS_ACTIVE = 1 ");

            sql.OrderBy("APPLICATION_ID");

            IEnumerable<Application> _apps = null;
            Page<Application> _pagedUsers = null;

            if (pageNumber.HasValue && pageNumber > 0)
            {
                _pagedUsers = db.Page<Application>(pageNumber.Value.CoerceTo<Int64>(), pageSize.HasValue ? pageSize.CoerceTo<Int64>() : 15, sql);
            }
            else
            {
                _apps = db.Query<Application>(sql);
            }

            if (_pagedUsers != null)
                return _pagedUsers.ToSubCollection<Application>();

            return _apps.ToSubCollection<Application>();
        }


        // APPLICATION IDs RELATED TO USER ID
        public IList<String> EnabledApplicationIdsForUser(String userId)
        {
            var sql = Sql.Builder
                .Append(" SELECT APPLICATION_ID FROM REVO_AUTH_USERS_APPLICATIONS ")
                .Append(" WHERE UPPER(USER_ID) = @0 ", userId.ToUpper())
            ;

            return db.Query<String>(sql).ToList<String>();
        }

        // USER IDs ENABLED TO APPLICATION
        public IList<String> EnabledUserIdsForApplication(String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT USER_ID FROM REVO_AUTH_USERS_APPLICATIONS ")
                .Append(" WHERE UPPER(APPLICATION_ID) = @0 ", applicationId.ToUpper())
            ;

            return db.Query<String>(sql).ToList<String>();
        }


        // EXISTs RELATION
        public Boolean IsUserEnabledForApplication(String applicationId, String userId)
        {
            var sql = Sql.Builder
                .Append(" SELECT APPLICATION_ID FROM REVO_AUTH_USERS_APPLICATIONS ")
                .Append(" WHERE UPPER(APPLICATION_ID) = @0 AND UPPER(USER_ID) = @1 ", applicationId.ToUpper(), userId.ToUpper())
            ;

            IEnumerable<String> result = db.Query<String>(sql);

            return result.HasValues();
        }

        // INSERT USER APPLICATION
        public IInsertOperationResult InsertUserApplication(String applicationId, String userId)
        {
            try
            {
                var sql = Sql.Builder
                                .Append(" INSERT INTO REVO_AUTH_USERS_APPLICATIONS ")
                                .Append(" ( USER_ID, APPLICATION_ID ) ")
                                .Append(" VALUES (@0, @1)", userId, applicationId)
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

        // REMOVE USER APPLICATION
        public IDeleteOperationResult RemoveUserApplication(String applicationId, String userId)
        {
            try
            {
                var sql = Sql.Builder
                                .Append(" DELETE FROM REVO_AUTH_USERS_APPLICATIONS ")
                                .Append(" WHERE APPLICATION_ID = @0 AND USER_ID = @1 ", applicationId, userId)
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

        // ENABLED APPLICATIONs FOR USER
        public ISubCollection<Application> EnabledApplicationsForUser(String userId)
        {
            var sql = Sql.Builder
                .Append(" SELECT    * ")
                .Append(" FROM      REVO_AUTH_APPLICATIONS A ")
                .Append("           INNER JOIN REVO_AUTH_USERS_APPLICATIONS UA ON A.APPLICATION_ID = UA.APPLICATION_ID ")
                .Append(" WHERE     A.IS_ACTIVE = 1 ")
                .Append("           AND UPPER(UA.USER_ID) = @0 ", userId.ToUpper())
            ;

            return db.Query<Application>(sql).ToSubCollection<Application>();
        }
    }
}
