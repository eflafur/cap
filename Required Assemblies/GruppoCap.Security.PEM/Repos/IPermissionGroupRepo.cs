using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security.PEM
{
    public interface IPermissionGroupRepo :  IRepository<PermissionGroup> 
    {
        List<IPermissionGroup> BrowsePermissionGroups(Boolean includePrivileged);

        // GRANT RELATED

        Boolean? GetGroupGrantDirect(String permissionId, Object groupId);

        IDeleteOperationResult DeleteAllGrantsByPermissionGroupId(Object groupId);

        IDeleteOperationResult DeleteAllGrantsByPermission(String permissionId);

        IOperationResult SetDirectGrantForPermissionGroup(String permissionId, Object groupId, Boolean granted);

        IDeleteOperationResult DeleteDirectGrantForPermissionGroup(String permissionId, Object groupId);

        IOperationResult PutUserInPermissionGroup(Object userId, Object groupId);

        IDeleteOperationResult RemoveUserFromPermissionGroup(Object userId, Object groupId);

        IDeleteOperationResult RemoveUserFromAllPermissionGroups(Object userId);

        IDeleteOperationResult RemoveAllUsersFromPermissionGroup(Object groupId);

        Object GetFirstPermissionGroupIdForUser(Object userId);
    }
}
