using GruppoCap.Core;
using GruppoCap.Core.Activity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GruppoCap.Activity.Core
{
    public class ActivityManager : IActivityManager
    {
        private IActivityService _activityService = null;

        #region CTOR

        public ActivityManager(IActivityService activityService) // ADD CACHE SERVICE AND DURATION
        {
            _activityService = activityService;
            //_cacheService = cacheService;
            //_cacheDuration = cacheDuration;
        }

        #endregion


        // REGISTER LOGIN
        public IInsertOperationResult RegisterLogin()
        {
            try { return _activityService.RegisterLogin(RevoContextHelpers.GetCurrentRevoWebRequest()); }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }

        }

        // REGISTER LOGIN (WITH DIRECT USER)
        public IInsertOperationResult RegisterLogin(IUser user)
        {
            try { return _activityService.RegisterLogin(user); }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }


        // REGISTER LOGIN ATTEMPT
        public IInsertOperationResult RegisterLoginAttempt()
        {
            try { return _activityService.RegisterLoginAttempt(RevoContextHelpers.GetCurrentRevoWebRequest()); }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER LOGIN ATTEMPT (WITH DIRECT USER)
        public IInsertOperationResult RegisterLoginAttempt(IUser user, ActivityVerb verb = ActivityVerb.TryToLogin)
        {
            try
            {
                return _activityService.RegisterLoginAttempt(user, verb);
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER LOGIN ATTEMPT (WITH ONLY THE USER ID)
        public IInsertOperationResult RegisterLoginAttempt(String userId)
        {
            try { return _activityService.RegisterLoginAttempt(userId); }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }


        // REGISTER LOGOUT
        public IInsertOperationResult RegisterLogout()
        {
            try { return _activityService.RegisterLogout(RevoContextHelpers.GetCurrentRevoWebRequest()); }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER IMPERSONATE
        public IInsertOperationResult RegisterImpersonate()
        {
            throw new NotImplementedException();
        }


        // REGISTER VIEW
        public IInsertOperationResult RegisterView<T>(T viewedEntity)
            where T : class, IEntity
        {
            try
            {
                return _activityService.RegisterView<T>(RevoContextHelpers.GetCurrentRevoWebRequest(), viewedEntity);
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER CREATED
        public IInsertOperationResult RegisterCreated<T>(T createdEntity)
            where T : class, IEntity
        {
            try
            {
                return _activityService.RegisterCreated<T>(RevoContextHelpers.GetCurrentRevoWebRequest(), createdEntity);
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER UPDATE
        public IInsertOperationResult RegisterUpdate<T>(T updatedEntity)
            where T : class, IEntity
        {
            try
            {
                return _activityService.RegisterUpdate<T>(RevoContextHelpers.GetCurrentRevoWebRequest(), updatedEntity);
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER DELETE
        public IInsertOperationResult RegisterDelete<T>(T deletedEntity)
            where T : class, IEntity
        {
            try
            {
                return _activityService.RegisterDelete<T>(RevoContextHelpers.GetCurrentRevoWebRequest(), deletedEntity);
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }


        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction<O, R>(O objectEntity, ActivityVerb verb, R relatedEntity)
            where O : class, IEntity
            where R : class, IEntity
        {
            try
            {
                return _activityService.RegisterRelatedAction<O, R>(
                    rreq: RevoContextHelpers.GetCurrentRevoWebRequest(),
                    objectEntity: objectEntity,
                    verb: verb,
                    relatedEntity: relatedEntity
                );
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction<O>(O objectEntity, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String relatedEntityDisplayText = "")
            where O : class, IEntity
        {
            try
            {
                return _activityService.RegisterRelatedAction<O>(
                    rreq: RevoContextHelpers.GetCurrentRevoWebRequest(),
                    objectEntity: objectEntity,
                    verb: verb,
                    relatedEntityId: relatedEntityId,
                    relatedEntityType: relatedEntityType,
                    relatedEntityDisplayText: relatedEntityDisplayText
                );
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction<R>(String objectEntityId, Type objectEntityType, ActivityVerb verb, R relatedEntity, String objectEntityDisplayText = "")
            where R : class, IEntity
        {
            try
            {
                return _activityService.RegisterRelatedAction<R>(
                    rreq: RevoContextHelpers.GetCurrentRevoWebRequest(),
                    objectEntityId: objectEntityId,
                    objectEntityType: objectEntityType,
                    objectEntityDisplayText: objectEntityDisplayText,
                    verb: verb,
                    relatedEntity: relatedEntity
                );
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER RELATED ACTION
        public IInsertOperationResult RegisterRelatedAction(String objectEntityId, Type objectEntityType, ActivityVerb verb, String relatedEntityId, Type relatedEntityType, String objectEntityDisplayText = "", String relatedEntityDisplayText = "")
        {
            try
            {
                return _activityService.RegisterRelatedAction(
                    rreq: RevoContextHelpers.GetCurrentRevoWebRequest(),
                    objectEntityId: objectEntityId,
                    objectEntityType: objectEntityType,
                    objectEntityDisplayText: objectEntityDisplayText,
                    verb: verb,
                    relatedEntityId: relatedEntityId,
                    relatedEntityType: relatedEntityType,
                    relatedEntityDisplayText: relatedEntityDisplayText
                );
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }

        // REGISTER CUSTOM ACTION
        public IInsertOperationResult RegisterCustomActivity(IRevoWebRequest rreq, String objectEntityId, Type objectEntityType, ActivityVerb verb, String verbMessage, String objectEntityDisplayText = "")
        {
            try
            {
                return _activityService.RegisterCustomActivity(
                    rreq: RevoContextHelpers.GetCurrentRevoWebRequest(),
                    objectEntityId: objectEntityId,
                    objectEntityType: objectEntityType,
                    objectEntityDisplayText: objectEntityDisplayText,
                    verb: verb,
                    verbMessage: verbMessage
                );
            }
            catch (Exception ex)
            {
                // LOG ERROR
                return null;
            }
        }



        // FILTER BY OBJECT ENTITY
        public IList<IActivity> FilterByObjectEntity(String objectEntityId, Type objectEntityType)
        {
            try
            {
                return _activityService.FilterByObjectEntity(RevoContextHelpers.GetCurrentRevoWebRequest(), objectEntityId, objectEntityType.Name).Items.ToList<IActivity>();
            }
            catch
            {
                return new List<IActivity>();
            }
        }

    }
}
