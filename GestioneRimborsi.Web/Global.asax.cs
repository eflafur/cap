using Castle.Windsor;
using Castle.Windsor.Installer;
using GestioneRimborsi.Core;
using GruppoCap.Activity.Core;
using GruppoCap.Authentication.Core;
using GruppoCap.Core;
using GruppoCap.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GestioneRimborsi.Web
{
    public class MvcApplication : RevoApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // GET THE GENERIC TYPE RESOLVER (GENERIC IOC CONTAINER)
            IGenericTypeResolver _resolver;
            _resolver = GetGenericTypeResolver();

            // CASTLE WINDSOR CONFIGURATION
            BootstrapContainer(_resolver);

            // REVO CONTEXT RESOLVE AND SET
            RevoContextHelpers.SetCurrentRevoContext(_resolver.ResolveInstance<IRevoContext>());

            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            RegisterRevoArchitectureServicesAndPermissions(ctx);

            RegisterCustomServicesAndPermissions(ctx);
        }

        // BOOTSTRAP CONTAINER
        private void BootstrapContainer(IGenericTypeResolver genericTypeResolver)
        {
            IWindsorContainer _container;
            _container = (genericTypeResolver as CastleWindsorGenericTypeResolver).WindsorContainer;

            // COMPONENTs REGISTRATION
            TryRegisterCoreConfigFilesWithAmbient(_container, "revo.core");
            TryRegisterCoreConfigFilesWithAmbient(_container, "revo.authentication");
            TryRegisterCoreConfigFilesWithAmbient(_container, "revo.activity");
            TryRegisterCoreConfigFilesWithAmbient(_container, "revo.permissioning");

            TryRegisterCoreConfigFilesWithAmbient(_container, "gestionerimborsi");

            // CONTROLLERs REGISTRATION
            _container.Install(FromAssembly.This());

            // FACTORY STUFF
            var _controllerFactory = new ControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }

        // APPLICATION - BEGIN REQUEST
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            IRevoContext ctx;

            // GET CURRENT REVO CONTEXT
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            // REVO WEB REQUEST
            RevoContextHelpers.SetCurrentRevoWebRequest(ctx.GenericTypeResolver.ResolveInstance<IRevoWebRequest>());
        }

        // SESSION START
        protected void Session_Start(Object sender, EventArgs e)
        {
            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();
            ctx.ActivityManager.RegisterLogin();
        }

        // SESSION END
        protected void Session_End(Object sender, EventArgs e)
        {
            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();
            if (ctx != null)
            {
                if (ctx.ActivityManager != null)
                {
                    ctx.ActivityManager.RegisterLogout();
                }
            }
        }

        // REGISTER REVO ARCHITECTURE SERVICEs
        private void RegisterRevoArchitectureServicesAndPermissions(IRevoContext ctx)
        {
            // USERs
            ctx.RegisterService<User, IUserService>();
            ctx.RegisterPermissionFor<User>();

            // CREDENTIAL
            ctx.RegisterService<Credential, ICredentialService>();

            // APPLICATIONs
            ctx.RegisterService<Application, IApplicationService>();

            // ACTIVITIES
            ctx.RegisterService<Activity, IActivityService>();
            ctx.RegisterPermission("activity.manage", "Activities", false, false);

            // CAP GROUPINGs
            ctx.RegisterService<CapGrouping, ICapGroupingService>();
            ctx.RegisterPermissionFor<CapGrouping>();

            ctx.RegisterPermission("tracking.view", "Tracking", false, false);

            ctx.RegisterPermission("security.manage", "Security", false, false);

            ctx.RegisterPermission("users.assign-to-grouping", "Entities - User", false, false);
        }

        // REGISTER CUSTOM SERVICEs
        private void RegisterCustomServicesAndPermissions(IRevoContext ctx)
        {

            ctx.RegisterService<Rimborso, ILottoRimborsiService>();
            //ctx.RegisterPermissionFor<Rimborso>(false, false);

            ctx.RegisterService<Cliente, IClienteService>();            

            ctx.RegisterService<GestioneRimborso, IRimborsoService>();
            //ctx.RegisterPermissionFor<GestioneRimborso>(false, false);

            ctx.RegisterService<FuoriStandard, IFuoriStandardService>();

            ctx.RegisterService<RettificaFuoriStandard, IRettificaFuoriStandardService>();

            ctx.RegisterService<ClienteBonusIdrico, IRimborsoService>();


            //TODO: DA ELIMINARE 
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.GestoreCliente", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.OperatoreRimborso", "COM 2.0 Gestione Rimborsi", false, false);
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.OperatoreBonusIdrico", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.OperatoreIndennizzi", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.OperatoreDisposizioni", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneFuoriStandard.GestionePermessi.OperatoreFuoriStandard", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);            
            //ctx.RegisterPermission("GestioneFuoriStandard.GestionePermessi.InserisciNuovoFuoriStandard", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneFuoriStandard.GestionePermessi.ResponsabileApprovazione", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneFuoriStandard.GestionePermessi.SolaLettura", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("GestioneRimborsi.GestionePermessi.ImpersonificaUtente", "COM 2.0 Gestione Rimborsi e Fuori Standard", false, false);
            //ctx.RegisterPermission("SuperUserPermissionCode", "SuperUser", false, false);


            //GESTIONE RIMBORSI ADMINISTRATION
            ctx.RegisterPermission("GestioneRimborsi.Setup.Manage", "Setup", false, false);

            //GESTIONE RIMBORSI
            ctx.RegisterPermission("gri.rimborsi", "Gestione Rimborsi", false, false);
            ctx.RegisterPermission("gri.iban.update", "Gestione Rimborsi", false, false);
            ctx.RegisterPermission("gri.disposizioni", "Gestione Rimborsi", false, false);
            ctx.RegisterPermission("gri.disposizioni.scegliUtenti", "Gestione Rimborsi", false, false);
            ctx.RegisterPermission("gri.rimborsi.supervisor", "Gestione Rimborsi", false, false);            

            //GESTIONE FUORI STANDARD            
            ctx.RegisterPermission("gfs.fuoriStandard", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.validazione", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.validazione.update", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.approvazione", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.approvazione.update", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.nonIndennizzabili", "Gestione Fuori Standard", false, false);            
            ctx.RegisterPermission("gfs.fuoriStandard.nonIndennizzabili.update", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.reportPrestazioni", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.storico", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.gestisciRettifiche", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.gestisciRettifiche.update", "Gestione Fuori Standard", false, false);
            ctx.RegisterPermission("gfs.fuoriStandard.annullaPrestazione", "Gestione Fuori Standard", false, false);
                
            //BONUS IDRICO
            ctx.RegisterPermission("bi.bonusIdrico", "Bonus Idrico", false, false);

        }
    }
}
