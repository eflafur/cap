using GruppoCap.Authentication.Core;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GruppoCap.Core.Mvc
{
    public abstract class BaseAuthController : RevoController
    {
        // USER LOGIN
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserLogin()
        {
            return View("LoginPage");
        }

        // USER LOGOUT
        [HttpGet]
        [AllowAnonymous]
        public ActionResult UserLogout()
        {
            return View("LogoutPage");
        }

        // REMOVE LOGIN COOKIEs
        private void RemoveLoginCookies()
        {
            HttpCookie c;

            c = Response.Cookies.Get("r3v0-4uthc00k13");

            if (c != null)
            {
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(c);
            }
        }

        // DO USER LOGIN
        private void DoUserLogin(IUser u)
        {
            Ensure.Arg(() => u).IsNotNull();

            // SET AUTH COOKIE
            FormsAuthentication.SetAuthCookie(u.UserId, false);

            // ADD THE COOKIE
            Response.Cookies.Add(new HttpCookie("r3v0-4uthc00k13", u.DisplayText));
        }

        // DO USER LOGOUT
        public ActionResult DoUserLogout()
        {
            // AUTH SIGNOUT
            FormsAuthentication.SignOut();

            if (Session != null) { Session.Abandon(); }

            RemoveLoginCookies();

            return RedirectToAction("UserLogout");
        }

        // LOGIN
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Authenticate(String email, String password)
        {
            // NO USERNAME/PASSWORD
            if (email.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
            {
                return JsonError(@"Email e/o password invalidi");
            }

            ICredentialService _credentialService = RevoContext.ServiceProvider.GetServiceFor<Credential>() as ICredentialService;
            if(_credentialService == null)
            {
                RevoContext.ContextLogger.Error("Error trying to get the Credential Service. Configuration could be wrong...");
                return JsonError(@"Errore in fase di recupero delle credenziali. Per favore, contatta gli amministratori del servizio");
            }

            ICredential _c = _credentialService.GetCredentialByEmailAndPassword(email, password);
            if(_c == null)
            {
                RevoContext.ActivityManager.RegisterLoginAttempt(email);
                return JsonWarning(@"Credenziali non trovate. Sei sicuro di aver digitato correttamente email e password?");
            }

            if (_c.IsActive == false)
            {
                RevoContext.ActivityManager.RegisterLoginAttempt(_c.UserId);
                return JsonWarning(@"Credenziali non attive. Non è possibile effettuare il login con queste email e password");
            }

            IUserService _userService = RevoContext.ServiceProvider.GetServiceFor<User>() as IUserService;
            if (_userService == null)
            {
                RevoContext.ContextLogger.Error("Error trying to get the User Service. Configuration could be wrong...");
                return JsonError(@"Errore in fase di recupero dell'utente. Per favore, contatta gli amministratori del servizio");
            }

            IUser _u = _userService.GetByAccount(_c.UserId, String.Empty, true);
            if (_u == null)
            {
                RevoContext.ActivityManager.RegisterLoginAttempt(_c.UserId);
                return JsonWarning(@"Utente non trovato con le credenziali inserite. Per favore, contatta gli amministratori del servizio.");
            }

            if (_u.IsActive == false)
            {
                RevoContext.ActivityManager.RegisterLoginAttempt(_u);
                return JsonWarning(@"Attenzione, questo utente risulta non abilitato...");
            }

            // CORRESPONDING USER FOUND -> LOG THE USER IN
            DoUserLogin(_u);

            // RENDER RESULTs
            RevoContext.ActivityManager.RegisterLogin(_u);
            return Json(new { status = "success", data = new { message = "Login effettuato con successo" } });
        }

        
    }
}
