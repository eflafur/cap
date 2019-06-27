using GruppoCap.Core.Mvc;
using GruppoCap.Security.PEM;

namespace GestioneRimborsi.Web.Controllers
{
    public class SecurityController : BaseSecurityController
    {
        // CTOR
        public SecurityController(IPEMService pemService)
            : base(pemService)
        {
            // EMPTY BY DEFAULT
        }
    }
}