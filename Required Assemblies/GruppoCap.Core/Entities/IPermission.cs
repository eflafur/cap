using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IPermission : IEntity
    {
        String PermissionId { get; set; }
        String PermissionCode { get; set; }
        String Description { get; set; }
        String CategoryName { get; set; }
        Boolean DefaultGrant { get; set; }
        Boolean IsPrivileged { get; set; }
    }
}
