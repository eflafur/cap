using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GruppoCap.Core;
using GruppoCap.Core.Mvc;
using System.Web.Configuration;
using System.Web;
using Newtonsoft.Json;

namespace GruppoCap
{
    public static class CommonSnippets
    {
        // TO RAW
        public static MvcHtmlString ToRaw(this String s)
        {
            return new MvcHtmlString(s);
        }

        public const String IsNotActive = @"<i class=""fa fa-lg fa-fw fa-minus-circle text-danger text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Non attiva""></i>";
        public const String IsPrivileged = @"<i class=""fa fa-lg fa-fw fa-user-secret text-warning text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Privileged""></i>";

        public const String IsOK = @"<i class=""fa fa-lg fa-fw fa-thumbs-o-up text-success text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Tutto OK""></i>";
        public const String IsSoSo = @"<i class=""fa fa-lg fa-fw fa-hand-stop-o text-orange text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Per essere andata è andata, ma qualcosina fuori posto c'è...""></i>";
        public const String IsKO = @"<i class=""fa fa-lg fa-fw fa-thumbs-o-down text-danger text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Totale fallimento""></i>";



        // ALLOWED GRANT
        public const String AllowedGrant = @"<span class=""fa-stack fa-lg"" data-toggle=""tooltip"" data-placement=""top"" title=""Direct grant"">
						                        <i class=""fa fa-unlock-alt fa-lg fa-stack-1x text-success"" style=""margin-top:2px;""></i>
					                        </span>";

        // DENIED GRANT
        public const String DeniedGrant = @"<span class=""fa-stack fa-lg"" data-toggle=""tooltip"" data-placement=""top"" title=""Direct grant"">
						    	                <i class=""fa fa-lock fa-lg fa-stack-1x text-danger"" style=""margin-top:2px;""></i>
							                </span>";

        // INHERITED ALLOWED GRANT
        public const String InheritedAllowedGrant = @"<span class=""fa-stack fa-lg"" data-toggle=""tooltip"" data-placement=""top"" title=""Inherited grant"">
					                                    <i class=""fa fa-unlock-alt fa-lg fa-stack-1x text-success"" style=""margin-top:3px; opacity: 0.7;""></i>
					                                    <i class=""fa fa-chevron-circle-left fa-stack-1x "" style=""margin-top:2px; margin-left: 10px; color: #555""></i>
				                                    </span>";

        // INHERITED DENIED GRANT
        public const String InheritedDeniedGrant = @"<span class=""fa-stack fa-lg"" data-toggle=""tooltip"" data-placement=""top"" title=""Inherited grant"">
						    				            <i class=""fa fa-lock fa-lg fa-stack-1x text-danger"" style=""margin-top:3px; opacity: 0.7;""></i>
						    				            <i class=""fa fa-chevron-circle-left fa-stack-1x "" style=""margin-top:2px; margin-left: 10px; color: #555""></i>
										            </span>";

        public const String IsMandatory = @"<i class=""fa fa-fw fa-thumb-tack text-primary text-after-icon"" data-toggle=""tooltip"" data-placement=""right"" title=""Dato obbligatorio"" style=""cursor: pointer;""></i>";



        // ICON LINK TO HOMEPAGE
        public static MvcHtmlString IconLinkToHomepage
        {
            get
            {
                return @"<a href=""{0}"" data-toggle=""tooltip"" data-placement=""right"" title=""Torna alla Homepage""><i class=""fa fa-home""></i></a>"
                    .FormatWith(CommonUrls.BaseUrl)
                    .ToRaw()
                ;
            }
        }

        // BREADCRUMB ICON LINK
        public static MvcHtmlString BreadcrumbIconLink(String url, String tooltipText, String icon, String iconDimension = "fw")
        {
            return @"<a href=""{0}"" data-toggle=""tooltip"" data-placement=""right"" title=""{1}""><i class=""fa fa-{2} fa-{3}""></i></a>"
                .FormatWith(url, tooltipText, iconDimension, icon)
                .ToRaw()
            ;
        }

