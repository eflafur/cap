using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.Security.PEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GruppoCap;
using GruppoCap.Authentication.Core;

namespace GruppoCap.Core.Mvc
{
    public class BaseSecurityController : RevoController
    {
        // PRIVATE MEMBERs
        private IPEMService _pemService = null;

        // CTOR
        public BaseSecurityController(IPEMService pemService)
        {
            _pemService = pemService;
        }

        // PERMISSION LIST
        public ActionResult PermissionList()
        {
            IList<IPermission> _permissions;
            _permissions = _pemService.BrowsePermissions(RevoRequest.CurrentUser.IsPrivileged);

            return View("PermissionList", _permissions);
        }

        // USER PERMISSION LIST
        [HttpGet]
        public ActionResult GetUserPermissionList(String userId)
        {
            IUserService _userService = RevoContext.ServiceProvider.GetServiceFor<User>() as IUserService;
            if (_userService == null)
                return null;

            IUser _user = _userService.GetById(userId);

            if (_user == null)
                return null;

            return PartialView("~/Views/Security/_userPermissions.cshtml", (User)_user);
        }

        // SET DIRECT GRANT ON PERMISSION
        [HttpPost]
        public JsonResult SetDefaultGrantOnPermission(String permissionCode)
        {
            IPermission _p;
            _p = _pemService.GetPermission(permissionCode);

            if (_p == null)
                return JsonError("Permission not found", "Warning!");

            _p.DefaultGrant = !_p.DefaultGrant;

            IUpdateOperationResult _opRes;
            _opRes = _pemService.UpdatePermission(_p);

            if(_opRes.GenericMeaning == false)
                return JsonError("Something gone wrong updating the default grant", "Oh no!");

            String _htmlGrant = _p.DefaultGrant ? CommonSnippets.AllowedGrant : CommonSnippets.DeniedGrant;

            return Json(new { status = "success", data = new { permissionId = _p.PermissionId, htmlGrant = _htmlGrant, message = "Permission default grant was correctly updated" } });
        }

        // SET DIRECT GRANT ON PERMISSION
        [HttpPost]
        public JsonResult SetUserGrantOnPermission(String permissionCode, String userId)
        {
            IOperationResult _res;
            IPermission _p;
            String _htmlGrant, _resultMessage;

            Boolean? _grant;
            _grant = _pemService.GetUserGrantDirect(permissionCode, userId);

            if (_grant.HasValue == false)
                _grant = true;
            else
                _grant = !_grant;

            _htmlGrant = _grant.Value ? CommonSnippets.AllowedGrant : CommonSnippets.DeniedGrant;
            _resultMessage = _grant.Value ? "granted" : "denied";

            _res = _pemService.SetDirectGrantForUser(permissionCode, userId, _grant.Value);
            if(_res.GenericMeaning)
            {
                _p = _pemService.GetPermission(permissionCode);
                return Json(new { status = "success", data = new { permissionId = _p.PermissionId, grant = _grant.Value, htmlGrant = _htmlGrant, message = "Permission was correctly {0} to {1}".FormatWith(_resultMessage, userId) } });
            }

            return JsonError("Something gone wrong trying to set the grant", "Oh no!");
        }

        // REMOVE DIRECT GRANT ON PERMISSION
        [HttpPost]
        public JsonResult RemoveUserGrantOnPermission(String permissionCode, String userId)
        {
            IOperationResult _res;
            IPermission _p;
            String _htmlGrant;

            Boolean _grant;

            _res = _pemService.DeleteDirectGrantForUser(permissionCode, userId);
            if(_res.GenericMeaning == false)
            {
                return JsonError("Something gone wrong trying to remove the grant", "Oh no!");
            }

            _grant = _pemService.GetUserGrantWithFallback(permissionCode, userId);
            _htmlGrant = _grant ? CommonSnippets.InheritedAllowedGrant : CommonSnippets.InheritedDeniedGrant;
            
            if (_res.GenericMeaning)
            {
                _p = _pemService.GetPermission(permissionCode);
                return Json(new { status = "success", data = new { permissionId = _p.PermissionId, grant = _grant, htmlGrant = _htmlGrant, message = "Permission grant was correctly removed from {0}".FormatWith(userId) } });
            }

            return JsonError("Something gone wrong trying to set the grant", "Oh no!");
        }


        // PERMISSION GROUP LIST
        public ActionResult PermissionGroupList()
        {
            IList<IPermissionGroup> _permissionGroups;
            _permissionGroups = _pemService.BrowsePermissionGroups(RevoRequest.CurrentUser.IsPrivileged);

            return View("PermissionGroupList", _permissionGroups);
        }

        // PERMISSION GROUP DETAIL
        public ActionResult PermissionGroupDetail(String permissionGroupId)
        {
            IPermissionGroup _permissionGroup;
            _permissionGroup = _pemService.GetPermissionGroup(permissionGroupId);

            
            return View("PermissionGroupDetail", _permissionGroup);
        }


