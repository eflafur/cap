using GruppoCap.Authentication.Core;
using GruppoCap.Core.Mvc;

namespace GestioneRimborsi.Web.Controllers
{
    public class CapGroupingController : BaseCapGroupingController
    {
        // CTOR
        public CapGroupingController(ICapGroupingService capGroupingService)
            : base(capGroupingService)
        {
            // EMPTY BY DEFAULT
        }
    }
}