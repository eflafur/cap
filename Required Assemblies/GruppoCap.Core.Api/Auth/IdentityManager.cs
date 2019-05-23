using GruppoCap.Core.Identity;
using System;
using System.Web;
using GruppoCap.Core.Caching;

namespace GruppoCap.Core.Api
{
    public class IdentityManager : IIdentityManager
    {

        private String _currentApplicationId = String.Empty;

        ICache _cacheProvider = null;

        #region CTOR

        public IdentityManager(String currentApplicationId, ICache cacheProvider)
        {

            _currentApplicationId = currentApplicationId;

            _cacheProvider = cacheProvider;
        }

        #endregion

        // GET CURRENT USER NAME
        public String CurrentUsername
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // GET CURRENT USER DOMAIN
        public String CurrentUserDomain
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // GET CURRENT USER
        public IUser CurrentUser
        {
            get
            {
                throw new NotImplementedException();
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

        // IS USER ENABLED
        public Boolean IsUserEnabled(String userId)
        {
            throw new NotImplementedException();
        }

        // IS USER ENABLED FOR APPLICATION
        public Boolean IsUserEnabledForApplication(String applicationId, String userId)
        {
            throw new NotImplementedException();
        }

        // CURRENT APPLICATION
        public IApplication CurrentApplication
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