        // FONT AWESOME CHECK BOX
        public static MvcHtmlString FontAwesomeCheckbox(this HtmlHelper helper, String controlId, Boolean isCurrentlyActive, String activeText, String notActiveText, String activeClass, String notActiveClass, String currentClass, String currentDescription, Boolean enabled = true)
        {
            String _checked = isCurrentlyActive ? @"<i class=""fa fa-check fa-stack-1x text-success""></i>" : String.Empty;

            String _eventOnClick = enabled ? @"toggleFontAwesomeCheckbox(this, '{0}', '{1}', '{2}', '{3}');"
                .FormatWith(
                    activeText.Replace("'", "&#39;"),
                    notActiveText.Replace("'", "&#39;"),
                    activeClass,
                    notActiveClass
                ) : @"javascript:void(0);"
            ;

            if (Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                return new MvcHtmlString(@"<div class=""input-group"">
                        <span id=""{0}"" class=""fa-stack fa-lg toggle-checkbox"" onclick=""{1}"">
                            <i class=""fa fa-square-o fa-stack-2x""></i>
                            {2}
                        </span>
                        <span class=""{3} text-after-icon toggle-checkbox-description mt-FAchkbox"">{4}</span>
                    </div>"
                    .FormatWith(
                        controlId,
                        _eventOnClick,
                        _checked,
                        currentClass,
                        currentDescription
                    )
                );
            }
            else
            {
                return new MvcHtmlString(@"<div class=""input-group"">
                        <span id=""{0}"" class=""fa-stack fa-lg toggle-checkbox"" onclick=""{1}"">
                            <i class=""fa fa-square-o fa-stack-2x""></i>
                            {2}
                        </span>
                        <span class=""{3} text-after-icon toggle-checkbox-description"">{4}</span>
                    </div>"
                    .FormatWith(
                        controlId,
                        _eventOnClick,
                        _checked,
                        currentClass,
                        currentDescription
                    )
                );
            }
                
        }

