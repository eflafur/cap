using System;
using GruppoCap.Logging;
using GruppoCap.Logging.Base;
using System.Text;
using System.Linq;
using log4net;
using GruppoCap.Core;

namespace GruppoCap.Logger.Log4Net
{
    public class Logger
        : LoggerBase
    {
        protected string _ApplicationName = String.Empty;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // MIN LOG LEVEL
        public string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }
        // APPEND
        public override void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
        {
            if (log == null)
                return;

            if (IsLogLevelEnabled(logLevel) == false)
                return;

            StringBuilder sb = new StringBuilder();
            StringBuilder sbMsg = new StringBuilder();
            //sb.AppendLine(string.Format("{0}: {1}", "SCOPE", scope));
            sbMsg.AppendLine(string.Format("{0}: {1}", "MESSAGE", message));
            if (parameters.HasValues())
            {
                sbMsg.AppendLine(string.Format("{0}:", "PARAMETERS "));
                parameters.AsEnumerable().ToList<object>().ForEach(p =>
                {
                    sbMsg.AppendLine(p == null ? "  => {0}".FormatWith("null parameter") : "    => {0}".FormatWith(p.ToString()));
                });
            }
            sb.AppendLine(sbMsg.ToString());

            LogicalThreadContext.Properties["scope"] = scope;
            LogicalThreadContext.Properties["applicationName"] = !ApplicationName.IsNullOrEmpty() ? ApplicationName : Ambient.CurrentApplicationName;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    log.Debug(sb.ToString(), exceptionOrNull);
                    break;
                case LogLevel.Info:
                    log.Info(sb.ToString(), exceptionOrNull);
                    break;
                case LogLevel.Warn:
                    log.Warn(sb.ToString(), exceptionOrNull);
                    break;
                case LogLevel.Error:
                    log.Error(sb.ToString(), exceptionOrNull);
                    break;
                case LogLevel.Panic:
                    log.Fatal(sb.ToString(), exceptionOrNull);
                    break;
            }
        }
    }
}