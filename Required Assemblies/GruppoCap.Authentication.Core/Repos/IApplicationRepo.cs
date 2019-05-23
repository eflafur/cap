using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface IApplicationRepo : IRepository<Application> 
    {
        IList<String> EnabledApplicationIdsForUser(String userId);

        ISubCollection<Application> EnabledApplicationsForUser(String userId);

        IList<String> EnabledUserIdsForApplication(String applicationId);

        Boolean IsUserEnabledForApplication(String applicationId, String userId);
        IInsertOperationResult InsertUserApplication(String applicationId, String userId);
        IDeleteOperationResult RemoveUserApplication(String applicationId, String userId);

        ISubCollection<Application> GetLastCreated(Int32 howMany = 5);
        ISubCollection<Application> GetLastUpdated(Int32 howMany = 5);

        ISubCollection<Application> Filter(String text = "", Boolean onlyActive = false, Int32? pageNumber = null, Int32? pageSize = null, Boolean upperizeParameters = false);
    }
}
