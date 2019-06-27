using System;

namespace GruppoCap.Logging.Common
{

	public class DummyLogger
		: ILogger
	{

		// PRIVATE MEMBERs
		private static ILogger _SharedDefaultLogger = null;

		#region " CTORs "

		// CTOR
		public DummyLogger()
		{
			// EMPTY
		}

		#endregion

		#region ILogger Members

		public Boolean IsLogLevelEnabled(LogLevel logLevel)
		{
			return false;
		}

		public void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			// EMPTY
		}

		#endregion

		// SHARED DEFAULT LOGGER
		public static ILogger SharedDefaultLogger
		{
			get
			{
				if (_SharedDefaultLogger == null)
					_SharedDefaultLogger = new DummyLogger();

				return _SharedDefaultLogger;
			}
		}

	}

}