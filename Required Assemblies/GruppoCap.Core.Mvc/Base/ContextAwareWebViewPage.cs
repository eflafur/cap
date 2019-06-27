using System.Web.Mvc;

namespace GruppoCap.Core.Mvc
{
    public abstract class ContextAwareWebViewPage : WebViewPage
    {
        public IRevoContext RevoContext { get; protected set; }
        public IRevoWebRequest RevoRequest { get; protected set; }

        public IUser CurrentUser { get; protected set; }

        // INITIALIZE PAGE (OVERRIDE)
        protected override void InitializePage()
        {
            base.InitializePage();

            RevoContext = RevoContextHelpers.GetCurrentRevoContext();
            RevoRequest = RevoContextHelpers.GetCurrentRevoWebRequest();

            CurrentUser = RevoRequest.CurrentUser;

        }
    }

    public abstract class ContextAwareWebViewPage<T> : WebViewPage<T>
    {
        public IRevoContext RevoContext { get; protected set; }
        public IRevoWebRequest RevoRequest { get; protected set; }

        public IUser CurrentUser { get; protected set; }

        // INITIALIZE PAGE (OVERRIDE)
        protected override void InitializePage()
        {
            base.InitializePage();

            RevoContext = RevoContextHelpers.GetCurrentRevoContext();
            RevoRequest = RevoContextHelpers.GetCurrentRevoWebRequest();

            CurrentUser = RevoRequest.CurrentUser;
        }
    }
}
