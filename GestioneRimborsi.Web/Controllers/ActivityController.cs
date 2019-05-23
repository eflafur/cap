using GruppoCap.Activity.Core;
using GruppoCap.Core.Mvc;

namespace GestioneRimborsi.Web.Controllers
{
    public class ActivityController :  BaseActivityController
    {
        // CTOR
        public ActivityController(IActivityService activityService)
            : base(activityService)
        {
            // EMPTY BY DEFAULT
        }
    }
}