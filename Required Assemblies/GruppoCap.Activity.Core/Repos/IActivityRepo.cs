using GruppoCap.Core;
using GruppoCap.Core.Data;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Activity.Core
{
    public interface IActivityRepo : IRepository<Activity> 
    {
        ISubCollection<Activity> Filter(Boolean includePrivileged = false, Int32? companyId = null, Int32 howMany = 100);

        ISubCollection<Activity> FilterByInvolvedEntity(String entityId, String entityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 100);

        ISubCollection<Activity> FilterByActor(String actorUserId, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 100);
        ISubCollection<Activity> FilterByObject(String objectEntityId, String entityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 100);
        ISubCollection<Activity> FilterByRelated(String relatedEntityId, String entityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 100);
    }
}
