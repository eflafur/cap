using System;

namespace GruppoCap.Logging
{

	public interface ILogger
	{

		Boolean IsLogLevelEnabled(LogLevel logLevel);

		void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters);

	}

}