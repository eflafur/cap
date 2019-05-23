using System;

namespace GruppoCap.Logging
{

	public interface ILoggerScope
	{

		ILogger Logger { get; }

		String Scope { get; }

		Boolean IsLogLevelEnabled(LogLevel logLevel);

		void Append(LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters);

	}

}