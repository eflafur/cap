using GruppoCap;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GruppoCap.Url
{
    public class UrlUtils
    {

        // CONSTs
        private static readonly Regex _Regex_StartsWithScheme = new Regex(@"^[a-zA-Z][azA-Z0-9+.-]+:.*", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


        // COMBINE TOKENs
        public static String CombineTokens(params String[] urlTokens)
        {
            return StringUtils.CombineStringsWithCharSeparator('/', urlTokens);
        }

        // IS QUERYSTRING PRESENT
        public static Boolean IsQueryStringPresent(String url)
        {
            if (url.IsNullOrWhiteSpace())
                return false;

            return url.IndexOf('?') >= 0;
        }

        // EXTRACT QUERYSTRING
        public static String ExtractQueryString(String url)
        {
            if (IsQueryStringPresent(url) == false)
                return null;

            return url.Substring(url.IndexOf('?') + 1);
        }

        // STRING DICTIONARY TO QUERYSTRING
        public static String StringDictionaryToQueryString(StringDictionary parameters)
        {
            if (parameters.IsNullOrEmpty())
                return String.Empty;

            StringBuilder sb1;

            sb1 = new StringBuilder();

            foreach (String key in parameters.Keys)
            {
                // IF NOT EMPTY -> ADD THE "&" CHAR
                if (sb1.Length > 0)
                    sb1.Append("&");

                // ATTACH THE KEY+VALUE BLOCK
                sb1.AppendFormat(@"{0}={1}", key, parameters[key]);
            }

            return sb1.ToString();
        }

        // APPEND QUERYSTRING
        public static String AppendQueryString(String url, String queryString)
        {
            // EMPTY QUERYSTRING CHECK
            if (queryString.IsNullOrWhiteSpace())
                return url;

            if (queryString.StartsWith("?") || queryString.StartsWith("&"))
            {
                // QUERYSTRING STARTS WITH "?" OR "&" --> REMOVE IT
                queryString = queryString.Substring(1);
            }

            if (url.IndexOf("?") == -1)
            {
                // URL DOES -NOT- CONTAINS "?" --> APPEND IT
                url += "?";
            }
            else
            {
                // URL CONTAINS "?"
                if (!url.EndsWith("?") && !url.EndsWith("&"))
                {
                    // URL CONTAINS "?" BUT NOT ENDS WITH "?" OR "&" --> ADD "&"
                    url += "&";
                }
            }

            return url + queryString;
        }

        // APPEND QUERYSTRING FROM STRING DICTIONARY
        public static String AppendQueryStringFromStringDictionary(String url, StringDictionary parameters)
        {
            // IF NO PARAMETERs -> RETURN IMMEDIATELY
            if (parameters.IsNullOrEmpty())
                return url;

            return UrlUtils.AppendQueryString(url, StringDictionaryToQueryString(parameters));
        }

        // APPEND CURRENT QUERYSTRING
        public static String AppendCurrentQueryString(String url)
        {
            return AppendQueryString(url, HttpContext.Current.Request.QueryString.ToString());
        }

        // REMOVE QUERYSTRING
        public static String RemoveQueryString(String url)
        {
            Int32 i;

            i = url.IndexOf('?');

            if (i == -1)
                return url;

            return url.Substring(0, i);
        }

        // EXTRACT FILE NAME
        public static String ExtractFileName(String url)
        {
            if (url.IsNullOrWhiteSpace())
                return url;

            // REMOVE THE QUERYSTRING
            url = RemoveQueryString(url);

            Int32 pos = 0;

            pos = url.LastIndexOf("/");

            if (pos > -1)
            {
                // "/" PRESENT --> REMOVE THE LEFT PART
                if (pos + 1 < url.Length)
                    url = url.Substring(pos + 1);
                else
                    url = String.Empty;
            }

            if (url.Contains("."))
            {
                // "." FOUND --> FILENAME FOUND
                return url;
            }
            else
            {
                // "." NOT FOUND --> NO FILENAME FOUND
                return String.Empty;
            }
        }

        // STARTS WITH SCHEME
        public static Boolean StartsWithScheme(String url, String scheme)
        {
            //Ensure.Arg(() => scheme).IsNotNullOrWhiteSpace();

            if (url.IsNullOrWhiteSpace())
                return false;

            return url.Trim().StartsWith(scheme.EnsureEndsWith(":"));
        }

        // STARTS WITH ANY SCHEME
        public static Boolean StartsWithAnyScheme(String url)
        {
            if (url.IsNullOrWhiteSpace())
                return false;

            return _Regex_StartsWithScheme.IsMatch(url.Trim());
        }

        // IS FRAGMENT
        public static Boolean IsFragment(String url)
        {
            if (url.IsNullOrWhiteSpace())
                return false;

            return url.Trim().StartsWith("#");
        }

        // ENSURE STARTS WITH HTTP OR HTTPS
        public static String EnsureStartsWithHttpOrHttps(String url, Boolean useHttpsIfNoScheme = false)
        {
            if (url.IsNullOrWhiteSpace())
                return null;

            if (url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                return url;

            return (useHttpsIfNoScheme ? "http://" : "http://") + url;
        }

        // IS MANIPOLABLE
        private static Boolean IsManipolable(String url)
        {
            if (url.IsNullOrWhiteSpace())
                return false;

            if (StartsWithAnyScheme(url))
                return false;

            if (IsFragment(url))
                return false;

            return true;
        }

        // TO ABSOLUTE
        public static String ToAbsolute(String url)
        {
            // CHECK
            if (IsManipolable(url) == false)
                return url ?? String.Empty;

            if (IsQueryStringPresent(url) == false)
                return VirtualPathUtility.ToAbsolute(url);

            String qs;

            // EXTRACT THE QUERYSTRING
            qs = ExtractQueryString(url);

            // REMOVE THE QUERYSTRING FROM THE URL
            url = RemoveQueryString(url);

            // MAKE THE URL ABSOLUTE
            url = VirtualPathUtility.ToAbsolute(url);

            // RE-APPEND THE ORIGINAL QUERYSTRING
            url = AppendQueryString(url, qs);

            return url;
        }

        // ABSOLUTE TO EXPOSABLE
        public static String ToExposable(String url, String baseUrl = null)
        {
            // CHECK
            if (IsManipolable(url) == false)
                return url ?? String.Empty;

            if (url.StartsWith("/") == false)
            {
                // IT'S NOT ABSOLUTE -> MAKE IT ABSOLUTE
                url = ToAbsolute(url);
            }

            // ENSURE WE HAVE A BASE URL
            if (baseUrl.IsNullOrWhiteSpace())
                baseUrl = HttpContext.Current.Request.Url.Authority;

            // NORMALIZE BASE URL
            baseUrl = EnsureStartsWithHttpOrHttps(baseUrl);

            // COMBINE AND RETURNS
            return CombineTokens(baseUrl, url);
        }

        // GET GRAVATAR IMAGE URL
        public static String GetGravatarImageUrl(String email, Int32? size = null, String defaultImageUrl = null)
        {
            //Ensure.Arg(() => email).IsNotNullOrWhiteSpace();
            if (email.IsNullOrWhiteSpace())
                return null;

            // NORMALIZE EMAIL
            email = email.Trim().ToLowerInvariant();

            String hash, imageUrl;

            // CALCULATE THE HASH
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                var sb = new StringBuilder(hashedBytes.Length * 2);

                for (var i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("X2"));
                }

                hash = sb.ToString().ToLowerInvariant();
            }

            // INITIAL VALUE
            //imageUrl = "http://www.gravatar.com/avatar.php".AppendQueryString("gravatar_id=" + hash);
            imageUrl = "http://www.gravatar.com/avatar/{0}".FormatWith(hash);

            // PARAM - SIZE
            if (size.HasValue)
            {
                imageUrl = AppendQueryString(imageUrl, "size=" + size.Value.ToString());
            }

            // PARAM - DEFAULT IMAGE
            if (defaultImageUrl.IsNullOrWhiteSpace() == false)
            {
                imageUrl = AppendQueryString(imageUrl, "default=" + HttpUtility.UrlEncode(defaultImageUrl));
            }

            return imageUrl;
        }

    }
}
