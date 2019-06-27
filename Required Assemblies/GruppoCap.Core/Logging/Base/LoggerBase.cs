using System;

namespace GruppoCap.Logging.Base
{

	public abstract class LoggerBase
		: ILogger
	{

		// PRIVATE MEMBERs
		protected LogLevel _MinLogLevel = LogLevel.Warn;

		#region ILogger Members

		// IS LOG LEVEL ENABLED
		public virtual Boolean IsLogLevelEnabled(LogLevel logLevel)
		{
			return _MinLogLevel <= logLevel;
		}

		// APPEND
		public abstract void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters);

		#endregion

		// MIN LOG LEVEL
		public LogLevel MinLogLevel
		{
			get { return _MinLogLevel; }
			set { _MinLogLevel = value; }
		}

	}

}