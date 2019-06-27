using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Permission
{
    public interface IPermissionManager
    {

        IPermission CreatePermissionInstance();

        IPermission GetPermission(String permissionCode);

        IInsertOperationResult InsertPermission(IPermission permission);

        IUpdateOperationResult UpdatePermission(IPermission permission);

        IDeleteOperationResult DeletePermission(String permissionCode);

        List<IPermission> BrowsePermissions(Boolean includePrivileged);

        // PERMISSION GROUPs RELATED

        IPermissionGroup CreatePermissionGroupInstance();

        IPermissionGroup GetPermissionGroup(Object permissionGroupId);

        IInsertOperationResult InsertPermissionGroup(IPermissionGroup group);

        IUpdateOperationResult UpdatePermissionGroup(IPermissionGroup group);

        IDeleteOperationResult DeletePermissionGroup(Object groupId);

        List<IPermissionGroup> BrowsePermissionGroups(Boolean includePrivileged);

        // GRANTs RELATED

        Boolean? GetUserGrantDirect(String permissionCode, Object userId);

        Boolean GetUserGrantWithFallback(String permissionCode, Object userId);

        Boolean? GetGroupGrantDirect(String permissionCode, Object groupId);

        Boolean GetGroupGrantWithFallback(String permissionCode, Object groupId);

        IDeleteOperationResult DeleteAllGrantsByUserId(Object userId);

        IDeleteOperationResult DeleteAllGrantsByPermissionGroupId(Object groupId);

        IDeleteOperationResult DeleteAllGrantsByPermission(String permissionCode);

        IOperationResult SetDirectGrantForUser(String permissionCode, Object userId, Boolean granted);

        IOperationResult SetDirectGrantForPermissionGroup(String permissionCode, Object groupId, Boolean granted);

        IDeleteOperationResult DeleteDirectGrantForUser(String permissionCode, Object userId);

        IDeleteOperationResult DeleteDirectGrantForPermissionGroup(String permissionCode, Object groupId);

        IOperationResult PutUserInPermissionGroup(Object userId, Object groupId);

        IDeleteOperationResult RemoveUserFromPermissionGroup(Object userId, Object groupId);

        IDeleteOperationResult RemoveUserFromAllPermissionGroups(Object userId);

        IDeleteOperationResult RemoveAllUsersFromPermissionGroup(Object groupId);

        Object GetFirstPermissionGroupIdForUser(Object userId);


        void EnsurePermissionExistence(String permissionCode, String categoryName, Boolean defaultGrant, Boolean isPrivileged);

        void EnsureEntityPermissionSetExistence(Type entityType, Boolean defaultGrant, Boolean isPrivileged);

        void EnsureEntityPermissionSetExistence(Type entityType);


        String GetEntityActionPermissionCode(String alias);
        String GetEntityActionPermissionCode(String alias, String action);

    }
}
