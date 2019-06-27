using GruppoCap.Authentication.Core;
using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GruppoCap;
using Newtonsoft.Json;
using GruppoCap.Activity.Core;
using GruppoCap.Core.Mvc;

namespace GestioneRimborsi.Web.Controllers
{
    public class UserController : RevoController
    {
        // PRIVATE MEMBERs
        private IUserService _userService = null;

        // CTOR
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //USER LIST
        public ActionResult List()
        {
            ISubCollection<User> _users;
            _users = _userService.FilterByApplicationId(
                rreq: RevoRequest,
                applicationId: Ambient.CurrentApplicationId
            );

            return View("List", _users);
        }



        // USER DETAIL
        public ActionResult Detail(String userId)
        {
            IUser _user;
            _user = _userService.GetById(userId);

            //RevoContext.ActivityManager.RegisterView<User>((User)_user);

            return View("Detail", _user);
        }

        // USER PERMISSIONS
        public ActionResult Permissions(String userId)
        {
            IUser _user;
            _user = _userService.GetById(userId);

            if (_user == null)
                return Redirect(CommonUrls.EntityNotFound);
            
            return View("Permission", _user);
        }

        // SEARCH
        public ActionResult Search(String term)
        {
            try
            {
                ISubCollection<User> _users;
                _users = _userService.FilterByApplicationId(
                    rreq: RevoRequest, 
                    term: term, 
                    applicationId: Ambient.CurrentApplicationId, 
                    onlyActive: true
                );

                if (_users.Items.HasValues() == false)
                {
                    return PartialMessage(
                        HtmlSnippets.Alert.Info("La tua ricerca non ha prodotto alcun risultato...")
                    );
                }

                return PartialView("~/Views/User/_list.cshtml", _users);
            }
            catch (Exception ex)
            {
                // LOG ERROR HERE

                return PartialMessage(
                    HtmlSnippets.Alert.Error(
                        exception: ex,
                        title: "Si è verificato un errore!",
                        recoveryUrl: CommonUrls.Users
                    )
                );
            }
        }


        // UPDATE
        [HttpPost]
        public JsonResult Update(String userId, String groupingPermissions, String mainGroupingPermission)
        {
            try
            {
                IUser _u = _userService.GetById(userId);

                ICapGrouping _currentGroup = null;
                String _relatedObjectText = String.Empty;

                ISubCollection<CapGrouping> _allGroups = ((ICapGroupingService)RevoContext.ServiceProvider.GetServiceFor<CapGrouping>()).ListByApplicationId(Ambient.CurrentApplicationId, true);

                if (_u == null)
                    return JsonError("L'utente non è stato trovato", "Attenzione!");

                // TRY TO DESERIALIZE IT FIRST, 'CAUSE IF I HAVE AN ERROR HERE, I DON'T NEED TO WORRY ABOUT "TRANSACTION"
                IDictionary<String, Boolean> _groupingPermissions = JsonConvert.DeserializeObject<IDictionary<String, Boolean>>(groupingPermissions);

                IDeleteOperationResult groupRemoveResult = new DeleteOperationResult(true);
                IInsertOperationResult groupPermissionResult = new InsertOperationResult(0, true);
                IUpdateOperationResult setMainGroupResult = new UpdateOperationResult(true);

                Boolean _everythingIsOk = true, _alreadyEnabled = false;

                foreach (KeyValuePair<String, Boolean> _p in _groupingPermissions)
                {
                    _alreadyEnabled = _userService.IsUserMemberOfGrouping(_u.UserId, _p.Key);

                    if (_alreadyEnabled && _p.Value == false) // ERA ASSOCIATO A QUESTO GRUPPO, ORA NON PIU'
                    {
                        groupRemoveResult = _userService.RemoveUserGrouping(_p.Key, _u.UserId);
                        if (groupRemoveResult.GenericMeaning)
                        {
                            _currentGroup = _allGroups.Items.Where(_i => _i.CapGroupingId == _p.Key).FirstOrDefault();
                            _relatedObjectText = _currentGroup == null ? _p.Key : _currentGroup.CapGroupingCode;

                            RevoContext.ActivityManager.RegisterRelatedAction<User>(
                                (User)_u,
                                ActivityVerb.Unlink,
                                _relatedObjectText,
                                typeof(CapGrouping)
                            );

                            _currentGroup = null;
                        }
                    }

                    if (_alreadyEnabled == false && _p.Value == true) // NON ERA ASSOCIATO A QUESTO GRUPPO, ORA SI
                    {
                        groupPermissionResult = _userService.InsertUserGrouping(_p.Key, _u.UserId);
                        if (groupPermissionResult.GenericMeaning)
                        {
                            _currentGroup = _allGroups.Items.Where(_i => _i.CapGroupingId == _p.Key).FirstOrDefault();
                            _relatedObjectText = _currentGroup == null ? _p.Key : _currentGroup.CapGroupingCode;

                            RevoContext.ActivityManager.RegisterRelatedAction<User>(
                                (User)_u,
                                ActivityVerb.Link,
                                _relatedObjectText,
                                typeof(CapGrouping)
                            );

                            _currentGroup = null;
                        }
                    }
                    
                    if (groupPermissionResult.GenericMeaning == false)
                        _everythingIsOk = false;

                    
                }

                setMainGroupResult = _userService.SetUserGroupingAsMain(mainGroupingPermission, _u.UserId, Ambient.CurrentApplicationId);
                if (setMainGroupResult.GenericMeaning == false)
                    _everythingIsOk = false;

                if (_everythingIsOk)
                {
                    return Json(new { status = "success", data = new { message = "L'utente è stato modificato ed abilitato con successo ai raggruppamenti" } });
                }
                else
                    return Json(new { status = "warning", data = new { message = "L'utente è stato modificato con successo, ma qualcosa è andato storto nell'abilitazione ai raggruppamenti" } });

            }
            catch (Exception ex)
            {
                return JsonError(ex);
            }
        }

    }
}