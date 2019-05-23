using GruppoCap.Core;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security.PEM
{
    public class SqlPermissionRepo : SqlRepositoryBase<Permission>, IPermissionRepo
    {
        // GET PERMISSION
        public IPermission GetByCode(String permissionCode)
        {
            try
            {
                return db.SingleOrDefault<Permission>(" WHERE PERMISSION_CODE = @0 ", permissionCode);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION

                throw ex;
            }
        }

        // BROWSE PERMISSIONS
        public List<IPermission> BrowsePermissions(Boolean includePrivileged = false)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM REVO_PERMISSIONS ");
            sql.Append(" WHERE 1 = 1 ");

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            sql.Append(" ORDER BY CATEGORY_NAME, PERMISSION_CODE ");

            IEnumerable<Permission> _permissions = null;
            _permissions = db.Query<Permission>(sql);

            return _permissions.ToList<IPermission>();
        }

        // GET USER GRANT DIRECT
        public Boolean? GetUserGrantDirect(String permissionId, Object userId)
        {
            try
            {
                var sql = Sql.Builder.Append(" SELECT GRANTED FROM REVO_GRANT_FOR_USER ");
                sql.Append(" WHERE USER_ID = @0 AND PERMISSION_ID = @1 ", userId, permissionId);

                return db.ExecuteScalar<Boolean>(sql);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // DELETE ALL GRANTS BY USER ID
        public IDeleteOperationResult DeleteAllGrantsByUserId(Object userId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_USER ");
                sql.Append(" WHERE USER_ID = @0 ", userId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // DELETE ALL GRANTS BY PERMISSION
        public IDeleteOperationResult DeleteAllGrantsByPermission(String permissionId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_USER ");
                sql.Append(" WHERE PERMISSION_ID = @0 ", permissionId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // SET DIRECT GRANT FOR USER
        public IOperationResult SetDirectGrantForUser(String permissionId, Object userId, Boolean granted)
        {
            try
            {
                //var sql = Sql.Builder.Append(" DECLARE internalId NUMBER(10); ");
                //sql.Append(" BEGIN ");
                //sql.Append("    BEGIN ");
                //sql.Append("        SELECT Internal_Id INTO internalId FROM REVO_GRANT_FOR_USER WHERE Permission_Id = @0 AND User_Id = @1; ", permissionId, userId);
                //sql.Append("        EXCEPTION WHEN NO_DATA_FOUND THEN internalId := NULL; ");
                //sql.Append("    END; ");
                //sql.Append("    IF internalId IS NULL THEN ");
                //sql.Append("        INSERT INTO REVO_GRANT_FOR_USER (Permission_Id, User_Id, Granted) VALUES (@0, @1 , @2); ", permissionId, userId, granted);
                //sql.Append("    ELSE ");
                //sql.Append("        UPDATE REVO_GRANT_FOR_USER SET Granted = @0 WHERE Permission_Id = @1 AND User_Id = @2; ", granted, permissionId, userId);
                //sql.Append("    END IF; ");
                //sql.Append(" END; ");

                var sql = Sql.Builder.Append(" IF EXISTS ( ");
                sql.Append(" SELECT Internal_Id FROM REVO_GRANT_FOR_USER WHERE Permission_Id = @0 AND User_Id = @1 ", permissionId, userId);
                sql.Append(" ) ");
                sql.Append(" UPDATE REVO_GRANT_FOR_USER SET Granted = @0 WHERE Permission_Id = @1 AND User_Id = @2; ", granted, permissionId, userId);
                sql.Append(" ELSE ");
                sql.Append(" INSERT INTO REVO_GRANT_FOR_USER (Permission_Id, User_Id, Granted) VALUES (@0, @1 , @2); ", permissionId, userId, granted);

                db.Execute(sql);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        // DELETE DIRECT GRANT FOR USER
        public IDeleteOperationResult DeleteDirectGrantForUser(String permissionId, Object userId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_USER ");
                sql.Append(" WHERE PERMISSION_ID = @0 AND USER_ID = @1", permissionId, userId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

    }
}
