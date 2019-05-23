using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public class CapGroupingService : ICapGroupingService
    {
        ICapGroupingRepo _groupingRepo = null;

        // CTOR
        public CapGroupingService(ICapGroupingRepo groupingRepository)
        {
            _groupingRepo = groupingRepository;
        }


        // INSTANCE NEW
        public ICapGrouping InstanceNew()
        {
            return _groupingRepo.CreateEntityInstance();
        }


        // GET BY ID
        public ICapGrouping GetById(String groupingId)
        {
            return _groupingRepo.GetById(groupingId);
        }

        // GET BY CODE
        public ICapGrouping GetByCode(String groupingCode)
        {
            return _groupingRepo.GetByGroupingCode(groupingCode);
        }


        // GET BY IDs
        public ISubCollection<CapGrouping> GetByIds(String[] ids)
        {
            return _groupingRepo.GetByIds(ids);
        }

        // LIST BY APPLICATION ID
        public ISubCollection<CapGrouping> ListByApplicationId(String applicationId, Boolean onlyActive = false)
        {
            return _groupingRepo.ListByApplication(applicationId, onlyActive);
        }


        // AVAILABLE GROUPINGs IDs FOR USER
        public IList<String> ActiveGroupingIdsForUserAndApplication(String userId, String applicationId)
        {
            return _groupingRepo.ActiveGroupingIdsForUserAndApplication(userId, applicationId);
        }

        // AVAILABLE GROUPINGs CODEs FOR USER
        public IList<String> ActiveGroupingCodesForUserAndApplication(String userId, String applicationId)
        {
            return _groupingRepo.ActiveGroupingCodesForUserAndApplication(userId, applicationId);
        }

        // ACTIVE GROUPING IDs FOR APPLICATION
        public IList<String> ActiveGroupingIdsForApplication(String applicationId)
        {
            return _groupingRepo.ActiveGroupingIdsForApplication(applicationId);
        }

        // CREATE
        public IInsertOperationResult Create(CapGrouping grouping)
        {
            return _groupingRepo.Insert(grouping);
        }

        // UPDATE
        public IUpdateOperationResult Update(CapGrouping grouping)
        {
            return _groupingRepo.Update(grouping);
        }

        // DELETE
        public IDeleteOperationResult Delete(String groupingId)
        {
            return _groupingRepo.DeleteById(groupingId);
        }


        // SET ACTIVE
        private IUpdateOperationResult SetActive(String groupingId, Boolean isActive, String author)
        {
            if (groupingId.IsNullOrWhiteSpace())
                return new UpdateOperationResult(false, "CapGroupingId cannot be null");

            ICapGrouping _g = this.GetById(groupingId);

            if (_g == null)
                return new UpdateOperationResult(false, "Cannot find the requested Cap Grouping");

            _g.IsActive = isActive;
            _g.LastUpdateMoment = DateTime.Now;
            _g.LastUpdateUserId = author;

            return this.Update((CapGrouping)_g);
        }

        // ACTIVATE
        public IUpdateOperationResult Activate(IRevoWebRequest rreq, String groupingId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUsername.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(groupingId, true, rreq.CurrentUser.UserId);
        }

        // DEACTIVATE
        public IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String groupingId)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser.UserId.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(groupingId, false, rreq.CurrentUser.UserId);
        }


        // IS USER MEMBER OF GROUPING
        public Boolean IsUserMemberOfGrouping(String userId, String groupingId)
        {
            return _groupingRepo.IsUserMemberOfGrouping(userId, groupingId);
        }

        // INSERT USER APPLICATION
        public IInsertOperationResult InsertUserGrouping(String groupingId, String userId)
        {
            return _groupingRepo.InsertUserGrouping(groupingId, userId);
        }

        // REMOVE USER APPLICATION
        public IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId)
        {
            return _groupingRepo.RemoveUserGrouping(groupingId, userId);
        }

        
        // MAIN GROUPING ID FOR USER AND APPLICATION
        public String MainGroupingIdForUserAndApplication(String userId, String applicationId)
        {
            return _groupingRepo.MainGroupingIdForUserAndApplication(userId, applicationId);
        }

        // MAIN GROUPING CODE FOR USER AND APPLICATION
        public String MainGroupingCodeForUserAndApplication(String userId, String applicationId)
        {
            return _groupingRepo.MainGroupingCodeForUserAndApplication(userId, applicationId);
        }

        // SET USER GROUPING AS MAIN
        public IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId)
        {
            return _groupingRepo.SetUserGroupingAsMain(groupingId, userId, applicationId);
        }

        // LIST BY USER ID
        public ISubCollection<CapGrouping> ListByUserId(String userId, Boolean onlyActive = false)
        {
            return _groupingRepo.ListByUser(userId, onlyActive, false);
        }

        // MAIN GROUPING FOR USER
        public ISubCollection<CapGrouping> MainGroupingForUser(String userId)
        {
            return _groupingRepo.ListByUser(userId, true, true);
        }
    }
}
