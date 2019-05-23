using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Activity.Core
{
    public interface IActivityService : IRevoService
    {
        IActivity InstanceNew();

        IActivity GetById(String id);

        ISubCollection<Activity> GetByIds(String[] ids);
        ISubCollection<Activity> Filter(IRevoWebRequest rreq, Int32? companyId = null, Int32 howMany = 1000);
        ISubCollection<Activity> FilterByActorEntity(IRevoWebRequest rreq, String actorUserId, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000);
        ISubCollection<Activity> FilterByObjectEntity(IRevoWebRequest rreq, String objectEntityId, String objectEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000);
        ISubCollection<Activity> FilterByRelatedEntity(IRevoWebRequest rreq, String relatedEntityId, String relatedEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000);
        ISubCollection<Activity> FilterByCompany(IRevoWebRequest rreq, Company c, Int32 howMany = 1000);

        IInsertOperationResult Register(IActivity activity);
        
        IInsertOperationResult RegisterLogin(IRevoWebRequest rreq);
        IInsertOperationResult RegisterLoginAttempt(IRevoWebRequest rreq);

        IInsertOperationResult RegisterLogin(IUser user);
        IInsertOperationResult RegisterLoginAttempt(IUser user);
        IInsertOperationResult RegisterLoginAttempt(IUser user, ActivityVerb verb);
        IInsertOperationResult RegisterLoginAttempt(String userId);

        IInsertOperationResult RegisterLogout(IRevoWebRequest rreq);
        IInsertOperationResult RegisterImpersonate(IRevoWebRequest rreq);

        IInsertOperationResult RegisterView<T>(IRevoWebRequest rreq, T viewedEntity) where T : class, IEntity;
        IInsertOperationResult RegisterCreated<T>(IRevoWebRequest rreq, T createdEntity) where T : class, IEntity;
        IInsertOperationResult RegisterUpdate<T>(IRevoWebRequest rreq, T updatedEntity) where T : class, IEntity;
        IInsertOperationResult RegisterDelete<T>(IRevoWebRequest rreq, T deletedEntity) where T : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<O, R>(IRevoWebRequest rreq, O objectEntity, ActivityVerb verb, R relatedEntity)
            where O : class, IEntity
            where R : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<O>(IRevoWebRequest rreq, O objectEntity, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String relatedEntityDisplayText = "")
            where O : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<R>(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, R relatedEntity, String objectEntityDisplayText = "")
            where R : class, IEntity;

        IInsertOperationResult RegisterRelatedAction(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String objectEntityDisplayText ="", String relatedEntityDisplayText = "");

        IInsertOperationResult RegisterCustomActivity(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String verbMessage, String objectEntityDisplayText = "");
    }
}
