using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security
{
    public static class SecurityHelpers
    {
        // CURRENT USER HAS PERMISSION FOR
        [Obsolete("CurrentUserHasPermissionFor<T> is deprecated, use the extension method HasPermissionFor<T>")]
        public static Boolean CurrentUserHasPermissionFor<T>() where T : IEntity
        {
            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            IRevoWebRequest req;
            req = RevoContextHelpers.GetCurrentRevoWebRequest();

            if (req.CurrentUser == null)
                return false;

            if (req.CurrentUser.IsPrivileged)
                return true;

            String _permissionCode;

            String _alias = typeof(T).Name;
            _permissionCode = ctx.PermissionManager.GetEntityActionPermissionCode(_alias);

            return ctx.PermissionManager.GetUserGrantWithFallback(_permissionCode, req.CurrentUser.UserId);
        }


        // HAS PERMISSION FOR
        public static Boolean HasPermissionFor<T>(this IUser user) where T : IEntity
        {
            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            if (user == null)
                return false;

            if (user.IsPrivileged)
                return true;

            String _permissionCode;

            String _alias = typeof(T).Name;
            _permissionCode = ctx.PermissionManager.GetEntityActionPermissionCode(_alias);

            return ctx.PermissionManager.GetUserGrantWithFallback(_permissionCode, user.UserId);
        }

        // HAS PERMISSION
        public static Boolean HasPermission(this IUser user, String permissionCode)
        {
            if (user == null)
                return false;

            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            return ctx.PermissionManager.GetUserGrantWithFallback(permissionCode, user.UserId);
        }

        // HAS PERMISSION OR IS PRIVILEGED
        public static Boolean HasPermissionOrIsPrivileged(this IUser user, String permissionCode)
        {
            if (user == null)
                return false;

            if (user.IsPrivileged)
                return true;

            return HasPermission(user, permissionCode);
        }

        // IS ENABLED FOR APPLICATION
        public static Boolean IsEnabledForApplication(this IUser user, String applicationId)
        {
            if (user == null)
                return false;

            if (user.IsPrivileged)
                return true;

            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            return ctx.IdentityManager.IsUserEnabledForApplication(applicationId, user.UserId);
        }
    }
}
