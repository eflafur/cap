using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface ICapGroupingService : IRevoService
    {
        ICapGrouping InstanceNew();

        ICapGrouping GetById(String groupingId);
        ICapGrouping GetByCode(String groupingCode);

        ISubCollection<CapGrouping> GetByIds(String[] ids);

        ISubCollection<CapGrouping> ListByApplicationId(String applicationId, Boolean onlyActive = false);

        ISubCollection<CapGrouping> ListByUserId(String userId, Boolean onlyActive = false);

        IList<String> ActiveGroupingIdsForUserAndApplication(String userId, String applicationId);
        String MainGroupingIdForUserAndApplication(String userId, String applicationId);
        String MainGroupingCodeForUserAndApplication(String userId, String applicationId);

        IList<String> ActiveGroupingCodesForUserAndApplication(String userId, String applicationId);
        IList<String> ActiveGroupingIdsForApplication(String applicationId);


        ISubCollection<CapGrouping> MainGroupingForUser(String userId);


        IInsertOperationResult Create(CapGrouping grouping);
        IUpdateOperationResult Update(CapGrouping grouping);
        IDeleteOperationResult Delete(String groupingId);

        IUpdateOperationResult Activate(IRevoWebRequest rreq, String groupingId);
        IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String groupingId);

        // THIS IS ABOUT USERS' RELATIONS
        Boolean IsUserMemberOfGrouping(String userId, String groupingId);
        IInsertOperationResult InsertUserGrouping(String groupingId, String userId);
        IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId);

        IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId);
    }
}