        // SET DIRECT GRANT ON PERMISSION FOR GROUP
        [HttpPost]
        public JsonResult SetGroupGrantOnPermission(String permissionCode, String permissionGroupId)
        {
            IOperationResult _res;
            IPermission _p;
            String _htmlGrant, _resultMessage;

            Boolean? _grant;
            _grant = _pemService.GetGroupGrantDirect(permissionCode, permissionGroupId);

            if (_grant.HasValue == false)
                _grant = true;
            else
                _grant = !_grant;

            _htmlGrant = _grant.Value ? CommonSnippets.AllowedGrant : CommonSnippets.DeniedGrant;
            _resultMessage = _grant.Value ? "granted" : "denied";

            _res = _pemService.SetDirectGrantForPermissionGroup(permissionCode, permissionGroupId, _grant.Value);
            if (_res.GenericMeaning)
            {
                _p = _pemService.GetPermission(permissionCode);
                return Json(new { status = "success", data = new { permissionId = _p.PermissionId, grant = _grant.Value, htmlGrant = _htmlGrant, message = "Permission was correctly {0} to the group".FormatWith(_resultMessage) } });
            }

            return JsonError("Something gone wrong trying to set the grant", "Oh no!");
        }

        // REMOVE DIRECT GRANT ON PERMISSION FOR GROUP
        [HttpPost]
        public JsonResult RemoveGroupGrantOnPermission(String permissionCode, String permissionGroupId)
        {
            IOperationResult _res;
            IPermission _p;
            String _htmlGrant;

            Boolean _grant;

            _res = _pemService.DeleteDirectGrantForPermissionGroup(permissionCode, permissionGroupId);
            if (_res.GenericMeaning == false)
            {
                return JsonError("Something gone wrong trying to remove the grant", "Oh no!");
            }

            _grant = _pemService.GetGroupGrantWithFallback(permissionCode, permissionGroupId);
            _htmlGrant = _grant ? CommonSnippets.InheritedAllowedGrant : CommonSnippets.InheritedDeniedGrant;

            if (_res.GenericMeaning)
            {
                _p = _pemService.GetPermission(permissionCode);
                return Json(new { status = "success", data = new { permissionId = _p.PermissionId, grant = _grant, htmlGrant = _htmlGrant, message = "Permission grant was correctly removed from the group" } });
            }

            return JsonError("Something gone wrong trying to set the grant", "Oh no!");
        }


        // CREATE
        [HttpPost]
        public JsonResult CreatePermissionGroup(String title, String description)
        {
            try
            {
                IPermissionGroup _pg;
                //_pg = _pemService.GetPermissionGroupByTitle(title);
                //if (_pg != null)
                //    return JsonError("Permission Group already exists", "Warning!");

                _pg = _pemService.CreatePermissionGroupInstance();

                _pg.Title = title;
                _pg.Description = description;
                _pg.IsPrivileged = false;

                IInsertOperationResult opRes = _pemService.InsertPermissionGroup(_pg);
                if (opRes.GenericMeaning)
                    return Json(new { status = "success", data = new { message = "Permission Group created successfully. Reload the current page to see the updated data" } });
                else
                    return JsonError("Something gone wrong creating the permission group", "Warning!");
            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }

        // CREATE
        [HttpPost]
        public JsonResult UpdatePermissionGroup(String permissionGroupId, String title, String description, Boolean isPrivileged)
        {
            try
            {
                IPermissionGroup _pg;
                _pg = _pemService.GetPermissionGroup(permissionGroupId);
                if (_pg == null)
                    return JsonError("Permission Group doesn't exists anymore", "Warning!");

                _pg.Title = title;
                _pg.Description = description;
                _pg.IsPrivileged = isPrivileged;

                IUpdateOperationResult opRes = _pemService.UpdatePermissionGroup(_pg);
                if (opRes.GenericMeaning)
                    return Json(new { status = "success", data = new { message = "Permission Group updated successfully." } });
                else
                    return JsonError("Something gone wrong updating the permission group", "Warning!");
            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }


        // SET PERMISSION GROUP FOR USER
        [HttpPost]
        public JsonResult SetPermissionGroupForUser(String userId, String permissionGroupId)
        {
            IOperationResult _res;

            // FIRST OF ALL, USER WILL BE REMOVED FOR ANY GROUP. 
            // THIS IS BECAUSE WE ASSUME THAT ANY USER CANNOT JOIN MORE THAN ONE GROUP.
            _res = _pemService.RemoveUserFromAllPermissionGroups(userId);

            if(permissionGroupId == Guid.Empty.ToString())
            {
                // I WAS TRYING TO REMOVE THE USER FROM GROUP
                if (_res.GenericMeaning)
                    return Json(new { status = "success", data = new { message = "User was correctly removed from the group" } });
                else
                    return JsonError("Something gone wrong trying to remove the user from the group!", "Oh no!");
            }

            if (_res.GenericMeaning == false)
                return JsonError("Something gone wrong trying to change the user's permission group!", "Oh no!");

            _res = _pemService.PutUserInPermissionGroup(userId, permissionGroupId);
            if (_res.GenericMeaning)
                return Json(new { status = "success", data = new { message = "User correctly joins the selected group" } });

            return JsonError("Something gone wrong trying to change the user's permission group!", "Oh no!");
        }

    }
}