using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public class RevoServiceBase : IRevoService
    {
        // CURRENT REVO WEB REQUEST
        public IRevoWebRequest RevoRequest
        {
            get { return RevoContextHelpers.GetCurrentRevoWebRequest(); }
        }

        // CURRENT REVO CONTEXT
        public IRevoContext RevoContext
        {
            get { return RevoContextHelpers.GetCurrentRevoContext(); }
        }
    }
}
