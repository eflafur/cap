using System.Web.Http;
using System.Web.Http.Controllers;

namespace GruppoCap.Core.Api
{
    public abstract class RevoApiController : ApiController
    {
        protected IRevoContext RevoContext { get; private set; }
        protected IRevoWebRequest RevoRequest { get; private set; }

        protected RevoApiController()
        {
            RevoContext = RevoContextHelpers.GetCurrentRevoContext();
            RevoRequest = RevoContextHelpers.GetCurrentRevoWebRequest();
        }

        // INITIALIZE
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }
    }
}
