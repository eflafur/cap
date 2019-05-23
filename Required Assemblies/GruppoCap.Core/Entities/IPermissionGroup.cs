using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IPermissionGroup : IEntity
    {
        String PermissionGroupId { get; set; }
        String Title { get; set; }
        String Description { get; set; }
        Boolean IsPrivileged { get; set; }
    }
}
