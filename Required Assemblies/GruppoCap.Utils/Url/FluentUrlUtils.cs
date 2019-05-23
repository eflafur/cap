using GruppoCap.Url;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace GruppoCap
{
    public static class FluentUrlUtils
    {

        // COMBINE URL TOKENs
        public static String CombineUrlTokens(this IEnumerable<String> urlTokens)
        {
            return UrlUtils.CombineTokens(urlTokens.ToArray());
        }

        // APPEND URL TOKENs
        public static String AppendUrlTokens(this String firstToken, params String[] urlTokens)
        {
            return urlTokens.Prepend(firstToken).CombineUrlTokens();
        }

        // IS QUERYSTRING PRESENT
        public static Boolean IsQueryStringPresent(this String url)
        {
            return UrlUtils.IsQueryStringPresent(url);
        }

        // EXTRACT QUERYSTRING
        public static String ExtractQueryString(this String url)
        {
            return UrlUtils.ExtractQueryString(url);
        }

        // TO QUERYSTRING
        public static String ToQueryString(this StringDictionary parameters)
        {
            return UrlUtils.StringDictionaryToQueryString(parameters);
        }

        // APPEND QUERYSTRING

        // APPEND QUERYSTRING
        public static String AppendQueryString(this String url, String queryString)
        {
            return UrlUtils.AppendQueryString(url, queryString);
        }

        // APPEND QUERYSTRING FROM STRING DICTIONARY
        public static String AppendQueryStringFromStringDictionary(this String url, StringDictionary parameters)
        {
            return UrlUtils.AppendQueryStringFromStringDictionary(url, parameters);
        }

        // REMOVE QUERYSTRING
        public static String RemoveQueryString(this String url)
        {
            return UrlUtils.RemoveQueryString(url);
        }

        // EXTRACT FILE NAME FROM URL
        public static String ExtractFileNameFromUrl(this String url)
        {
            return UrlUtils.ExtractFileName(url);
        }

        // ENSURE URL STARTS WITH HTTP OR HTTPS
        public static String EnsureUrlStartsWithHttpOrHttps(this String url, Boolean useHttpsIfNoScheme = false)
        {
            return UrlUtils.EnsureStartsWithHttpOrHttps(url, useHttpsIfNoScheme);
        }

        // TO ABSOLUTE URL
        public static String ToAbsoluteUrl(this String url)
        {
            return UrlUtils.ToAbsolute(url);
        }

        // ABSOLUTE TO EXPOSABLE URL
        public static String ToExposableUrl(this String url, String baseUrl = null)
        {
            return UrlUtils.ToExposable(url, baseUrl);
        }

    }

}
