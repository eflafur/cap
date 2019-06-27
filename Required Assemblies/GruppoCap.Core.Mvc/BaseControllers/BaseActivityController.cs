using GruppoCap.Activity.Core;
using GruppoCap.Core.Data;
using System;
using System.Web.Mvc;

namespace GruppoCap.Core.Mvc
{
    public abstract class BaseActivityController : RevoController
    {
        // PRIVATE MEMBERs
        private IActivityService _activityService = null;

        // CTOR
        public BaseActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: Activity
        public ActionResult List()
        {
            ISubCollection<GruppoCap.Activity.Core.Activity> _activities;

            if (RevoRequest.CurrentUser.IsPrivileged)
            {
                _activities = _activityService.Filter(RevoRequest);
            }
            else
            {
                _activities = _activityService.FilterByActorEntity(
                    rreq: RevoRequest,
                    actorUserId: RevoRequest.CurrentUsername,
                    companyId: (Int32)RevoRequest.CurrentUser.Company,
                    howMany: 1000
               );
            }

            return View("List", _activities);
        }


    }
}