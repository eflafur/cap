
using System.ComponentModel;
namespace GruppoCap.Core
{
    public enum ActivityVerb
    {
        [Description("Logged in")]
        Login = 1,
        [Description("viewed")]
        View = 2,
        [Description("created")]
        Create = 3,
        [Description("updated")]
        Update = 4,
        [Description("deleted")]
        Delete = 5,
        [Description("linked")]
        Link = 6,
        [Description("unlinked")]
        Unlink = 7,
        [Description("starts impersonating")]
        Impersonate = 8,
        [Description("do something")]
        CustomActivity = 9,
        [Description("tried to login")]
        TryToLogin = 95,
        [Description("tried to login")]
        TryToLoginInactiveUser = 96,
        [Description("tried to login")]
        TryToLoginNotEnabledUser = 97,
        [Description("logged out")]
        Logout = 99
    }
}