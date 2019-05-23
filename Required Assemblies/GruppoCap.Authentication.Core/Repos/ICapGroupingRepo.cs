using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface ICapGroupingRepo : IRepository<CapGrouping>
    {
        CapGrouping GetByGroupingCode(String capGroupingCode);

        ISubCollection<CapGrouping> ListByApplication(String applicationId, Boolean onlyActive = false);
        ISubCollection<CapGrouping> ListByUser(String userId, Boolean onlyActive = false, Boolean onlyMain = false);

        IList<String> ActiveGroupingIdsForUserAndApplication(String userId, String applicationId);
        String MainGroupingIdForUserAndApplication(String userId, String applicationId);
        String MainGroupingCodeForUserAndApplication(String userId, String applicationId);

        IList<String> ActiveGroupingCodesForUserAndApplication(String userId, String applicationId);
        IList<String> ActiveGroupingIdsForApplication(String applicationId);

        Boolean IsUserMemberOfGrouping(String userId, String groupingId);
        IInsertOperationResult InsertUserGrouping(String groupingId, String userId, Boolean isMain = false);
        IDeleteOperationResult RemoveUserGrouping(String groupingId, String userId);

        IUpdateOperationResult SetUserGroupingAsMain(String groupingId, String userId, String applicationId);
    }
}
