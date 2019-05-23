using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GruppoCap.Core.Mvc
{
    public static class CommonUrls
    {
        #region BASE URLs

        // BASE URL
        public static String BaseUrl
        {
            get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
        }

        // ADMINISTRATION BASE URL
        public static String AdministrationBaseUrl
        {
            get { return BaseUrl.AppendUrlTokens("administration").ToAbsoluteUrl(); }
        }

        // SECuRiTY BASE URL
        public static String SecurityBaseUrl
        {
            get { return BaseUrl.AppendUrlTokens("security").ToAbsoluteUrl(); }
        }

        #endregion


        // USER LOGIN
        public static String UserLogin
        {
            get { return BaseUrl.AppendUrlTokens("login").ToAbsoluteUrl(); }
        }

        // USER LOGOUT
        public static String UserLogout
        {
            get { return BaseUrl.AppendUrlTokens("do-user-logout").ToAbsoluteUrl(); }
        }

        // DO USER LOGIN
        public static String AuthenticateUser
        {
            get { return BaseUrl.AppendUrlTokens("authenticate").ToAbsoluteUrl(); }
        }



        // USER NOT AUTHENTICATED
        public static String UserNotAuthenticated
        {
            get { return BaseUrl.AppendUrlTokens("user-not-authenticated").ToAbsoluteUrl(); }
        }

        // USER NOT ENABLED
        public static String UserNotEnabled
        {
            get { return BaseUrl.AppendUrlTokens("user-not-enabled").ToAbsoluteUrl(); }
        }

        // USER NOT PRIVILEGED
        public static String UserNotPrivileged
        {
            get { return BaseUrl.AppendUrlTokens("user-not-privileged").ToAbsoluteUrl(); }
        }

        // ENTITY NOT FOUND
        public static String EntityNotFound
        {
            get { return BaseUrl.AppendUrlTokens("entity-not-found").ToAbsoluteUrl(); }
        }

        // EBS DATA NOT FOUND
        public static String EBSDataNotFound
        {
            get { return BaseUrl.AppendUrlTokens("ebs-data-not-found").ToAbsoluteUrl(); }
        }

        // APPLICATION NOT ENABLED
        public static String ApplicationNotEnabled
        {
            get { return BaseUrl.AppendUrlTokens("application-not-enabled").ToAbsoluteUrl(); }
        }

        // APPLICATION IN MAINTENANCE
        public static String ApplicationInMaintenance
        {
            get { return BaseUrl.AppendUrlTokens("application-in-maintenance").ToAbsoluteUrl(); }
        }

        // APPLICATION REFERENCES
        public static String ApplicationReferences
        {
            get { return BaseUrl.AppendUrlTokens("references").ToAbsoluteUrl(); }
        }

        // DASHBOARD
        public static String Dashboard
        {
            get { return BaseUrl.AppendUrlTokens("dashboard").ToAbsoluteUrl(); }
        }

        // CRITICAL DASHBOARD
        public static String CriticalDashboard
        {
            get { return BaseUrl.AppendUrlTokens("critical-dashboard").ToAbsoluteUrl(); }
        }

        // ERROR PAGE
        public static String ErrorPage
        {
            get { return BaseUrl.AppendUrlTokens("error").ToAbsoluteUrl(); }
        }


        #region USER URLs

        // USERS
        public static String Users
        {
            get { return BaseUrl.AppendUrlTokens("users").ToAbsoluteUrl(); }
        }

        // USERS SEARCH
        public static String UserSearch(String term)
        {
            return Users.AppendUrlTokens("search", term).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // USER DETAIL
        public static String UserDetail(String userId)
        {
            return BaseUrl.AppendUrlTokens("user", userId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // USER PERMISSIONS
        public static String UserPermissions(String userId)
        {
            return BaseUrl.AppendUrlTokens("user", userId, "permissions").ToAbsoluteUrl().EnsureEndsWith("/");
        }

        #endregion



        #region CAPGROUPING URLs

        // CAP GROUPING
        public static String Groupings
        {
            get { return BaseUrl.AppendUrlTokens("groupings").ToAbsoluteUrl(); }
        }

        // CAP GROUPING DELETE
        public static String CapGroupingDelete(String capGroupingId)
        {
            return BaseUrl.AppendUrlTokens("grouping", capGroupingId, "delete-confirm").ToAbsoluteUrl();
        }

        // CAP GROUPING DETAIL
        public static String CapGroupingDetail(String capGroupingId)
        {
            return BaseUrl.AppendUrlTokens("grouping", capGroupingId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        #endregion

        #region ACTIVITIES URLs

        // ACTIVITIES
        public static String Activities
        {
            get { return BaseUrl.AppendUrlTokens("activities").ToAbsoluteUrl(); }
        }

        // USERS SEARCH
        public static String ActivitySearch(String term)
        {
            return Activities.AppendUrlTokens("search", term).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        #endregion

        #region PERMISSION URLs

        // PERMISSIONs
        public static String Permissions
        {
            get { return SecurityBaseUrl.AppendUrlTokens("permissions").ToAbsoluteUrl(); }
        }

        // PERMISSION SET DEFAULT GRANT
        public static String PermissionSetDefaultGrant(String permissionCode)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", permissionCode, "setdefaultgrant").ToAbsoluteUrl();
        }

        // PERMISSION SET USER GRANT
        public static String PermissionSetUserGrant(String permissionCode, String userId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", permissionCode, "setusergrant", userId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // PERMISSION REMOVE USER GRANT
        public static String PermissionRemoveUserGrant(String permissionCode, String userId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", permissionCode, "removeusergrant", userId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // PERMISSION GROUPs
        public static String PermissionGroups
        {
            get { return SecurityBaseUrl.AppendUrlTokens("permissiongroups").ToAbsoluteUrl(); }
        }

        // PERMISSION GROUP DETAIL
        public static String PermissionGroupDetail(String permissionGroupId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissiongroups", permissionGroupId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // PERMISSION SET GROUP GRANT
        public static String PermissionSetGroupGrant(String permissionCode, String permissionGroupId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", permissionCode, "setgroupgrant", permissionGroupId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // PERMISSION REMOVE GROUP GRANT
        public static String PermissionRemoveGroupGrant(String permissionCode, String permissionGroupId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", permissionCode, "removegroupgrant", permissionGroupId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        // PERMISSION SET GROUP FOR USER
        public static String PermissionSetGroupForUser(String userId, String permissionGroupId)
        {
            return SecurityBaseUrl.AppendUrlTokens("permissions", userId, "setpermissiongroup", permissionGroupId).ToAbsoluteUrl().EnsureEndsWith("/");
        }

        #endregion

        #region ADMINISTRATION URLs

        // ADMINISTRATION PANEL
        public static String AdministrationPanel
        {
            get { return AdministrationBaseUrl; }
        }

        #endregion
    }
}
