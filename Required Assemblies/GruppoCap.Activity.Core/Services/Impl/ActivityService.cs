using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;

namespace GruppoCap.Activity.Core
{
    public class ActivityService : IActivityService
    {
        IActivityRepo _activityRepo = null;

        // CTOR
        public ActivityService(IActivityRepo activityRepository)
        {
            _activityRepo = activityRepository;
        }

        // INSTANCE NEW
        public IActivity InstanceNew()
        {
            return _activityRepo.CreateEntityInstance();
        }


        // GET BY ID
        public IActivity GetById(String id)
        {
            return _activityRepo.GetById(id);
        }

        // GET BY IDs
        public ISubCollection<Activity> GetByIds(String[] ids)
        {
            return _activityRepo.GetByIds(ids);
        }

        // FILTER
        public ISubCollection<Activity> Filter(IRevoWebRequest rreq, Int32? companyId = null, Int32 howMany = 1000)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser == null)
                throw new NullReferenceException("Revolution Web Request - Current User cannot be empty");

            return _activityRepo.Filter(rreq.CurrentUser.IsPrivileged, companyId, howMany);
        }

        // FILTER BY ACTOR
        public ISubCollection<Activity> FilterByActorEntity(IRevoWebRequest rreq, String actorUserId, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser == null)
                throw new NullReferenceException("Revolution Web Request - Current User cannot be empty");

            return _activityRepo.FilterByActor(
                actorUserId: actorUserId, 
                includePrivileged: rreq.CurrentUser.IsPrivileged, 
                companyId: companyId,
                howMany: howMany
            );
        }

        // FILTER BY OBJECT ENTITY
        public ISubCollection<Activity> FilterByObjectEntity(IRevoWebRequest rreq, String objectEntityId, String objectEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser == null)
                throw new NullReferenceException("Revolution Web Request - Current User cannot be empty");

            return _activityRepo.FilterByObject(
                objectEntityId: objectEntityId,
                entityType: objectEntityType,
                includePrivileged: rreq.CurrentUser.IsPrivileged,
                companyId: companyId,
                howMany: howMany
            );
        }

        // FILTER BY RELATED ENTITY
        public ISubCollection<Activity> FilterByRelatedEntity(IRevoWebRequest rreq, String relatedEntityId, String relatedEntityType, Boolean includePrivileged = false, Int32? companyId = null, Boolean upperizeParameters = false, Int32 howMany = 1000)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser == null)
                throw new NullReferenceException("Revolution Web Request - Current User cannot be empty");

            return _activityRepo.FilterByObject(
                objectEntityId: relatedEntityId,
                entityType: relatedEntityType,
                includePrivileged: rreq.CurrentUser.IsPrivileged,
                companyId: companyId,
                howMany: howMany
            );
        }

        // FILTER BY COMPANY
        public ISubCollection<Activity> FilterByCompany(IRevoWebRequest rreq, Company c, Int32 howMany = 1000)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser == null)
                throw new NullReferenceException("Revolution Web Request - Current User cannot be empty");

            return _activityRepo.Filter(
                includePrivileged: rreq.CurrentUser.IsPrivileged,
                companyId: (Int32)c,
                howMany: howMany
            );
        }


        // REGISTER (GENERIC ACTIVITY)
        public IInsertOperationResult Register(IActivity activity)
        {
            return _activityRepo.Insert(activity as Activity);
        }


        // REGISTER LOGIN
        public IInsertOperationResult RegisterLogin(IRevoWebRequest rreq)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.Login;

            return Register(_a);
        }

        // REGISTER LOGIN
        public IInsertOperationResult RegisterLogin(IUser user)
        {
            if (user == null)
                throw new NullReferenceException("User cannot be null if you want to register his/her login");

            IActivity _a = InstanceNew();
            _a.SetupActor(user);
            _a.Verb = ActivityVerb.Login;

            _a.IPAddress = RevoContextHelpers.GetCurrentRevoWebRequest().CurrentIPAddress;

            return Register(_a);
        }


        // REGISTER LOGIN ATTEMPT
        public IInsertOperationResult RegisterLoginAttempt(IRevoWebRequest rreq)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.TryToLogin;

            return Register(_a);
        }

        // REGISTER LOGIN ATTEMPT
        public IInsertOperationResult RegisterLoginAttempt(IUser user)
        {
            if (user == null)
                throw new NullReferenceException("User cannot be null if you want to register his/her login");

            IActivity _a = InstanceNew();
            _a.SetupActor(user);
            _a.Verb = ActivityVerb.TryToLogin;

            _a.IPAddress = RevoContextHelpers.GetCurrentRevoWebRequest().CurrentIPAddress;

            return Register(_a);
        }

        // REGISTER LOGIN ATTEMPT
        public IInsertOperationResult RegisterLoginAttempt(IUser user, ActivityVerb verb)
        {
            if (user == null)
                throw new NullReferenceException("User cannot be null if you want to register his/her login");

            IActivity _a = InstanceNew();
            _a.SetupActor(user);
            _a.Verb = verb;

            _a.IPAddress = RevoContextHelpers.GetCurrentRevoWebRequest().CurrentIPAddress;

            return Register(_a);
        }

        // REGISTER LOGIN ATTEMPT
        public IInsertOperationResult RegisterLoginAttempt(String userId)
        {
            if (userId.IsNullOrWhiteSpace())
                throw new NullReferenceException("User Id cannot be null if you want to register his/her login");

            IActivity _a = InstanceNew();

            _a.ActorEntityId = userId;
            _a.ActorEntityDisplayText = userId;
            _a.Company = Company.CapHolding; // BY DEFAULT, I DON'T KNOW WHO THE USER IS...
            _a.IsPrivileged = false;
            _a.IPAddress = RevoContextHelpers.GetCurrentRevoWebRequest().CurrentIPAddress;

            _a.Verb = ActivityVerb.TryToLogin;

            return Register(_a);
        }


        // REGISTER LOGOUT
        public IInsertOperationResult RegisterLogout(IRevoWebRequest rreq)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.Logout;

            return Register(_a);
        }

        // REGISTER IMPERSONATE
        public IInsertOperationResult RegisterImpersonate(IRevoWebRequest rreq)
        {
            throw new NotImplementedException("La funzionalità di impersonificazione non è ad oggi disponibile");
        }


        // REGISTER VIEW
        public IInsertOperationResult RegisterView<T>(IRevoWebRequest rreq, T viewedEntity) where T : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.View;

            _a.ObjectEntityId = viewedEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(T).Name;
            _a.ObjectEntityDisplayText = viewedEntity.DisplayText;

            return Register(_a);
        }

        // REGISTER CREATED
        public IInsertOperationResult RegisterCreated<T>(IRevoWebRequest rreq, T createdEntity) where T : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.Create;

            _a.ObjectEntityId = createdEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(T).Name;
            _a.ObjectEntityDisplayText = createdEntity.DisplayText;

            return Register(_a);
        }

        // REGISTER UPDATE
        public IInsertOperationResult RegisterUpdate<T>(IRevoWebRequest rreq, T updatedEntity) where T : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.Update;

            _a.ObjectEntityId = updatedEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(T).Name;
            _a.ObjectEntityDisplayText = updatedEntity.DisplayText;

            return Register(_a);
        }

        // REGISTER DELETED
        public IInsertOperationResult RegisterDelete<T>(IRevoWebRequest rreq, T deletedEntity) where T : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = ActivityVerb.Delete;

            _a.ObjectEntityId = deletedEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(T).Name;
            _a.ObjectEntityDisplayText = deletedEntity.DisplayText;

            return Register(_a);
        }


        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction<O, R>(IRevoWebRequest rreq, O objectEntity, ActivityVerb verb, R relatedEntity)
            where O : class, IEntity
            where R : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = verb;

            _a.ObjectEntityId = objectEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(O).Name;
            _a.ObjectEntityDisplayText = objectEntity.DisplayText;

            _a.RelatedEntityId = relatedEntity.EntityId.ToString();
            _a.RelatedEntityType = typeof(R).Name;
            _a.RelatedEntityDisplayText = relatedEntity.DisplayText;

            return Register(_a);
        }

        public IInsertOperationResult RegisterRelatedAction<O>(IRevoWebRequest rreq, O objectEntity, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String relatedEntityDisplayText = "")
            where O : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = verb;

            _a.ObjectEntityId = objectEntity.EntityId.ToString();
            _a.ObjectEntityType = typeof(O).Name;
            _a.ObjectEntityDisplayText = objectEntity.DisplayText;

            _a.RelatedEntityId = relatedEntityId;
            _a.RelatedEntityType = relatedEntityType.Name;
            _a.RelatedEntityDisplayText = relatedEntityDisplayText;

            return Register(_a);
        }

        public IInsertOperationResult RegisterRelatedAction<R>(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, R relatedEntity, String objectEntityDisplayText = "")
            where R : class, IEntity
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = verb;

            _a.ObjectEntityId = objectEntityId;
            _a.ObjectEntityType = objectEntityType.Name;
            _a.ObjectEntityDisplayText = objectEntityDisplayText;

            _a.RelatedEntityId = relatedEntity.EntityId.ToString();
            _a.RelatedEntityType = typeof(R).Name;
            _a.RelatedEntityDisplayText = relatedEntity.DisplayText;

            return Register(_a);
        }

        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String objectEntityDisplayText = "", String relatedEntityDisplayText = "")
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = verb;

            _a.ObjectEntityId = objectEntityId;
            _a.ObjectEntityType = objectEntityType.Name;
            _a.ObjectEntityDisplayText = objectEntityDisplayText;

            _a.RelatedEntityId = relatedEntityId;
            _a.RelatedEntityType = relatedEntityType.Name;
            _a.RelatedEntityDisplayText = relatedEntityDisplayText;

            return Register(_a);
        }

        // REGISTER CUSTOM ACTION
        public IInsertOperationResult RegisterCustomActivity(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String verbMessage, String objectEntityDisplayText = "")
        {
            IActivity _a = InstanceNew();
            _a.SetupActor(rreq);
            _a.Verb = verb;

            _a.ObjectEntityId = objectEntityId;
            _a.ObjectEntityType = objectEntityType.Name;
            _a.ObjectEntityDisplayText = objectEntityDisplayText;

            _a.RelatedEntityDisplayText = verbMessage;

            return Register(_a);
        }
    }
}
