using GruppoCap.Core;
using GruppoCap.Security.PEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Security.PEM
{
    public class PEMService : IPEMService
    {
        IPermissionRepo _permissionRepo = null;
        IPermissionGroupRepo _permissionGroupRepo = null;

        // CTOR
        public PEMService(IPermissionRepo permissionRepository, IPermissionGroupRepo permissionGroupRepo)
        {
            _permissionRepo = permissionRepository;
            _permissionGroupRepo = permissionGroupRepo;
        }

        #region PERMISSIONs

        // CREATE PERMISSION INSTANCE
        public IPermission CreatePermissionInstance()
        {
            return _permissionRepo.CreateEntityInstance();
        }

        // CREATE PERMISSION INSTANCE
        public IPermission GetPermission(String permissionCode)
        {
            return _permissionRepo.GetByCode(permissionCode);
        }

        // INSERT PERMISSION
        public IInsertOperationResult InsertPermission(IPermission permission)
        {
            return _permissionRepo.Insert((Permission)permission);
        }

        // UPDATE PERMISSION
        public IUpdateOperationResult UpdatePermission(IPermission permission)
        {
            return _permissionRepo.Update((Permission)permission);
        }

        // DELETE PERMISSION
        public IDeleteOperationResult DeletePermission(String permissionCode)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new DeleteOperationResult(false, "Permission not found");

            return _permissionRepo.DeleteById(_p.PermissionId);
        }

        // BROWSE PERMISSION
        public List<IPermission> BrowsePermissions(Boolean includePrivileged)
        {
            return _permissionRepo.BrowsePermissions(includePrivileged);
        }

        #endregion

        #region PERMISSION GROUPs

        // CREATE PERMISSION GROUP INSTANCE
        public IPermissionGroup CreatePermissionGroupInstance()
        {
            return _permissionGroupRepo.CreateEntityInstance();
        }

        // GET PERMISSION GROUP
        public IPermissionGroup GetPermissionGroup(Object permissionGroupId)
        {
            return _permissionGroupRepo.GetById(permissionGroupId);
        }

        // INSERT PERMISSION GROUP
        public IInsertOperationResult InsertPermissionGroup(IPermissionGroup group)
        {
            return _permissionGroupRepo.Insert((PermissionGroup)group);  
        }

        // UPDATE PERMISSION GROUP
        public IUpdateOperationResult UpdatePermissionGroup(IPermissionGroup group)
        {
            return _permissionGroupRepo.Update((PermissionGroup)group);
        }

        // DELETE PERMISSION GROUP
        public IDeleteOperationResult DeletePermissionGroup(Object groupId)
        {
            return _permissionGroupRepo.DeleteById(groupId);
        }

        // BROWSE PERMISSION GROUPS
        public List<IPermissionGroup> BrowsePermissionGroups(Boolean includePrivileged)
        {
            return _permissionGroupRepo.BrowsePermissionGroups(includePrivileged);
        }

        #endregion

        #region GRANTs

        // GET USER GRANT DIRECT
        public Boolean? GetUserGrantDirect(String permissionCode, Object userId)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return null;

            return _permissionRepo.GetUserGrantDirect(_p.PermissionId, userId);
        }

        // GET USER GRANT WITH FALLBACK
        public Boolean GetUserGrantWithFallback(String permissionCode, Object userId)
        {
            Boolean? _res;
            _res = GetUserGrantDirect(permissionCode, userId);

            if (_res.HasValue)
            {
                // DIRECT GRANT
                return _res.Value;
            }
            else
            {
                // FALLBACK CHAIN: THAT IS --> GROUP's GRANT (EVENTUALLY) --> PERMISSION's DEFAULT

                Object groupId;
                groupId = GetFirstPermissionGroupIdForUser(userId);

                if (groupId != null)
                {
                    // THE USER IS IN A GROUP, TRY TO GO THAT WAY...
                    return GetGroupGrantWithFallback(permissionCode, groupId);
                }

                // PERMISSION's DEFAULT GRANT
                IPermission p1;
                p1 = GetPermission(permissionCode);

                if (p1 == null)
                    return false;
                else
                    return p1.DefaultGrant;
            }
        }

        // GET GROUP GRANT DIRECT
        public Boolean? GetGroupGrantDirect(String permissionCode, Object groupId)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return null;

            return _permissionGroupRepo.GetGroupGrantDirect(_p.PermissionId, groupId);
        }

        // GET GROUP GRANT WITH FALLBACK
        public Boolean GetGroupGrantWithFallback(String permissionCode, Object groupId)
        {
            Boolean? _res;
            _res = GetGroupGrantDirect(permissionCode, groupId);

            if (_res.HasValue)
            {
                // DIRECT GRANT
                return _res.Value;
            }
            else
            {
                // PERMISSION's DEFAULT GRANT
                IPermission p1;
                p1 = GetPermission(permissionCode);

                if (p1 == null)
                    return false;
                else
                    return p1.DefaultGrant;
            }
        }

        // DELETE ALL GRANTs BY USER ID
        public IDeleteOperationResult DeleteAllGrantsByUserId(Object userId)
        {
            return _permissionRepo.DeleteAllGrantsByUserId(userId);
        }

        // DELETE ALL GRANTs BY PERMISSION GROUP ID
        public IDeleteOperationResult DeleteAllGrantsByPermissionGroupId(Object groupId)
        {
            return _permissionGroupRepo.DeleteAllGrantsByPermissionGroupId(groupId);
        }

        // DELETE ALL GRANTs BY PERMISSION
        public IDeleteOperationResult DeleteAllGrantsByPermission(String permissionCode)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new DeleteOperationResult(false, "Permission not found");

            IDeleteOperationResult opRes;
            opRes = _permissionRepo.DeleteAllGrantsByPermission(_p.PermissionId);

            if (opRes.GenericMeaning == false)
                return opRes;

            opRes = _permissionGroupRepo.DeleteAllGrantsByPermission(_p.PermissionId);

            return opRes;
        }

        // SET DIRECT GRANT FOR USER
        public IOperationResult SetDirectGrantForUser(String permissionCode, Object userId, Boolean granted)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new OperationResult(false, "Permission not found");

            return _permissionRepo.SetDirectGrantForUser(_p.PermissionId, userId, granted);
        }

        // SET DIRECT GRANT FOR PERMISSION GROUP
        public IOperationResult SetDirectGrantForPermissionGroup(String permissionCode, Object groupId, Boolean granted)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new OperationResult(false, "Permission not found");

            return _permissionGroupRepo.SetDirectGrantForPermissionGroup(_p.PermissionId, groupId, granted);
        }

        // DELETE DIRECT GRANT FOR USER
        public IDeleteOperationResult DeleteDirectGrantForUser(String permissionCode, Object userId)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new DeleteOperationResult(false, "Permission not found");

            return _permissionRepo.DeleteDirectGrantForUser(_p.PermissionId, userId);
        }

        // DELETE DIRECT GRANT FOR PERMISSION GROUP
        public IDeleteOperationResult DeleteDirectGrantForPermissionGroup(String permissionCode, Object groupId)
        {
            IPermission _p;
            _p = _permissionRepo.GetByCode(permissionCode);

            if (_p == null)
                return new DeleteOperationResult(false, "Permission not found");

            return _permissionGroupRepo.DeleteDirectGrantForPermissionGroup(_p.PermissionId, groupId);
        }

        #endregion

        #region MANAGE USERs IN GROUPs

        // PUT USER IN PERMISSION GROUP
        public IOperationResult PutUserInPermissionGroup(Object userId, Object groupId)
        {
            return _permissionGroupRepo.PutUserInPermissionGroup(userId, groupId);
        }

        // REMOVE USER FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveUserFromPermissionGroup(Object userId, Object groupId)
        {
            return _permissionGroupRepo.RemoveUserFromPermissionGroup(userId, groupId);
        }

        // REMOVE USER FROM ALL PERMISSION GROUPs
        public IDeleteOperationResult RemoveUserFromAllPermissionGroups(Object userId)
        {
            return _permissionGroupRepo.RemoveUserFromAllPermissionGroups(userId);
        }

        // REMOVE ALL USERs FROM PERMISSION GROUP
        public IDeleteOperationResult RemoveAllUsersFromPermissionGroup(Object groupId)
        {
            return _permissionGroupRepo.RemoveAllUsersFromPermissionGroup(groupId);
        }

        // GET FIRST PERMISSION GROUP ID FOR USER
        public Object GetFirstPermissionGroupIdForUser(Object userId)
        {
            return _permissionGroupRepo.GetFirstPermissionGroupIdForUser(userId);
        }

        #endregion

    }
}