        // DATEPICKER CLEAR BUTTON
        public static MvcHtmlString DatepickerClearButton()
        {
            if (Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                return new MvcHtmlString(@"
                    <div class=""input-group-append"" >
                        <button class=""btn btn-outline-danger clear-datepicker"" type=""button"" onclick=""clearDatepickerInput(this);"" data-toggle=""tooltip"" data-placement=""top"" title=""Rimuovi la data"">
                            <i class=""fa fa-calendar-times-o"" ></i>
                        </button>
                    </div>
                ");
            }

            return new MvcHtmlString(@"
                <span class=""input-group-btn"" >
                    <button class=""btn btn-default clear-datepicker"" type=""button"" onclick=""clearDatepickerInput(this);"" data-toggle=""tooltip"" data-placement=""top"" title=""Rimuovi la data"">
                        <i class=""fa fa-calendar-times-o"" ></i>
                    </button>
                </span>
            ");
        }

        // DATEPICKER CLEAR BUTTON
        public static MvcHtmlString DatepickerClearButton(this HtmlHelper helper)
        {
            if(Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                return new MvcHtmlString(@"
                    <div class=""input-group-append"" >
                        <button class=""btn btn-outline-danger clear-datepicker"" type=""button"" onclick=""clearDatepickerInput(this);"" data-toggle=""tooltip"" data-placement=""top"" title=""Rimuovi la data"">
                            <i class=""fa fa-calendar-times-o"" ></i>
                        </button>
                    </div>
                ");
            }

            return new MvcHtmlString(@"
                <span class=""input-group-btn"" >
                    <button class=""btn btn-default clear-datepicker"" type=""button"" onclick=""clearDatepickerInput(this);"" data-toggle=""tooltip"" data-placement=""top"" title=""Rimuovi la data"">
                        <i class=""fa fa-calendar-times-o text-danger"" ></i>
                    </button>
                </span>
            ");
        }

        // GET PAGE TITLE
        public static MvcHtmlString GetPageTitle(this HtmlHelper helper)
        {
            String _viewbagTitle = helper.ViewBag.Title;
            return helper.GetPageTitle(_viewbagTitle);
        }

        // GET PAGE TITLE
        public static MvcHtmlString GetPageTitle(this HtmlHelper helper, String title)
        {
            String _title = title.IsNullOrWhiteSpace() ? Ambient.CurrentApplicationName : "{0} - {1}".FormatWith(title, Ambient.CurrentApplicationName);
            return new MvcHtmlString(_title);
        }



        // ENTITY LINK
        public static MvcHtmlString EntityLink(this HtmlHelper helper, String text, String cssClass, String entityUrl)
        {
            return new MvcHtmlString(@"<a class=""text-{1} entity-link"" href=""{0}"">{2}</a>".FormatWith(
                entityUrl,
                cssClass,
                text
            ));
        }

        // MENU PILLS
        public static MvcHtmlString AreaMenuPills(this HtmlHelper helper, String label, String icon, String url, String controller)
        {
            var routeData = helper.ViewContext.RouteData.Values;
            var currentController = routeData["controller"];

            String _cssClass = String.Empty;
            String _linkSnippet = String.Empty;

            if (Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                if (String.Equals(controller, currentController as String, StringComparison.OrdinalIgnoreCase))
                {
                    _cssClass = "active";
                }

                _linkSnippet = @"<li class=""nav-item"">
                                    <a class=""nav-link {0}"" href=""{1}"">
                                        <span class=""text-white"">{3}</span>
                                    </a>
                                </li>";
            }
            else
            {
                if (String.Equals(controller, currentController as String, StringComparison.OrdinalIgnoreCase))
                {
                    _cssClass = "class=\"active\"";
                }

                _linkSnippet = @"<li role=""presentation"" {0}>
                                    <a href=""{1}"">
                                        <span class=""text-white"">{3}</span>
                                    </a>
                                </li>";
            }

            return new MvcHtmlString(_linkSnippet.FormatWith(_cssClass, url, icon, label));
        }


        // MENU LINK
        public static MvcHtmlString AreaMenuLink(this HtmlHelper helper, String label, String icon, String url, String controller)
        {
            var routeData = helper.ViewContext.RouteData.Values;
            var currentController = routeData["controller"];

            String _cssClass = String.Empty;
            String _linkSnippet = String.Empty;

            if (Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                if (String.Equals(controller, currentController as String, StringComparison.OrdinalIgnoreCase))
                {
                    _cssClass = "active";
                }

                _linkSnippet = @"<li class=""nav-item"">
                                    <a class=""nav-link {0}"" href=""{1}"">
                                        <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
                                    </a>
                                </li>";
            }
            else
            {
                if (String.Equals(controller, currentController as String, StringComparison.OrdinalIgnoreCase))
                {
                    _cssClass = "class=\"active\"";
                }

                _linkSnippet = @"<li role=""presentation"" {0}>
                                    <a href=""{1}"">
                                        <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
                                    </a>
                                </li>";
            }

            return new MvcHtmlString(_linkSnippet.FormatWith(_cssClass, url, icon, label));
        }

        // MENU LINK
        public static MvcHtmlString AreaMenuLink(this HtmlHelper helper, String label, String icon, String url, Boolean forceActive = false)
        {
            String _cssClass = String.Empty;
            String _linkSnippet = String.Empty;

            if (Ambient.IsCurrentApplicationBootstrapVersionNewerThan4)
            {
                _linkSnippet = @"<li class=""nav-item"">
                                    <a class=""nav-link {0}"" href=""{1}"">
                                        <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
                                    </a>
                                </li>";

                if (forceActive)
                    _cssClass = "active";
            }
            else
            {
                _linkSnippet = @"<li role=""presentation"" {0}>
                                    <a href=""{1}"">
                                        <i class=""fa fa-fw fa-lg {2}""></i><span class=""text-after-icon"">{3}</span>
                                    </a>
                                </li>";

                if (forceActive)
                    _cssClass = "class=\"active\"";
            }

            return new MvcHtmlString(_linkSnippet.FormatWith(_cssClass, url, icon, label));
        }



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


    public static class CommonExtensions
    {
        public static void RedirectIfNotFound(this IUser user)
        {
            if (user == null)
            {
                if (Ambient.CurrentAuthenticationMode == AuthenticationMode.Windows)
                {
                    HttpContext.Current.Response.Redirect(CommonUrls.UserNotAuthenticated);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(CommonUrls.UserLogin);
                }
            }
        }


        public static ContentResult ToJson(this IEntity entity)
        {
            if (entity == null)
                return null;

            ContentResult _res = new ContentResult();

            JsonSerializerSettings _settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented,
                DateFormatString = "dd/MM/yyyy HH:mm"
            };

            _res.Content = JsonConvert.SerializeObject(entity, _settings);

            _res.ContentType = "application/json";

            return _res;
        }


    }

}
