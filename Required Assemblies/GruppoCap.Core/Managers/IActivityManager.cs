using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Activity
{
    public interface IActivityManager
    {
        IInsertOperationResult RegisterLogin();
        IInsertOperationResult RegisterLoginAttempt();

        IInsertOperationResult RegisterLogin(IUser user);
        IInsertOperationResult RegisterLoginAttempt(IUser user, ActivityVerb verb = ActivityVerb.TryToLogin);
        IInsertOperationResult RegisterLoginAttempt(String userId);

        IInsertOperationResult RegisterLogout();
        IInsertOperationResult RegisterImpersonate();

        IInsertOperationResult RegisterView<T>(T viewedEntity) where T : class, IEntity;
        IInsertOperationResult RegisterCreated<T>(T createdEntity) where T : class, IEntity;
        IInsertOperationResult RegisterUpdate<T>(T updatedEntity) where T : class, IEntity;
        IInsertOperationResult RegisterDelete<T>(T deletedEntity) where T : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<O, R>(O objectEntity, ActivityVerb verb, R relatedEntity)
            where O : class, IEntity
            where R : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<O>(O objectEntity, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String relatedEntityDisplayText = "")
            where O : class, IEntity;

        IInsertOperationResult RegisterRelatedAction<R>(String objectEntityId, Type objectEntityType, ActivityVerb verb, R relatedEntity, String objectEntityDisplayText = "")
            where R : class, IEntity;

        IInsertOperationResult RegisterRelatedAction(String objectEntityId, Type objectEntityType, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String objectEntityDisplayText = "", String relatedEntityDisplayText = "");

        IInsertOperationResult RegisterCustomActivity(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String verbMessage, String objectEntityDisplayText = "");

        IList<IActivity> FilterByObjectEntity(String objectEntityId, Type objectEntityType);
            
    }
}
