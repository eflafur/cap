using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface IUserService : IRevoService
    {
        IUser InstanceNew();

        IUser GetById(String id);
        IUser GetByAccount(String id, String domain, Boolean useCredentials = false);
        IUser GetByAccountWithGroupings(String id, String domain, String applicationId, Boolean useCredentials = false);

        ISubCollection<User> GetByIds(String[] ids, Boolean onlyActive = false, Boolean includePrivileged = false, Boolean upperizeParameters = false);

        ISubCollection<User> Filter(IRevoWebRequest rreq, String term = "");
        ISubCollection<User> FilterByApplicationId(IRevoWebRequest rreq, String applicationId, Boolean onlyActive = false, String term = "");
        ISubCollection<User> FilterByCompany(Company c);

        //ISubCollection<User> GetAllPaged(Int32 pageNumber);
        //ISubCollection<User> GetAll();

        IInsertOperationResult Create(User user);
        IUpdateOperationResult Update(User user);
        IDeleteOperationResult Delete(String userId);

        ISubCollection<User> GetLastCreated(IRevoWebRequest rreq, Int32 howMany);
        ISubCollection<User> GetLastUpdated(IRevoWebRequest rreq, Int32 howMany);

        IUpdateOperationResult Activate(IRevoWebRequest rreq, String userId);
        IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String userId);

        // THIS IS ABOUT APPLICATIONS' RELATIONS
        Boolean IsUserEnabledForApplication(String applicationId, String userId);
        IInsertOperationResult InsertUserApplication(String applicationId, String userId);
        IDeleteOperationResult RemoveUserApplication(String applicationId, String userId);

        // THIS IS ABOUT GROUPING' RELATIONS
        Boolean IsUserMemberOfGrouping(String userId, String groupingId);
        IInsertOperationResult InsertUserGrouping(String groupingId, String userId, Boolean isMain = false);
        IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId);
        IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId);

        // THIS IS ABOUT COMPANIES' RELATIONS
        IList<Int32> GetTrustedCompanyIds(String userId);
        Boolean IsUserEnabledForTrustedCompany(Company company, String userId);
        IInsertOperationResult InsertUserTrustedCompany(Company company, String userId);
        IDeleteOperationResult RemoveUserTrustedCompany(Company company, String userId);
    }
}
