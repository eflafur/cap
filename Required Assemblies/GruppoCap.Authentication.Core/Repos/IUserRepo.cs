using GruppoCap.Core;
using GruppoCap.Core.Data;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface IUserRepo : IRepository<User> 
    {
        User GetUserByAccount(String userId, String domain);

        ISubCollection<User> GetByIds(Object[] ids, Boolean onlyActive = false, Boolean includePrivileged = false, Boolean upperizeParameters = false);

        ISubCollection<User> GetLastCreated(Boolean includePrivileged = false, Int32 howMany = 5);
        ISubCollection<User> GetLastUpdated(Boolean includePrivileged = false, Int32 howMany = 5);

        ISubCollection<User> Filter(String text = "", Boolean onlyActive = false, Boolean includePrivileged = false, Int32? companyId = null, Int32? pageNumber = null, Int32? pageSize = null, Boolean upperizeParameters = false);

        IEnumerable<Int32> GetAllCompanyIdsForUser(String userId);
        Boolean IsUserEnabledForCompany(Int32 companyId, String userId);
        IInsertOperationResult InsertUserCompany(Int32 companyId, String userId, Boolean isMain = false);
        IDeleteOperationResult RemoveUserCompany(Int32 companyId, String userId);

    }
}
