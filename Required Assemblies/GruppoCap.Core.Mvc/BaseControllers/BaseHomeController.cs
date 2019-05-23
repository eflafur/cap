using System.Web.Mvc;

namespace GruppoCap.Core.Mvc
{
    public abstract class BaseHomeController : RevoController
    {
        // INDEX
        public ActionResult Index()
        {
            return View();
        }

        // USER LOGIN
        public ActionResult UserLogin()
        {
            return View("LoginPage");
        }

        // USER LOGOUT
        public ActionResult UserLogout()
        {
            return View("LogoutPage");
        }

        // USER NOT AUTHENTICATED
        public ActionResult UserNotAuthenticated()
        {
            return View();
        }

        // USER NOT ENABLED
        public ActionResult UserNotEnabled()
        {
            return View();
        }

        // USER NOT PRIVILEGED
        public ActionResult UserNotPrivileged()
        {
            return View();
        }

        // ENTITY NOT FOUND
        public ActionResult EntityNotFound()
        {
            return View();
        }

        // APP IN MAINTENANCE
        public ActionResult ApplicationInMaintenance()
        {
            return View();
        }

        // APP NOT ENABLED
        public ActionResult ApplicationNotEnabled()
        {
            return View();
        }

        // APP REFERENCE
        public ActionResult ApplicationReference()
        {
            return View();
        }

        // ADMINISTRATION PANEL
        public ActionResult AdministrationPanel()
        {
            return View("Administration");
        }

        // DASHBOARD PANEL
        public ActionResult Dashboard()
        {
            return View();
        }

        // CRITICAL DASHBOARD PANEL
        public ActionResult CriticalDashboard()
        {
            return View();
        }

        // ERROR
        public ActionResult ErrorPage()
        {
            return View();
        }

    }
}
