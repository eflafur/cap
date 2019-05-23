using GruppoCap.Authentication.Core;
using GruppoCap.Core;
using GruppoCap.Core.Identity;
using System;
using System.Linq;
using System.Web;
using GruppoCap;
using System.Web.Configuration;
using GruppoCap.Core.Caching;

namespace GruppoCap.Authentication
{
    public class IdentityManager : IIdentityManager
    {
        private IUserService _userService = null;
        private IApplicationService _applicationService = null;
        private ICredentialService _credentialService = null;
        private String _currentApplicationId = String.Empty;

        ICache _cacheProvider = null;

        #region CTOR

        public IdentityManager(IUserService userService, ICredentialService credentialService, IApplicationService applicationService, String currentApplicationId, ICache cacheProvider) 
        {
            _userService = userService;
            _credentialService = credentialService;
            _applicationService = applicationService;

            _currentApplicationId = currentApplicationId;

            _cacheProvider = cacheProvider;
        }

        #endregion

        // GET CURRENT USER NAME
        public String CurrentUsername
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                    return String.Empty;

                return GetAuthenticationUserId(HttpContext.Current.User.Identity.Name);
            }
        }

        // GET CURRENT USER DOMAIN
        public String CurrentUserDomain
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                    return String.Empty;


                return GetAuthenticationUserDomain(HttpContext.Current.User.Identity.Name);
            }
        }

        // GET CURRENT USER
        public IUser CurrentUser
        {
            get
            {
                if (CurrentUsername.IsNullOrWhiteSpace())
                    return null;

                Boolean _useCredential = Ambient.CurrentAuthenticationMode == AuthenticationMode.Forms;

                IUser _currentUser = null;

                if (_cacheProvider == null)
                {
                    if (_currentApplicationId.IsNullOrWhiteSpace())
                        return _userService.GetByAccount(CurrentUsername, CurrentUserDomain, _useCredential);
                    else
                        return _userService.GetByAccountWithGroupings(CurrentUsername, CurrentUserDomain, _currentApplicationId, _useCredential);
                }

                String _cacheKey = CachingUtils.GetCacheKey(CurrentUsername);

                _currentUser = _cacheProvider.GetByKey<IUser>(_cacheKey);
                if (_currentUser == null)
                {
                    _currentUser = _currentApplicationId.IsNullOrWhiteSpace()
                        ? _userService.GetByAccount(CurrentUsername, CurrentUserDomain, _useCredential)
                        : _userService.GetByAccountWithGroupings(CurrentUsername, CurrentUserDomain, _currentApplicationId, _useCredential)
                    ;

                    _cacheProvider.Put<IUser>(_cacheKey, _currentUser, _cacheProvider.CacheMediumDurationInMinutes.Minutes());
                }

                return _currentUser;
            }
        }

        // CURRENT IP ADDRESS
        public String CurrentIPAddress
        {
            get
            {
                String result = String.Empty;
                String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ip.IsNullOrWhiteSpace() == false)
                {
                    String[] ipRange = ip.Split(',');
                    Int32 le = ipRange.Length - 1;
                    result = ipRange[0];
                }
                else
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return result;
            }
        }

        // GET WINDOWS AUTHENTICATION USERNAME
        private String GetWindowsAuthenticationUserId()
        {
            String _requestVariable;

            if (System.Web.HttpContext.Current.Request.ServerVariables.Count == 0)
                return String.Empty;

            _requestVariable = System.Web.HttpContext.Current.Request.ServerVariables["LOGON_USER"];

            return GetAuthenticationUserId(_requestVariable);
        }

        // GET AUTHENTICATION USERNAME
        private String GetAuthenticationUserId(String account)
        {
            if (account.IsNullOrWhiteSpace())
                return String.Empty;

            String _userId;
            _userId = account.Split('\\').LastOrDefault();

            return _userId;
        }

        // GET AUTHENTICATION DOMAIN
        private String GetAuthenticationUserDomain(String account)
        {
            if (account.IsNullOrWhiteSpace())
                return String.Empty;

            String[] _tokens;
            _tokens = account.Split('\\');

            if (_tokens.Count() > 1)
                return _tokens.FirstOrDefault();

            return String.Empty;
        }

        // IS USER ENABLED
        public Boolean IsUserEnabled(String userId)
        {
            if (CurrentUser == null)
                throw new ArgumentNullException("Current User");

            return CurrentUser.IsActive;
        }

        // IS USER ENABLED FOR APPLICATION
        public Boolean IsUserEnabledForApplication(String applicationId, String userId)
        {
            Boolean? _isUserEnabledForApplication = null;

            if(_cacheProvider == null)
                return _userService.IsUserEnabledForApplication(applicationId, userId);

            String _cacheKey = CachingUtils.GetCacheKey("{0}-enabled-for-{1}".FormatWith(userId, applicationId));

            _isUserEnabledForApplication = _cacheProvider.GetByKey<Boolean?>(_cacheKey);
            if(_isUserEnabledForApplication.HasValue == false)
            {
                _isUserEnabledForApplication = _userService.IsUserEnabledForApplication(applicationId, userId);
                _cacheProvider.Put<Boolean>(_cacheKey, _isUserEnabledForApplication.Value, _cacheProvider.CacheMediumDurationInMinutes.Minutes());
            }

            return _isUserEnabledForApplication.Value;
        }

        // CURRENT APPLICATION
        public IApplication CurrentApplication
        {
            get
            {
                IApplication _currentApplication = null;

                if (_cacheProvider == null)
                    return _applicationService.GetById(Ambient.CurrentApplicationId);

                String _cacheKey = CachingUtils.GetCacheKey("{0}:{1}".FormatWith("current-app-id", Ambient.CurrentApplicationId));

                _currentApplication = _cacheProvider.GetByKey<IApplication>(_cacheKey);
                if (_currentApplication == null)
                {
                    _currentApplication = _applicationService.GetById(Ambient.CurrentApplicationId);
                    _cacheProvider.Put<IApplication>(_cacheKey, _currentApplication, _cacheProvider.CacheMediumDurationInMinutes.Minutes());
                }

                return _currentApplication;
            }
        }
    }
}
