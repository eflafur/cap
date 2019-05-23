using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;
using GruppoCap.DAL;
using GruppoCap.Core.Caching;

namespace GruppoCap.Authentication.Core
{
    public class UserService : IUserService
    {
        IUserRepo _userRepo = null;
        ICredentialRepo _credentialRepo = null;
        IApplicationRepo _applicationRepo = null;
        ICapGroupingRepo _groupingRepo = null;

        // CTOR
        public UserService(IUserRepo userRepository, ICredentialRepo credentialRepo, IApplicationRepo applicationRepo, ICapGroupingRepo groupingRepo)
        {
            _userRepo = userRepository;
            _credentialRepo = credentialRepo;
            _applicationRepo = applicationRepo;
            _groupingRepo = groupingRepo;
        }


        public IUser InstanceNew()
        {
            return _userRepo.CreateEntityInstance();
        }


        // GET BY ID
        public IUser GetById(String userId)
        {
            return _userRepo.GetById(userId);
        }

        // GET BY ACCOUNT
        public IUser GetByAccount(String userId, String domain, Boolean useCredentials = false)
        {
            IUser _user;

            String _userId = userId;
            String _domain = domain;

            if (_userId.IsNullOrWhiteSpace())
                return null;
            else
                _userId = _userId.Trim();

            if (_domain.IsNullOrWhiteSpace() == false)
                _domain = _domain.Trim();

            _user = _userRepo.GetUserByAccount(_userId, _domain);

            if (_user == null)
                return null;

            _user.TrustedCompanyIds = _userRepo.GetAllCompanyIdsForUser(_userId).ToList<Int32>();

            _user.TrustedCompanies = new List<Company>() { };
            foreach(Company c in EnumUtils.GetValues<Company>())
            {
                if(_user.TrustedCompanyIds.Contains((Int32)c))
                {
                    _user.TrustedCompanies.Add(c);
                }
            }

            if (_user.TrustedCompanyIds.Contains((Int32)_user.Company) == false)
            {
                _user.TrustedCompanyIds.Add((Int32)_user.Company);
            }

            if(_user.TrustedCompanies.Contains(_user.Company) == false)
            {
                _user.TrustedCompanies.Add(_user.Company);
            }

            if(useCredentials)
            {
                ISubCollection<Credential> _credentials = _credentialRepo.GetCredentialsByAccount(_userId);
                if (_credentials != null && _credentials.Info.TotalItems > 0)
                {
                    _user.Credentials = _credentials.Items.ToList<ICredential>();
                }
            }

            return _user;
        }

        // GET USER BY ACCOUNT WITH GROUPINGS
        public IUser GetByAccountWithGroupings(String id, String domain, String applicationId, Boolean useCredentials = false)
        {
            IUser _user;
            _user = GetByAccount(id, domain, useCredentials);

            if (_user == null)
                return null;

            IList<String> _groupIds = _groupingRepo.ActiveGroupingIdsForUserAndApplication(id, applicationId);
            _user.GroupingIds = _groupIds;

            IList<String> _groupCodes = _groupingRepo.ActiveGroupingCodesForUserAndApplication(id, applicationId);
            _user.GroupingCodes = _groupCodes;

            String _mainGroupId = _groupingRepo.MainGroupingIdForUserAndApplication(id, applicationId);
            _user.MainGroupingId = _mainGroupId;

            String _mainGroupCode = _groupingRepo.MainGroupingCodeForUserAndApplication(id, applicationId);
            _user.MainGroupingCode = _mainGroupCode;
            
            return _user;
        }


        // GET BY IDs
        public ISubCollection<User> GetByIds(String[] ids, Boolean onlyActive = false, Boolean includePrivileged = false, Boolean upperizeParameters = false)
        {
            return _userRepo.GetByIds(ids, onlyActive, includePrivileged);
        }

        // FILTER 
        public ISubCollection<User> Filter(IRevoWebRequest _revoRequest, String term = "")
        {
            return _userRepo.Filter(text: term, upperizeParameters: true, includePrivileged: _revoRequest.CurrentUser.IsPrivileged);
        }

        // FILTER BY COMPANY
        public ISubCollection<User> FilterByCompany(Company company)
        {
            return _userRepo.Filter(includePrivileged: true, onlyActive: false, companyId: (Int32)company);
        }

        // FILTER BY APPLICATION ID
        public ISubCollection<User> FilterByApplicationId(IRevoWebRequest _revoRequest, String applicationId, Boolean onlyActive = false, String term = "")
        {
            IList<String> _ids = _applicationRepo.EnabledUserIdsForApplication(applicationId);

            if (_ids.HasValues() == false)
                return null;

            if (term.IsNullOrWhiteSpace())
            {
                return this.GetByIds(
                    ids: _ids.ToArray(),
                    includePrivileged: _revoRequest.CurrentUser.IsPrivileged
                );
            }

            ISubCollection<User> _filteredUsers = this.Filter(_revoRequest, term);
            IEnumerable<User> _filteredUsersWithApplication = null;

            if (_filteredUsers.Items.HasValues())
                _filteredUsersWithApplication = _filteredUsers.Items.Where(_u => _ids.Contains(_u.UserId));

            return _filteredUsersWithApplication.ToSubCollection<User>();
        }


