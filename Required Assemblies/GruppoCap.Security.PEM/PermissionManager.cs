using GruppoCap.Core;
using GruppoCap.Core.Activity;
using GruppoCap.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GruppoCap.Security.PEM
{
    public class PermissionManager : IPermissionManager
    {
        private IPEMService _pemService = null;

        #region CTOR

        public PermissionManager(IPEMService pemService) // ADD CACHE SERVICE AND DURATION
        {
            _pemService = pemService;
            //_cacheService = cacheService;
            //_cacheDuration = cacheDuration;
        }

        #endregion


        // CREATE PERMISSION INSTANCE
        public IPermission CreatePermissionInstance()
        {
            return _pemService.CreatePermissionInstance();
        }

        // GET PERMISSION
        public IPermission GetPermission(String permissionCode)
        {
            return _pemService.GetPermission(permissionCode);
        }

        // INSERT PERMISSION
        public IInsertOperationResult InsertPermission(IPermission permission)
        {
            return _pemService.InsertPermission(permission);
        }

        // UPDATE PERMISSION
        public IUpdateOperationResult UpdatePermission(IPermission permission)
        {
            return _pemService.UpdatePermission(permission);
        }

        // DELETE PERMISSION
        public IDeleteOperationResult DeletePermission(String permissionCode)
        {
            return _pemService.DeletePermission(permissionCode);
        }

        // BROWSE PERMISSIONS
        public List<IPermission> BrowsePermissions(Boolean includePrivileged)
        {
            return _pemService.BrowsePermissions(includePrivileged);
        }


        // CREATE PERMISSION GROUP INSTANCE
        public IPermissionGroup CreatePermissionGroupInstance()
        {
            return _pemService.CreatePermissionGroupInstance();
        }

        // GET PERMISSION GROUP
        public IPermissionGroup GetPermissionGroup(Object permissionGroupId)
        {
            return _pemService.GetPermissionGroup(permissionGroupId);
        }

        // INSERT PERMISSION GROUP
        public IInsertOperationResult InsertPermissionGroup(IPermissionGroup group)
        {
            return _pemService.InsertPermissionGroup(group);
        }

        // UPDATE PERMISSION GROUP
        public IUpdateOperationResult UpdatePermissionGroup(IPermissionGroup group)
        {
            return _pemService.UpdatePermissionGroup(group);
        }

        // DELETE PERMISSION GROUP
        public IDeleteOperationResult DeletePermissionGroup(Object groupId)
        {
            return _pemService.DeletePermissionGroup(groupId);
        }

        // BROWSE PERMISSION GROUPS
        public List<IPermissionGroup> BrowsePermissionGroups(Boolean includePrivileged)
        {
            return _pemService.BrowsePermissionGroups(includePrivileged);
        }


        // GET USER GRANT DIRECT
        public Boolean? GetUserGrantDirect(String permissionCode, Object userId)
        {
            return _pemService.GetUserGrantDirect(permissionCode, userId);
        }

        // GET USER GRANT WITH FALLBACK
        public Boolean GetUserGrantWithFallback(String permissionCode, Object userId)
        {
            return _pemService.GetUserGrantWithFallback(permissionCode, userId);
        }

        // GET GROUP GRANT DIRECT
        public Boolean? GetGroupGrantDirect(String permissionCode, Object groupId)
        {
            return _pemService.GetGroupGrantDirect(permissionCode, groupId);
        }

        // GET GROUP GRANT WITH FALLBACK
        public Boolean GetGroupGrantWithFallback(String permissionCode, Object groupId)
        {
            return _pemService.GetGroupGrantWithFallback(permissionCode, groupId);
        }

        // DELETE ALL GRANTS BY USER ID
        public IDeleteOperationResult DeleteAllGrantsByUserId(Object userId)
        {
            return _pemService.DeleteAllGrantsByUserId(userId);
        }

        // DELETE ALL GRANTS BY PERMISSION GROUP ID
        public IDeleteOperationResult DeleteAllGrantsByPermissionGroupId(Object groupId)
        {
            return _pemService.DeleteAllGrantsByPermissionGroupId(groupId);
        }

        // DELETE ALL GRANTS BY PERMISSION
        public IDeleteOperationResult DeleteAllGrantsByPermission(String permissionCode)
        {
            return _pemService.DeleteAllGrantsByPermission(permissionCode);
        }

        // SET DIRECT GRANT FOR USER
        public IOperationResult SetDirectGrantForUser(String permissionCode, Object userId, Boolean granted)
        {
            return _pemService.SetDirectGrantForUser(permissionCode, userId, granted);
        }

        // SET DIRECT GRANT FOR PERMISSION GROUP
        public IOperationResult SetDirectGrantForPermissionGroup(String permissionCode, Object groupId, Boolean granted)
        {
            return _pemService.SetDirectGrantForPermissionGroup(permissionCode, groupId, granted);
        }

        // DELETE DIRECT GRANT FOR USER
        public IDeleteOperationResult DeleteDirectGrantForUser(String permissionCode, Object userId)
        {
            return _pemService.DeleteDirectGrantForUser(permissionCode, userId);
        }

        // DELETE DIRECT GRANT FOR PERMISSION GROUP
        public IDeleteOperationResult DeleteDirectGrantForPermissionGroup(String permissionCode, Object groupId)
        {
            return _pemService.DeleteDirectGrantForPermissionGroup(permissionCode, groupId);
        }

        // PUT USER IN PERMISSION GROUP
        public IOperationResult PutUserInPermissionGroup(Object userId, Object groupId)
        {
            return _pemService.PutUserInPermissionGroup(userId, groupId);
        }

        // REMOVE USER FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveUserFromPermissionGroup(Object userId, Object groupId)
        {
            return _pemService.RemoveUserFromPermissionGroup(userId, groupId);
        }

        // REMOVE USER FROM ALL PERMISSION GROUPS
        public IDeleteOperationResult RemoveUserFromAllPermissionGroups(Object userId)
        {
            return _pemService.RemoveUserFromAllPermissionGroups(userId);
        }

        // REMOVE ALL USERS FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveAllUsersFromPermissionGroup(Object groupId)
        {
            return _pemService.RemoveAllUsersFromPermissionGroup(groupId);
        }

        // GET FIRST PERMISSION GROUP ID FOR USER
        public Object GetFirstPermissionGroupIdForUser(Object userId)
        {
            return _pemService.GetFirstPermissionGroupIdForUser(userId);
        }



        // ENSURE PERMISSION EXISTENCE
        public void EnsurePermissionExistence(String permissionCode, String categoryName, Boolean defaultGrant, Boolean isPrivileged)
        {
            IPermission p1;

            p1 = GetPermission(permissionCode);

            if (p1 == null)
            {
                // PERMISSION NOT EXISTS -> CREATE IT
                p1 = CreatePermissionInstance();

                p1.PermissionCode = permissionCode;
                p1.CategoryName = categoryName;
                p1.DefaultGrant = defaultGrant;
                p1.IsPrivileged = isPrivileged;

                InsertPermission(p1);
            }
        }

        // ENSURE ENTITY PERMISSION SET EXISTENCE
        public void EnsureEntityPermissionSetExistence(Type entityType, Boolean defaultGrant, Boolean isPrivileged)
        {
            String alias;
            String _entityBasicActions = "create,update,delete";

            alias = entityType.Name;

            foreach (String action in _entityBasicActions.Split(','))
            {
                EnsurePermissionExistence(
                    GetEntityActionPermissionCode(alias, action)
                    , "Entities - " + alias
                    , defaultGrant
                    , isPrivileged
                );
            }

            EnsurePermissionExistence(
                GetEntityActionPermissionCode(alias, null)
                , "Entities - " + alias
                , defaultGrant
                , isPrivileged
            );

        }

        // ENSURE ENTITY PERMISSION SET EXISTENCE
        public void EnsureEntityPermissionSetExistence(Type entityType)
        {
            EnsureEntityPermissionSetExistence(entityType, true, false);
        }

        // GET ENTITY ACTION PERMISSION CODE
        public String GetEntityActionPermissionCode(String alias, String action)
        {
            String _tmp;
            _tmp = "entity.{0}".FormatWith(alias.ToLower());

            if (action.IsNullOrWhiteSpace())
                return _tmp;

            return "{0}.{1}".FormatWith(_tmp, action);
        }

        // GET ENTITY ACTION PERMISSION CODE
        public String GetEntityActionPermissionCode(String alias)
        {
            return GetEntityActionPermissionCode(alias, null);
        }
    }
}
