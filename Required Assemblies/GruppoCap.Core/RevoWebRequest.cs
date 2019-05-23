using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GruppoCap.Core
{
    public class RevoWebRequest : IRevoWebRequest
    {
        // PRIVATE MEMBERs
        private IRevoContext _ctx = null;
        private IDictionary<String, Object> _extraData = new Dictionary<String, Object>();

        // CTOR
        public RevoWebRequest(IRevoContext ctx)
        {
            this._ctx = ctx;
        }

        // REVO CONTEXT
        public IRevoContext RevoContext
        {
            get { return this._ctx; }
        }

        // WEB CONTEXT
        public HttpContext WebContext
        {
            get { return System.Web.HttpContext.Current; }
        }

        // CURRENT IP ADDRESS
        private String _currentIPAddress = null;
        public String CurrentIPAddress
        {
            get
            {
                if (_currentIPAddress == null)
                    _currentIPAddress = _ctx.IdentityManager.CurrentIPAddress;

                return _currentIPAddress;
            }
            private set
            {
                _currentIPAddress = value;
            }
        }

        // CURRENT USERNAME
        private String _currentUsername = null;
        public String CurrentUsername
        {
            get
            {
                if (_currentUsername == null)
                    _currentUsername = _ctx.IdentityManager.CurrentUsername;

                return _currentUsername;
            }
            private set
            {
                _currentUsername = value;
            }
        }

        // CURRENT USER
        private IUser _currentUser = null;
        public IUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = _ctx.IdentityManager.CurrentUser;

                return _currentUser;
            }
            private set
            {
                _currentUser = value;
            }
        }

        // IMPERSONATED USER
        //private IUser _impersonatedUser = null;
        //public IUser ImpersonatedUser
        //{
        //    get
        //    {
        //        //if (_currentUser == null)
        //        //    _currentUser = _ctx.IdentityManager.GetCurrentUser();

        //        return _impersonatedUser;
        //    }
        //    set
        //    {
        //        _impersonatedUser = value;
        //    }
        //}

        //private String _currentIPAddress = null;
        //public String CurrentIPAddress 

        // EXTRA DATA

        public IDictionary<String, Object> ExtraData
        {
            get { return _extraData; }
        }

        // CLEAR CURRENT USER
        public void ClearCurrentUser()
        {
            this.CurrentUser = null;
        }
    }
}
