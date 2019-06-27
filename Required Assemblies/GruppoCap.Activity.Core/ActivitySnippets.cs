using GruppoCap.Core;
using System;

namespace GruppoCap.Activity.Core
{
    public static class ActivitySnippets
    {
        // ACTIVITY TEXT
        public static class ActivityText
        {
            public const String Login = @"<i class=""fa fa-lg fa-fw fa-sign-in text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Login""></i>";
            public const String TryToLogin = @"<i class=""fa fa-lg fa-fw fa-sign-in text-after-icon text-danger"" data-toggle=""tooltip"" data-placement=""top"" title=""Login Attempt""></i>";
            public const String TryToLoginInactiveUser = @"<i class=""fa fa-lg fa-fw fa-sign-in text-after-icon text-danger"" data-toggle=""tooltip"" data-placement=""top"" title=""Login Attempt""></i>";
            public const String TryToLoginNotEnabledUser = @"<i class=""fa fa-lg fa-fw fa-sign-in text-after-icon text-danger"" data-toggle=""tooltip"" data-placement=""top"" title=""Login Attempt""></i>";
            public const String Logout = @"<i class=""fa fa-lg fa-fw fa-sign-out text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Logout""></i>";
            public const String View = @"<i class=""fa fa-lg fa-fw fa-eye text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""View""></i>";
            public const String Update = @"<i class=""fa fa-lg-fa-fw fa-edit text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Update""></i>";
            public const String Create = @"<i class=""fa fa-lg fa-fw fa-plus text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Create""></i>";
            public const String Delete = @"<i class=""fa fa-lg fa-fw fa-trash-o text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Delete""></i>";
            public const String Link = @"<i class=""fa fa-lg fa-fw fa-chain text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Link""></i>";
            public const String Unlink = @"<i class=""fa fa-lg fa-fw fa-chain-broken text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Unlink""></i>";
            public const String Impersonate = @"<i class=""fa fa-lg fa-fw fa-refresh text-after-icon"" data-toggle=""tooltip"" data-placement=""top"" title=""Impersonate""></i>";

            // GET TEXT
            public static String GetText(Activity a)
            {
                switch (a.Verb)
                {
                    case ActivityVerb.Login:
                    case ActivityVerb.Logout:
                    case ActivityVerb.TryToLogin:
                        return "{0} {1} from {2}".FormatWith(
                            GetActor(a),
                            a.Verb.GetDescriptionOr(),
                            a.IPAddress
                        );
                    case ActivityVerb.TryToLoginInactiveUser:
                        return "{0} {1} from {2} but he/she was rejected because NOT ACTIVE".FormatWith(
                            GetActor(a),
                            a.Verb.GetDescriptionOr(),
                            a.IPAddress
                        );
                    case ActivityVerb.TryToLoginNotEnabledUser:
                        return "{0} {1} from {2} but he/she was rejected because NOT ENABLED FOR THIS APPLICATION".FormatWith(
                            GetActor(a),
                            a.Verb.GetDescriptionOr(),
                            a.IPAddress
                        );
                    case ActivityVerb.Create:
                        return "<a href=\"{0}\">{1}</a> has created a new {2} named {3}".FormatWith(
                            "javascript:void(0);",
                            a.ActorEntityDisplayText,
                            a.ObjectEntityType,
                            GetObject(a)
                        );
                    case ActivityVerb.Update:
                        return "<a href=\"{0}\">{1}</a> has updated {2}".FormatWith(
                            "javascript:void(0);",
                            a.ActorEntityDisplayText,
                            GetObject(a)
                        );
                    case ActivityVerb.View:
                        return "<a href=\"{0}\">{1}</a> had viewed {2}".FormatWith(
                            "javascript:void(0);",
                            a.ActorEntityDisplayText,
                            GetObject(a)
                        );
                    case ActivityVerb.Delete:
                        return "<a href=\"{0}\">{1}</a> has deleted {2}".FormatWith(
                            "javascript:void(0);",
                            a.ActorEntityDisplayText,
                            GetObject(a)
                        );
                    case ActivityVerb.Link:
                        return "{0} linked {1} to {2}".FormatWith(
                            GetActor(a),
                            GetObject(a),
                            GetRelated(a)
                        );
                    case ActivityVerb.Unlink:
                        return "{0} unlinked {1} from {2}".FormatWith(
                            GetActor(a),
                            GetObject(a),
                            GetRelated(a)
                        );
                    case ActivityVerb.CustomActivity:
                        return "{0} {1}".FormatWith(
                            GetActor(a),
                            GetCustom(a)
                        );
                    default:
                        return String.Empty;
                }
            }

            // GET ACTOR
            private static String GetActor(Activity a)
            {
                return "<a href=\"{0}\" class=\"entityTooltip text-primary\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{2}\">{1}</a>".FormatWith(
                    "javascript:void(0);",
                    a.ActorEntityDisplayText,
                    "USER"
                );
            }

            // GET OBJECT
            private static String GetObject(Activity a)
            {
                switch (a.ObjectEntityType.ToLower())
                {
                    default:
                        if (a.Verb == ActivityVerb.Delete)
                            return "the {0} known as <strong>{1}</strong>".FormatWith(a.ObjectEntityType, a.ObjectEntityDisplayText);

                        return " <a class=\"entityTooltip text-primary\" href=\"{0}\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{2}\">{1}</a>".FormatWith(
                            "javascript:void(0);",
                            (a.ObjectEntityDisplayText.IsNullOrWhiteSpace()) ? a.ObjectEntityId.ToTitleCase() : a.ObjectEntityDisplayText,
                            a.ObjectEntityType
                        );
                }
            }

            // GET RELATED
            private static String GetRelated(Activity a)
            {
                switch (a.ObjectEntityType.ToLower())
                {
                    default:
                        if (a.Verb == ActivityVerb.Delete)
                            return "the {0} known as <strong>{1}</strong>".FormatWith(a.RelatedEntityType, a.RelatedEntityDisplayText);

                        return " <a class=\"entityTooltip text-primary\" href=\"{0}\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{2}\">{1}</a>".FormatWith(
                            "javascript:void(0);",
                            (a.RelatedEntityDisplayText.IsNullOrWhiteSpace()) ? a.RelatedEntityId.ToTitleCase() : a.RelatedEntityDisplayText,
                            a.RelatedEntityType
                        );
                }
            }

            // GET CUSTOM
            private static String GetCustom(Activity a)
            {
                if (a.Verb == ActivityVerb.CustomActivity)
                    return a.RelatedEntityDisplayText;

                return "done something strange...";
                
            }
        }
    }
}
