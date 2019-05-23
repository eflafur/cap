using System;
using System.Collections.Generic;

namespace GruppoCap.Logging.Common
{

	public class MultiLogger
		: ILogger
	{

		protected IList<ILogger> _Loggers = null;

		#region " CTORs "

		// CTOR
		public MultiLogger(IList<ILogger> loggers)
		{
			_Loggers = loggers;
		}

		#endregion

		// LOGGERs
		public IList<ILogger> Loggers
		{
			get { return _Loggers; }
		}

		#region ILogger Members

		// IS LOG LEVEL ENABLED
		public Boolean IsLogLevelEnabled(LogLevel logLevel)
		{
			return true;
		}

		// APPEND
		public void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			foreach (ILogger logger in _Loggers)
			{
				try
				{
					logger.Append(scope, logLevel, exceptionOrNull, message, parameters);
				}
				catch (Exception exc)
				{
					exc = exc = null;
					// EMPTY
				}
			}
		}

		#endregion

	}

}