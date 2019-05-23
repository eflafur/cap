using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GruppoCap.Core
{
    public interface IRevoWebRequest
    {
        IRevoContext RevoContext { get; }

        HttpContext WebContext { get; }

        String CurrentIPAddress { get; }

        String CurrentUsername { get; }

        IUser CurrentUser { get; }
        //IUser ImpersonatedUser { get; set; }

        IDictionary<String, Object> ExtraData { get; }

        //IDictionaryBasedServicesStore ServicesStore { get; }

        void ClearCurrentUser();
    }
}
