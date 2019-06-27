using GruppoCap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestioneRimborsi.Web
{
    public static class AppExtensions
    {
        public static Boolean IsSuperUser(this IUser user)
        {

            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            return ctx.PermissionManager.GetUserGrantWithFallback("gri.rimborsi.supervisor", user.UserId);
        }

        //HAS PERMISSION
        public static Boolean HasPermissionFor(this IUser user, String permissionCode)
        {
            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            return ctx.PermissionManager.GetUserGrantWithFallback(permissionCode, user.UserId);
        }

        // HAS PERMISSION OR IS PRIVILEGED
        public static Boolean HasPermissionOrPrivileged(this IUser user, String permissionCode)
        {
            if (user.IsPrivileged)
                return true;

            return HasPermissionFor(user, permissionCode);
        }

        // IS ENABLED FOR APPLICATION
        public static Boolean IsEnabledForApp(this IUser user, String applicationId)
        {
            if (user.IsPrivileged)
                return true;

            IRevoContext ctx;
            ctx = RevoContextHelpers.GetCurrentRevoContext();

            return ctx.IdentityManager.IsUserEnabledForApplication(applicationId, user.UserId);
        }
        public static List<SelectListItem> ToSelectListItem<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null)
                return null;
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                listItems.Add(new SelectListItem() { Text = pair.Value.ToString(), Value = pair.Key.ToString() });
            }
            return listItems;
        }
        public static List<SelectListItem> ToSelectListItem<TKey>(this List<TKey> dict)
        {
            if (dict == null)
                return null;
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var pair in dict)
            {
                listItems.Add(new SelectListItem() { Text = pair.ToString(), Value = pair.ToString() });
            }
            return listItems;
        }
    }
}