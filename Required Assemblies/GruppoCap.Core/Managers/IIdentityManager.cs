using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Identity
{
    public interface IIdentityManager
    {
        IUser CurrentUser {get; }
        String CurrentUsername { get; }
        String CurrentIPAddress { get;  }

        Boolean IsUserEnabled(String userId);
        Boolean IsUserEnabledForApplication(String applicationId, String userId);

        IApplication CurrentApplication { get; }
    }
}
