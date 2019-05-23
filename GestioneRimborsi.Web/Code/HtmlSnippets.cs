using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GruppoCap;

namespace GestioneRimborsi.Web
{
    public static class HtmlSnippets
    {
        // ENTITY LINK
        public static MvcHtmlString EntityLink(this HtmlHelper helper, String text, String cssClass, String entityUrl)
        {
            return new MvcHtmlString(@"<a class=""text-{1} entity-link"" href=""{0}"">{2}</a>".FormatWith(
                entityUrl,
                cssClass,
                text
            ));
        }

        // MENU LINK
//        public static MvcHtmlString AreaMenuLink(this HtmlHelper helper, String label, String icon, String url, String controller)
//        {
//            var routeData = helper.ViewContext.RouteData.Values;
//            var currentController = routeData["controller"];

//            String _cssClass = String.Empty;
//            String _linkSnippet = @"<li role=""presentation"" {0}>
//                                        <a href=""{1}"">
//                                            <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
//                                        </a>
//                                    </li>";

//            if (String.Equals(controller, currentController as String, StringComparison.OrdinalIgnoreCase))
//            {
//                _cssClass = "class=\"active\"";
//            }

//            return new MvcHtmlString(_linkSnippet.FormatWith(_cssClass, url, icon, label));
//        }

//        // MENU LINK
//        public static MvcHtmlString AreaMenuLink(this HtmlHelper helper, String label, String icon, String url, Boolean forceActive = false)
//        {
//            String _cssClass = String.Empty;
//            String _linkSnippet = @"<li role=""presentation"" {0}>
//                                        <a href=""{1}"">
//                                            <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
//                                        </a>
//                                    </li>";

//            if (forceActive)
//                _cssClass = "class=\"active\"";

//            return new MvcHtmlString(_linkSnippet.FormatWith(_cssClass, url, icon, label));
//        }

        //ALERT CLASS
        public static class Alert
        {
            // ERROR
            public static String Error(String text = "", String title = "", String recoveryUrl = "", Exception exception = null)
            {
                String _titleSnippet = @"<h4><i class=""fa fa-lg fa-frown-o""></i><span class=""text-after-icon"">{0}</span></h4>";
                String _recoveryUrlSnippet = @"<br /><br /><p class=""text-center breath-on-top-20"">
                                                <a class=""alert-link"" title=""Ricarica la pagina"" href=""{0}"">
                                                    <i class=""fa fa-3x fa-refresh""></i>
                                                </a>
                                            </p>";

                String _titleBlock = title.IsNullOrWhiteSpace() ? String.Empty : _titleSnippet.FormatWith(title);
                String _recoveryUrlBlock = recoveryUrl.IsNullOrWhiteSpace() ? String.Empty : _recoveryUrlSnippet.FormatWith(recoveryUrl);

                String _errorMessage = exception == null ? String.Empty : @"<p><samp>{0}</samp></p>
                                                                    <p><samp>{1}</samp></p".FormatWith(exception.Message, exception.StackTrace);

                return @"<div class=""alert alert-danger"" role=""alert"">
                            {0}
                            <p>{1}</p>
                            {2}
                            {3}
                        </div>".FormatWith(_titleBlock, text, _errorMessage, _recoveryUrlBlock);
            }

            // WARNING
            public static String Warning(String text)
            {
                return @"<div class=""alert alert-dismissable alert-warning"" role=""alert"">
                            <i class=""fa fa-lg fa-exclamation-triangle""></i><span class=""text-after-icon"">{0}</span>
                        </div>".FormatWith(text);
            }

            // INFO
            public static String Info(String text)
            {
                return @"<div class=""alert alert-dismissable alert-info breath-on-top-20"" role=""alert"">
                            <i class=""fa fa-lg fa-info-circle""></i><span class=""text-after-icon"">{0}</span>
                        </div>".FormatWith(text);
            }

            // SUCCESS
            public static String Success(String text)
            {
                return @"<div class=""alert alert-dismissable alert-success"" role=""alert"">
                            <i class=""fa fa-lg fa-smile-o""></i><span class=""text-after-icon"">{0}</span>
                        </div>".FormatWith(text);
            }
        }
    }
}