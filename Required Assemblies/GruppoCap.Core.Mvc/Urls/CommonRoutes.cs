using System.Web.Mvc;
using System.Web.Routing;

namespace GruppoCap.Core.Mvc
{
    public static class CommonRoutes
    {
        // REGISTER REVO ROUTES
        public static void RegisterRevoRoutes(RouteCollection routes)
        {
            #region BASE ROUTEs

            routes.MapRoute(
                name: "root",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            // USER LOGIN
            routes.MapRoute(
                name: "user-login",
                url: "login",
                defaults: new { controller = "Auth", action = "UserLogin" }
            );

            // USER LOGOUT
            routes.MapRoute(
                name: "user-logout",
                url: "logout",
                defaults: new { controller = "Auth", action = "UserLogout" }
            );

            // USER LOGIN
            routes.MapRoute(
                name: "authenticate-user",
                url: "authenticate",
                defaults: new { controller = "Auth", action = "Authenticate" }
            );

            // USER LOGIN
            routes.MapRoute(
                name: "turn-off-user",
                url: "do-user-logout",
                defaults: new { controller = "Auth", action = "DoUserLogout" }
            );

            // USER NOT PRESENT
            routes.MapRoute(
                name: "error-user-not-authenticated",
                url: "user-not-authenticated",
                defaults: new { controller = "Home", action = "UserNotAuthenticated" }
            );

            // USER DISABLED
            routes.MapRoute(
                name: "error-user-not-enabled",
                url: "user-not-enabled",
                defaults: new { controller = "Home", action = "UserNotEnabled" }
            );

            // NOT PRIVILEGED FOR APPLICATION
            routes.MapRoute(
                name: "error-user-without-privilege",
                url: "user-not-privileged",
                defaults: new { controller = "Home", action = "UserNotPrivileged" }
            );

            routes.MapRoute(
                name: "error-entity-not-found",
                url: "entity-not-found",
                defaults: new { controller = "Home", action = "EntityNotFound" }
            );

            routes.MapRoute(
                name: "error-ebs-data-not-found",
                url: "ebs-data-not-found",
                defaults: new { controller = "Home", action = "EBSDataNotFound" }
            );

            routes.MapRoute(
                name: "application-not-enabled",
                url: "application-not-enabled",
                defaults: new { controller = "Home", action = "ApplicationNotEnabled" }
            );

            routes.MapRoute(
                name: "application-in-maintenance",
                url: "application-in-maintenance",
                defaults: new { controller = "Home", action = "ApplicationInMaintenance" }
            );

            routes.MapRoute(
                name: "application-reference",
                url: "references",
                defaults: new { controller = "Home", action = "ApplicationReference" }
            );

            // ERROR PAGE
            routes.MapRoute(
                name: "error-page",
                url: "error",
                defaults: new { controller = "Home", action = "ErrorPage" }
            );

            #endregion

            #region DASHBOARDs

            routes.MapRoute(
                name: "dashboard",
                url: "dashboard",
                defaults: new { controller = "Home", action = "Dashboard" }
            );

            routes.MapRoute(
                name: "critical-dashboard",
                url: "critical-dashboard",
                defaults: new { controller = "Home", action = "CriticalDashboard" }
            );

            #endregion

            #region USERS ROUTEs
            routes.MapRoute(
                name: "users",
                url: "users",
                defaults: new { controller = "User", action = "List" }
            );

            routes.MapRoute(
                name: "users-last-updated",
                url: "users/last-updated",
                defaults: new { controller = "User", action = "LastUpdatedList" }
            );

            routes.MapRoute(
                name: "users-last-created",
                url: "users/last-created",
                defaults: new { controller = "User", action = "LastCreatedList" }
            );

            routes.MapRoute(
                name: "users-search",
                url: "users/search/{term}",
                defaults: new { controller = "User", action = "Search" }
            );
            
            routes.MapRoute(
                name: "user-update",
                url: "user/{userId}/update",
                defaults: new { controller = "User", action = "Update" }
            );

            routes.MapRoute(
                name: "user",
                url: "user/{userId}",
                defaults: new { controller = "User", action = "Detail" }
            );

            routes.MapRoute(
                name: "user-permission",
                url: "user/{userId}/permissions",
                defaults: new { controller = "User", action = "Permissions" }
            );

            #endregion

            #region CAP GROUPINGs

            routes.MapRoute(
                name: "capgroupings",
                url: "groupings",
                defaults: new { controller = "CapGrouping", action = "List" }
            );

            routes.MapRoute(
                name: "capgrouping-create",
                url: "grouping/create",
                defaults: new { controller = "CapGrouping", action = "Create" }
            );

            routes.MapRoute(
                name: "capgrouping-update",
                url: "grouping/{capGroupingId}/update",
                defaults: new { controller = "CapGrouping", action = "Update" }
            );

            routes.MapRoute(
                name: "capgrouping-ready-to-delete",
                url: "grouping/{capGroupingId}/delete-confirm",
                defaults: new { controller = "CapGrouping", action = "ReadyToDelete" }
            );

            routes.MapRoute(
                name: "capgrouping-delete",
                url: "grouping/{capGroupingId}/delete",
                defaults: new { controller = "CapGrouping", action = "Delete" }
            );

            routes.MapRoute(
                name: "capgrouping",
                url: "grouping/{capGroupingId}",
                defaults: new { controller = "CapGrouping", action = "Detail" }
            );

            #endregion

            #region ACTIVITIES ROUTEs
            routes.MapRoute(
                name: "activities",
                url: "activities",
                defaults: new { controller = "Activity", action = "List" }
            );
            #endregion

            #region PERMISSIONs ROUTEs
            routes.MapRoute(
                name: "permissions",
                url: "security/permissions",
                defaults: new { controller = "Security", action = "PermissionList" }
            );

            routes.MapRoute(
                name: "permission-set-default-grant",
                url: "security/permissions/{permissionCode}/setdefaultgrant",
                defaults: new { controller = "Security", action = "SetDefaultGrantOnPermission" }
            );

            routes.MapRoute(
                name: "permission-set-user-grant",
                url: "security/permissions/{permissionCode}/setusergrant/{userId}",
                defaults: new { controller = "Security", action = "SetUserGrantOnPermission" }
            );

            routes.MapRoute(
                name: "permission-remove-user-grant",
                url: "security/permissions/{permissionCode}/removeusergrant/{userId}",
                defaults: new { controller = "Security", action = "RemoveUserGrantOnPermission" }
            );

            // USED ONLY BY AJAX LOAD OF security/_userPermissions PARTIAL VIEW
            // THERE ISN'T ANY MATCHING METHOD IN UrlFor
            routes.MapRoute(
                name: "user-permission-list",
                url: "security/{userId}/permissions",
                defaults: new { controller = "Security", action = "GetUserPermissionList" }
            );
            #endregion

            #region PERMISSION GROUPS ROUTEs
            routes.MapRoute(
                name: "permissiongroups",
                url: "security/permissiongroups",
                defaults: new { controller = "Security", action = "PermissionGroupList" }
            );

            routes.MapRoute(
                name: "permissiongroup-create",
                url: "security/permissiongroups/create",
                defaults: new { controller = "Security", action = "CreatePermissionGroup" }
            );

            routes.MapRoute(
                name: "permissiongroup-update",
                url: "security/permissiongroups/{permissionGroupId}/update",
                defaults: new { controller = "Security", action = "UpdatePermissionGroup" }
            );

            routes.MapRoute(
                name: "permissiongroup",
                url: "security/permissiongroups/{permissionGroupId}",
                defaults: new { controller = "Security", action = "PermissionGroupDetail" }
            );

            routes.MapRoute(
                name: "permission-set-group-grant",
                url: "security/permissions/{permissionCode}/setgroupgrant/{permissionGroupId}",
                defaults: new { controller = "Security", action = "SetGroupGrantOnPermission" }
            );

            routes.MapRoute(
                name: "permission-remove-group-grant",
                url: "security/permissions/{permissionCode}/removegroupgrant/{permissionGroupId}",
                defaults: new { controller = "Security", action = "RemoveGroupGrantOnPermission" }
            );

            routes.MapRoute(
                name: "permission-set-permissiongroup-for-user",
                url: "security/permissions/{userId}/setpermissiongroup/{permissionGroupId}",
                defaults: new { controller = "Security", action = "SetPermissionGroupForUser" }
            );
            #endregion

            #region ADMINISTRATION ROUTEs

            routes.MapRoute(
                name: "administration-panel",
                url: "administration/",
                defaults: new { controller = "Home", action = "AdministrationPanel" }
            );

            #endregion
        }
    }
}
