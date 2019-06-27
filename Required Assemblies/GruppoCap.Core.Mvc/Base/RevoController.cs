using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace GruppoCap.Core
{
    public abstract class RevoController : Controller
    {
        protected IRevoContext RevoContext { get; private set; }
        protected IRevoWebRequest RevoRequest { get; private set; }

        // INITIALIZE
        protected override void Initialize(RequestContext requestContext)
        {
            RevoContext = RevoContextHelpers.GetCurrentRevoContext();
            RevoRequest = RevoContextHelpers.GetCurrentRevoWebRequest();

            base.Initialize(requestContext);
        }

        // ON EXCEPTION (OVERRIDE)
        protected override void OnException(ExceptionContext filterContext)
        {
            //RevoContext.LoggingService.Error(RevoRequest.WebContext.Request, filterContext.Exception);
            base.OnException(filterContext);
        }
       

        // GET ERROR AS JSON (WITH EXCEPTION)
        public JsonResult JsonError(Exception ex)
        {
            return GetResultAsJson("failure", "Attenzione", ex.Message);
        }

        // GET ERROR AS JSON (CUSTOM MESSAGE)
        public JsonResult JsonError(String message, String title = null)
        {
            String _title;
            _title = title.IsNullOrWhiteSpace() ? "Attenzione" : title;

            return GetResultAsJson("failure", _title, message);
        }

        // GET INFO AS JSON
        public JsonResult JsonInfo(String message, String title = null)
        {
            String _title;
            _title = title.IsNullOrWhiteSpace() ? "Attenzione" : title;

            return GetResultAsJson("info", _title, message);
        }

        // GET WARN AS JSON
        public JsonResult JsonWarning(String message, String title = null)
        {
            String _title;
            _title = title.IsNullOrWhiteSpace() ? "Attenzione" : title;

            return GetResultAsJson("warn", _title, message);
        }

        // GET RESULT AS JSON
        private JsonResult GetResultAsJson(String status, String title, String message)
        {
            return Json(new { status = status, data = new { title = title, message = message } });
        }


        // PARTIAL MESSAGE
        public ContentResult PartialMessage(String message)
        {
            ContentResult c = new ContentResult();
            c.Content = message;

            return c;
        }

        // PARTIAL MESSAGE
        public ContentResult PartialMessage(MvcHtmlString message)
        {
            ContentResult c = new ContentResult();
            c.Content = message.ToHtmlString();

            return c;
        }
    }
}
