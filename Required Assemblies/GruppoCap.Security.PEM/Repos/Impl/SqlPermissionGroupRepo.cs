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
    public class SqlPermissionGroupRepo : SqlRepositoryBase<PermissionGroup>, IPermissionGroupRepo
    {
        // BROWSE PERMISSION GROUPS
        public List<IPermissionGroup> BrowsePermissionGroups(Boolean includePrivileged)
        {
            var sql = Sql.Builder.Append(" SELECT * FROM REVO_PERMISSIONGROUPS ");
            sql.Append(" WHERE 1 = 1 ");

            if (includePrivileged == false)
                sql.Append(" AND IS_PRIVILEGED = 0 ");

            IEnumerable<PermissionGroup> _groups = null;
            _groups = db.Query<PermissionGroup>(sql);

            return _groups.ToList<IPermissionGroup>();
        }

        // GET GROUP GRANT DIRECT
        public Boolean? GetGroupGrantDirect(String permissionId, Object groupId)
        {
            try
            {
                var sql = Sql.Builder.Append(" SELECT GRANTED FROM REVO_GRANT_FOR_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_GROUP_ID = @0 AND PERMISSION_ID = @1 ", groupId, permissionId);

                return db.ExecuteScalar<Boolean>(sql);
            }
            catch (Exception ex)
            {
                // LOG EXCEPTION
                return null;
            }
        }

        // DELETE ALL GRANTS BY PERMISSION GROUP ID
        public IDeleteOperationResult DeleteAllGrantsByPermissionGroupId(Object groupId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_GROUP_ID = @0 ", groupId);

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
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_ID = @0 ", permissionId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // SET DIRECT GRANT FOR PERMISSION GROUP
        public IOperationResult SetDirectGrantForPermissionGroup(String permissionId, Object groupId, Boolean granted)
        {
            try
            {
                //var sql = Sql.Builder.Append(" DECLARE internalId NUMBER(10); ");
                //sql.Append(" BEGIN ");
                //sql.Append("    BEGIN ");
                //sql.Append("        SELECT INTERNAL_ID INTO internalId FROM REVO_GRANT_FOR_PERMISSIONGROUP WHERE PERMISSION_ID = @0 AND PERMISSION_GROUP_ID = @1; ", permissionId, groupId);
                //sql.Append("        EXCEPTION WHEN NO_DATA_FOUND THEN internalId := NULL; ");
                //sql.Append("    END; ");
                //sql.Append("    IF internalId IS NULL THEN ");
                //sql.Append("        INSERT INTO REVO_GRANT_FOR_PERMISSIONGROUP (PERMISSION_ID, PERMISSION_GROUP_ID, GRANTED) VALUES (@0, @1 , @2); ", permissionId, groupId, granted);
                //sql.Append("    ELSE ");
                //sql.Append("        UPDATE REVO_GRANT_FOR_PERMISSIONGROUP SET GRANTED = @0 WHERE PERMISSION_ID = @1 AND PERMISSION_GROUP_ID = @2; ", granted, permissionId, groupId);
                //sql.Append("    END IF; ");
                //sql.Append(" END; ");

                var sql = Sql.Builder.Append(" IF EXISTS ( ");
                sql.Append(" SELECT Internal_Id FROM REVO_GRANT_FOR_PERMISSIONGROUP WHERE PERMISSION_ID = @0 AND PERMISSION_GROUP_ID = @1 ", permissionId, groupId);
                sql.Append(" ) ");
                sql.Append(" UPDATE REVO_GRANT_FOR_PERMISSIONGROUP SET Granted = @0 WHERE PERMISSION_ID = @1 AND PERMISSION_GROUP_ID = @2; ", granted, permissionId, groupId);
                sql.Append(" ELSE ");
                sql.Append(" INSERT INTO REVO_GRANT_FOR_PERMISSIONGROUP (PERMISSION_ID, PERMISSION_GROUP_ID, GRANTED) VALUES (@0, @1 , @2); ", permissionId, groupId, granted);

                db.Execute(sql);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        // DELETE DIRECT GRANT FOR PERMISSION GROUP
        public IDeleteOperationResult DeleteDirectGrantForPermissionGroup(String permissionId, Object groupId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_GRANT_FOR_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_ID = @0 AND USER_ID = @1", permissionId, groupId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // PUT USER IN PERMISSION GROUP
        public IOperationResult PutUserInPermissionGroup(Object userId, Object groupId)
        {
            try
            {
                //var sql = Sql.Builder.Append(" DECLARE internalId NUMBER(10); ");
                //sql.Append(" BEGIN ");
                //sql.Append("    BEGIN ");
                //sql.Append("        SELECT INTERNAL_ID INTO internalId FROM REVO_USER_IN_PERMISSIONGROUP WHERE PERMISSION_GROUP_ID = @0 AND USER_ID = @1; ", groupId, userId);
                //sql.Append("        EXCEPTION WHEN NO_DATA_FOUND THEN internalId := NULL; ");
                //sql.Append("    END; ");
                //sql.Append("    IF internalId IS NULL THEN ");
                //sql.Append("        INSERT INTO REVO_USER_IN_PERMISSIONGROUP (PERMISSION_GROUP_ID, USER_ID) VALUES (@0, @1); ", groupId, userId);
                //sql.Append("    END IF; ");
                //sql.Append(" END; ");

                var sql = Sql.Builder.Append(" IF NOT EXISTS ( ");
                sql.Append(" SELECT Internal_Id FROM REVO_USER_IN_PERMISSIONGROUP WHERE PERMISSION_GROUP_ID = @0 AND USER_ID = @1 ", groupId, userId);
                sql.Append(" ) ");
                sql.Append(" INSERT INTO REVO_USER_IN_PERMISSIONGROUP (PERMISSION_GROUP_ID, USER_ID) VALUES (@0, @1); ", groupId, userId);

                db.Execute(sql);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        // REMOVE USER FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveUserFromPermissionGroup(Object userId, Object groupId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_USER_IN_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_GROUP_ID = @0 AND USER_ID = @1", groupId, groupId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // REMOVE USER FROM ALL PERMISSION GROUPS
        public IDeleteOperationResult RemoveUserFromAllPermissionGroups(Object userId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_USER_IN_PERMISSIONGROUP ");
                sql.Append(" WHERE USER_ID = @0", userId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // REMOVE ALL USERS FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveAllUsersFromPermissionGroup(Object groupId)
        {
            try
            {
                var sql = Sql.Builder.Append(" DELETE FROM REVO_USER_IN_PERMISSIONGROUP ");
                sql.Append(" WHERE PERMISSION_GROUP_ID = @0", groupId);

                db.Execute(sql);

                return new DeleteOperationResult(true);
            }
            catch (Exception ex)
            {
                return new DeleteOperationResult(false, ex.Message);
            }
        }

        // GET FIRST PERMISSION GROUP ID FOR USER
        public Object GetFirstPermissionGroupIdForUser(Object userId)
        {
            try
            {
                var sql = Sql.Builder.Append(" SELECT PERMISSION_GROUP_ID FROM REVO_USER_IN_PERMISSIONGROUP ");
                sql.Append(" WHERE USER_ID = @0 ", userId);

                return db.ExecuteScalar<String>(sql);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
