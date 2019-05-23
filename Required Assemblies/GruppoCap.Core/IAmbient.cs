using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace GruppoCap.Core
{
    public interface IAmbient
    {
        String Current { get; }
        String CurrentApplicationId { get; }
        String CurrentApplicationName { get; }
        String CurrentApplicationVersion { get; }
        String CurrentApplicationConnectionStringName { get; }
        String CurrentApplicationConnectionString { get; }

        String CurrentApplicationBootstrapMainVersion { get; }

        AuthenticationMode CurrentAuthenticationMode { get; }
    }
}
