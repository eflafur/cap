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
    public class CapGroupingRepo : RepositoryBase<CapGrouping>, ICapGroupingRepo
    {
        // GET BY GROUPING CODE
        public CapGrouping GetByGroupingCode(String capGroupingCode)
        {
            try
            {
                return db.SingleOrDefault<CapGrouping>(" WHERE UPPER(CAPGROUPING_CODE) = @0 ", capGroupingCode.ToUpper());
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                throw ex;
            }
        }

        // GROUPING IDs RELATED TO USER ID
        public IList<String> ActiveGroupingIdsForUserAndApplication(String userId, String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT UG.CAPGROUPING_ID FROM REVO_AUTH_USERS_CAPGROUPINGS UG, REVO_AUTH_CAPGROUPINGS G")
                .Append(" WHERE UG.CAPGROUPING_ID = G.CAPGROUPING_ID ")
                .Append(" AND UPPER(UG.USER_ID) = @0 ", userId.ToUpper())
                .Append(" AND UPPER(G.APPLICATION_ID) = @0", applicationId.ToUpper())
            ;

            return db.Query<String>(sql).ToList<String>();
        }

        // GROUPING IDs ENABLED TO APPLICATION
        public IList<String> ActiveGroupingIdsForApplication(String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT CAPGROUPING_ID FROM REVO_AUTH_CAPGROUPINGS ")
                .Append(" WHERE UPPER(APPLICATION_ID) = @0 AND IS_ACTIVE = 1", applicationId.ToUpper())
            ;

            return db.Query<String>(sql).ToList<String>();
        }

        // LIST BY APPLICATION
        public ISubCollection<CapGrouping> ListByApplication(String applicationId, Boolean onlyActive = false)
        {
            var sql = Sql.Builder
                .Append(" SELECT * FROM REVO_AUTH_CAPGROUPINGS ")
                .Append(" WHERE UPPER(APPLICATION_ID) = @0 ", applicationId.ToUpper())
            ;

            if (onlyActive)
                sql.Append(" AND IS_ACTIVE = 1 ");

            return db.Query<CapGrouping>(sql).ToSubCollection<CapGrouping>();
        }


        // EXISTs RELATION
        public Boolean IsUserMemberOfGrouping(String userId, String groupingId)
        {
            var sql = Sql.Builder
                .Append(" SELECT CAPGROUPING_ID FROM REVO_AUTH_USERS_CAPGROUPINGS ")
                .Append(" WHERE UPPER(CAPGROUPING_ID) = @0 AND UPPER(USER_ID) = @1 ", groupingId.ToUpper(), userId.ToUpper())
            ;

            IEnumerable<String> result = db.Query<String>(sql);

            return result.HasValues();
        }

        // INSERT USER GROUPING
        public IInsertOperationResult InsertUserGrouping(String groupingId, String userId, Boolean isMain = false)
        {
            try
            {
                var sql = Sql.Builder
                    .Append(" INSERT INTO REVO_AUTH_USERS_CAPGROUPINGS ")
                    .Append(" ( USER_ID, CAPGROUPING_ID, FLAG_MAIN ) ")
                    .Append(" VALUES (@0, @1, @2)", userId, groupingId, isMain)
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
        public IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId)
        {
            try
            {
                var sql = Sql.Builder
                                .Append(" DELETE FROM REVO_AUTH_USERS_CAPGROUPINGS ")
                                .Append(" WHERE CAPGROUPING_ID = @0 AND USER_ID = @1 ", groupingId, userId)
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

        // ACTIVE GROUPING CODES FOR USER AND APPLICATION
        public IList<String> ActiveGroupingCodesForUserAndApplication(String userId, String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT G.CAPGROUPING_CODE FROM REVO_AUTH_USERS_CAPGROUPINGS UG ")
                .Append(" INNER JOIN REVO_AUTH_CAPGROUPINGS G ON UG.CAPGROUPING_ID = G.CAPGROUPING_ID ")
                //.Append(" INNER JOIN REVO_AUTH_USERS_APPLICATIONS UA ON UG.USER_ID = UA.USER_ID ")
                .Append(" WHERE UPPER(UG.USER_ID) = @0 ", userId.ToUpper())
                .Append(" AND UPPER(G.APPLICATION_ID) = @0", applicationId.ToUpper())
            ;

            return db.Query<String>(sql).ToList<String>();
        }

        // MAIN GROUPING ID FOR USER AND APPLICATION 
        public String MainGroupingIdForUserAndApplication(String userId, String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT UG.CAPGROUPING_ID FROM REVO_AUTH_USERS_CAPGROUPINGS UG, REVO_AUTH_CAPGROUPINGS G")
                .Append(" WHERE UG.CAPGROUPING_ID = G.CAPGROUPING_ID ")
                .Append(" AND UPPER(UG.USER_ID) = @0 ", userId.ToUpper())
                .Append(" AND UPPER(G.APPLICATION_ID) = @0", applicationId.ToUpper())
                .Append(" AND UG.FLAG_MAIN = 1 " )
            ;

            return db.SingleOrDefault<String>(sql);
        }

        // MAIN GROUPING CODE FOR USER AND APPLICATION 
        public String MainGroupingCodeForUserAndApplication(String userId, String applicationId)
        {
            var sql = Sql.Builder
                .Append(" SELECT G.CAPGROUPING_CODE FROM REVO_AUTH_USERS_CAPGROUPINGS UG, REVO_AUTH_CAPGROUPINGS G")
                .Append(" WHERE UG.CAPGROUPING_ID = G.CAPGROUPING_ID ")
                .Append(" AND UPPER(UG.USER_ID) = @0 ", userId.ToUpper())
                .Append(" AND UPPER(G.APPLICATION_ID) = @0", applicationId.ToUpper())
                .Append(" AND UG.FLAG_MAIN = 1 ")
            ;

            return db.SingleOrDefault<String>(sql);
        }

        // SET USER GROUPING AS MAIN
        public IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId)
        {
            try
            {
                var sql = Sql.Builder.Append(" UPDATE REVO_AUTH_USERS_CAPGROUPINGS ")
                                .Append(" SET REVO_AUTH_USERS_CAPGROUPINGS.FLAG_MAIN = 0 ")
                                .Append(" WHERE UPPER(USER_ID) = @0 ", userId.ToUpper())
                                .Append(" AND CAPGROUPING_ID IN ")
                                .Append(" (SELECT CAPGROUPING_ID FROM REVO_AUTH_CAPGROUPINGS WHERE UPPER(APPLICATION_ID) = @0) ", applicationId.ToUpper());
                            ;

                Int32 result = db.Execute(sql);

                sql = Sql.Builder.Append(" UPDATE REVO_AUTH_USERS_CAPGROUPINGS ")
                                .Append(" SET FLAG_MAIN = 1 ")
                                .Append(" WHERE CAPGROUPING_ID = @0 AND UPPER(USER_ID) = @1 ", groupingId, userId.ToUpper());

                result = db.Execute(sql);

                return new UpdateOperationResult(true);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return new UpdateOperationResult(false, ex.Message);
            }
        }

        // LIST BY USER (CROSS APPLICATIONs)
        public ISubCollection<CapGrouping> ListByUser(String userId, Boolean onlyActive = false, Boolean onlyMain = false)
        {
            var sql = Sql.Builder
                .Append(" SELECT    CG.* ")
                .Append(" FROM      REVO_AUTH_CAPGROUPINGS CG ")
                .Append("           INNER JOIN REVO_AUTH_USERS_CAPGROUPINGS UCG ON CG.CAPGROUPING_ID = UCG.CAPGROUPING_ID ")
                .Append(" WHERE     UPPER(UCG.USER_ID) = @0", userId.ToUpper())
            ;

            if (onlyActive)
                sql.Append(" AND IS_ACTIVE = 1 ");

            if (onlyMain)
                sql.Append(" AND UCG.FLAG_MAIN = 1 ");

            return db.Query<CapGrouping>(sql).ToSubCollection<CapGrouping>();
        }
    }
}
