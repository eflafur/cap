using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;

namespace GruppoCap.Core
{
    public interface IUser : ITrackedEntity
    {
        String UserId { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String Domain { get; set; }
        Company Company { get; set; }
        Boolean IsActive { get; set; }
        Boolean IsPrivileged { get; set; }

        IList<String> GroupingIds { get; set; }
        IList<String> GroupingCodes { get; set; }

        IList<Company> TrustedCompanies { get; set; }
        IList<Int32> TrustedCompanyIds { get; set; }
        
        IList<ICredential> Credentials { get; set; }

        String MainGroupingCode { get; set; }
        String MainGroupingId { get; set; }
    }
}
