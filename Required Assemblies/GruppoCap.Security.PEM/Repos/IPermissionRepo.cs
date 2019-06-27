using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security.PEM
{
    public interface IPermissionRepo :  IRepository<Permission> 
    {
        IPermission GetByCode(String permissionCode);

        List<IPermission> BrowsePermissions(Boolean includePrivileged);

        // GRANT RELATED

        Boolean? GetUserGrantDirect(String permissionId, Object userId);

        IDeleteOperationResult DeleteAllGrantsByUserId(Object userId);

        IDeleteOperationResult DeleteAllGrantsByPermission(String permissionId);

        IOperationResult SetDirectGrantForUser(String permissionId, Object userId, Boolean granted);

        IDeleteOperationResult DeleteDirectGrantForUser(String permissionId, Object userId);
        
    }
}