        // GET ALL
        public ISubCollection<User> GetAll()
        {
            return _userRepo.List();
        }

        // GET PAGED
        public ISubCollection<User> GetAllPaged(Int32 pageNumber)
        {
            return _userRepo.Filter(pageNumber: pageNumber);
        }


        // CREATE
        public IInsertOperationResult Create(User user)
        {
            return _userRepo.Insert(user);
        }

        // UPDATE
        public IUpdateOperationResult Update(User user)
        {
            return _userRepo.Update(user);
        }

        // DELETE
        public IDeleteOperationResult Delete(String userId)
        {
            return _userRepo.DeleteById(userId);
        }


        // GET LAST CREATED
        public ISubCollection<User> GetLastCreated(IRevoWebRequest _revoRequest, Int32 howMany)
        {
            return _userRepo.GetLastCreated(includePrivileged: _revoRequest.CurrentUser.IsPrivileged, howMany: howMany);
        }

        // GET LAST UPDATED
        public ISubCollection<User> GetLastUpdated(IRevoWebRequest _revoRequest, Int32 howMany)
        {
            return _userRepo.GetLastUpdated(includePrivileged: _revoRequest.CurrentUser.IsPrivileged, howMany: howMany);
        }


        // SET ACTIVE
        private IUpdateOperationResult SetActive(String userId, Boolean isActive, String author)
        {
            if (userId.IsNullOrWhiteSpace())
                return new UpdateOperationResult(false, "UserId cannot be null");

            IUser _u = this.GetById(userId);

            if (_u == null)
                return new UpdateOperationResult(false, "Cannot find the requested user");

            _u.IsActive = isActive;
            _u.LastUpdateMoment = DateTime.Now;
            _u.LastUpdateUserId = author;

            return this.Update((User)_u);
        }

        // ACTIVATE
        public IUpdateOperationResult Activate(IRevoWebRequest rreq, String userId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUsername.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(userId, true, rreq.CurrentUser.UserId);
        }

        // DEACTIVATE
        public IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String userId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser.UserId.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(userId, false, rreq.CurrentUser.UserId);
        }


        // IS USER ENABLED FOR APPLICATION
        public Boolean IsUserEnabledForApplication(String applicationId, String userId)
        {
            return _applicationRepo.IsUserEnabledForApplication(applicationId, userId);
        }

        // INSERT USER APPLICATION
        public IInsertOperationResult InsertUserApplication(String applicationId, String userId)
        {
            return _applicationRepo.InsertUserApplication(applicationId, userId);
        }

        // REMOVE USER APPLICATION
        public IDeleteOperationResult RemoveUserApplication(String applicationId, String userId)
        {
            return _applicationRepo.RemoveUserApplication(applicationId, userId);
        }


        // IS USER MEMBER OF GROUPING
        public Boolean IsUserMemberOfGrouping(String userId, String groupingId)
        {
            return _groupingRepo.IsUserMemberOfGrouping(userId, groupingId);
        }

        // INSERT USER GROUPING
        public IInsertOperationResult InsertUserGrouping(String groupingId, String userId, Boolean isMain = false)
        {
            return _groupingRepo.InsertUserGrouping(groupingId, userId, isMain);
        }

        // REMOVE USER GROUPING
        public IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId)
        {
            return _groupingRepo.RemoveUserGrouping(groupingId, userId);
        }


        // SET USER GROUPING AS MAIN
        public IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId)
        {
            return _groupingRepo.SetUserGroupingAsMain(groupingId, userId, applicationId);
        }


        // GET TRUSTED COMPANY IDs
        public IList<Int32> GetTrustedCompanyIds(String userId)
        {
            return _userRepo.GetAllCompanyIdsForUser(userId).ToList<Int32>();
        }

        // IS USER ENABLED FOR COMPANY
        public Boolean IsUserEnabledForTrustedCompany(Company company, String userId)
        {
            return _userRepo.IsUserEnabledForCompany((Int32)company, userId);
        }

        // INSERT USER - TRUSTED COMPANY
        public IInsertOperationResult InsertUserTrustedCompany(Company company, String userId)
        {
            return _userRepo.InsertUserCompany((Int32)company, userId);
        }

        // REMOVE USER APPLICATION
        public IDeleteOperationResult RemoveUserTrustedCompany(Company company, String userId)
        {
            return _userRepo.RemoveUserCompany((Int32)company, userId);
        }


    }
}
