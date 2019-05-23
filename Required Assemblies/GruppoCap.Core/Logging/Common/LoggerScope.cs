using System;

namespace GruppoCap.Logging.Common
{

	public class LoggerScope
		: ILoggerScope
	{

		#region " CTORs "

		// CTOR
		public LoggerScope(ILogger logger, String scope)
		{
			Logger = logger;
			Scope = scope;
		}

		#endregion

		#region ILoggerScope Members

		// LOGGER
		public ILogger Logger { get; private set; }

		// SCOPE
		public String Scope { get; private set; }

		// IS LOG LEVEL ENABLED
		public Boolean IsLogLevelEnabled(LogLevel logLevel)
		{
			return Logger.IsLogLevelEnabled(logLevel);
		}

		// APPEND
		public void Append(LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			Logger.Append(Scope, logLevel, exceptionOrNull, message, parameters);
		}

		#endregion

	}

}