using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface IApplicationService : IRevoService
    {
        IApplication InstanceNew();

        IApplication GetById(String applicationId);
        
        ISubCollection<Application> GetByIds(String[] ids);
        ISubCollection<Application> Filter(String term = "", Boolean onlyActive = false);
        IList<String> EnabledApplicationIdsForUser(String userId);
        ISubCollection<Application> EnabledApplicationsForUser(String userId);
        IList<String> EnabledUserIdsForApplication(String applicationId);
        
        IInsertOperationResult Create(Application application);
        IUpdateOperationResult Update(Application application);
        IDeleteOperationResult Delete(String applicationId);

        ISubCollection<Application> GetLastCreated(Int32 howMany);
        ISubCollection<Application> GetLastUpdated(Int32 howMany);

        IUpdateOperationResult Activate(IRevoWebRequest rreq, String applicationId);
        IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String applicationId);

        // THIS IS ABOUT USERS' RELATIONS
        Boolean IsUserEnabledForApplication(String applicationId, String userId);
        IInsertOperationResult InsertUserApplication(String applicationId, String userId);
        IDeleteOperationResult RemoveUserApplication(String applicationId, String userId);
    }
}
