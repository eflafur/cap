using System;
using System.Web;
using KGroup.Logging;
using KGroup.Logging.Base;

namespace KGroup.Web.Logging
{

	public class AspNetTraceLogger
		: LoggerBase
	{

		// PRIVATE MEMBERs
		protected Boolean _IncludeLogLevelInMessage = true;

		#region " CTORs "

		// CTOR
		public AspNetTraceLogger()
		{
			// EMPTY
		}

		// CTOR
		public AspNetTraceLogger(LogLevel minLogLevel)
			: this()
		{
			_MinLogLevel = minLogLevel;
		}

		#endregion

		// CURRENT TRACE CONTEXT
		private TraceContext CurrentTraceContext
		{
			get { return System.Web.HttpContext.Current.Trace; }
		}

		// INCLUDE LOG LEVEL IN MESSAGE
		public Boolean IncludeLogLevelInMessage
		{
			get { return _IncludeLogLevelInMessage; }
			set { _IncludeLogLevelInMessage = value; }
		}

		// APPEND
		public override void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			TraceContext _ctx = null;

			// GET THE CURRENT TRACE CONTEXT
			_ctx = CurrentTraceContext;

			// TRACE CONTEXT CHECK
			if (_ctx == null)
				return;

			// CHECK - ASP.NET TRACING ENABLED
			if (_ctx.IsEnabled == false)
				return;

			// CHECK - LOG LEVEL ENABLED
			if (IsLogLevelEnabled(logLevel) == false)
				return;

			Action<String, String, Exception> traceWritingAction;

			// GET THE CORRECT ACTION
			if (logLevel >= LogLevel.Warn)
			{
				traceWritingAction = _ctx.Warn;
			}
			else
			{
				traceWritingAction = _ctx.Write;
			}

			String m;

			// FORMAT THE MESSAGE
			m = String.Format(message, parameters);

			if (IncludeLogLevelInMessage)
			{
				// INCLUDE LOG LEVEL
				m = logLevel.ToString().ToUpperInvariant() + " - " + m;
			}

			// WRITE IT !
			traceWritingAction(scope, m, exceptionOrNull);
		}

	}

}