using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;

namespace GruppoCap.Core
{
    public interface ICredential : ITrackedEntity
    {
        String UserId { get; set; }
        String EMail { get; set; }
        String PasswordHash { get; set; }
        Boolean IsActive { get; set; }
    }
}
