using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public class ApplicationService : IApplicationService
    {
        IApplicationRepo _applicationRepo = null;

        // CTOR
        public ApplicationService(IApplicationRepo applicationRepository)
        {
            _applicationRepo = applicationRepository;
        }


        // INSTANCE NEW
        public IApplication InstanceNew()
        {
            return _applicationRepo.CreateEntityInstance();
        }


        // GET BY ID
        public IApplication GetById(String applicationId)
        {
            return _applicationRepo.GetById(applicationId);
        }


        // GET BY IDs
        public ISubCollection<Application> GetByIds(String[] ids)
        {
            return _applicationRepo.GetByIds(ids);
        }

        // FILTER 
        public ISubCollection<Application> Filter(String term = "", Boolean onlyActive = false)
        {
            return _applicationRepo.Filter(text: term, onlyActive: onlyActive, upperizeParameters: true);
        }

        // AVAILABLE APPLICATION IDs FOR USER
        public IList<String> EnabledApplicationIdsForUser(String userId)
        {
            return _applicationRepo.EnabledApplicationIdsForUser(userId);
        }

        public IList<String> EnabledUserIdsForApplication(String applicationId)
        {
            return _applicationRepo.EnabledUserIdsForApplication(applicationId);
        }

        // CREATE
        public IInsertOperationResult Create(Application application)
        {
            return _applicationRepo.Insert(application);
        }

        // UPDATE
        public IUpdateOperationResult Update(Application application)
        {
            return _applicationRepo.Update(application);
        }

        // DELETE
        public IDeleteOperationResult Delete(String applicationId)
        {
            return _applicationRepo.DeleteById(applicationId);
        }


        // GET LAST CREATED
        public ISubCollection<Application> GetLastCreated(Int32 howMany)
        {
            return _applicationRepo.GetLastCreated(howMany: howMany);
        }

        // GET LAST UPDATED
        public ISubCollection<Application> GetLastUpdated(Int32 howMany)
        {
            return _applicationRepo.GetLastUpdated(howMany: howMany);
        }


        // SET ACTIVE
        private IUpdateOperationResult SetActive(String applicationId, Boolean isActive, String author)
        {
            if (applicationId.IsNullOrWhiteSpace())
                return new UpdateOperationResult(false, "ApplicationId cannot be null");

            IApplication _a = this.GetById(applicationId);

            if (_a == null)
                return new UpdateOperationResult(false, "Cannot find the requested application");

            _a.IsActive = isActive;
            _a.LastUpdateMoment = DateTime.Now;
            _a.LastUpdateUserId = author;

            return this.Update((Application)_a);
        }

        // ACTIVATE
        public IUpdateOperationResult Activate(IRevoWebRequest rreq, String applicationId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUsername.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(applicationId, true, rreq.CurrentUser.UserId);
        }

        // DEACTIVATE
        public IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String applicationId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser.UserId.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(applicationId, false, rreq.CurrentUser.UserId);
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

        // ENABLED APPLICATIONs FOR USER
        public ISubCollection<Application> EnabledApplicationsForUser(String userId)
        {
            return _applicationRepo.EnabledApplicationsForUser(userId);
        }
    }
}
