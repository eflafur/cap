using System;
using System.Diagnostics;
using GruppoCap.Logging.Base;

namespace GruppoCap.Logging.Common
{

	public class EventLogger
		: LoggerBase
	{

		#region " CTORs "

		// CTOR
		public EventLogger(String source)
		{
			Source = source;
		}

		#endregion

		public String Source { get; set; }

		// GET ENTRY TYPE FROM LOG LEVEL
		public EventLogEntryType GetEntryTypeFromLogLevel(LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Trace:
					return EventLogEntryType.Information;
				case LogLevel.Debug:
					return EventLogEntryType.Information;
				case LogLevel.Info:
					return EventLogEntryType.Information;
				case LogLevel.Warn:
					return EventLogEntryType.Warning;
				case LogLevel.Error:
					return EventLogEntryType.Error;
				case LogLevel.Panic:
					return EventLogEntryType.Error;
				default:
					return EventLogEntryType.Error;
			}
		}

		// APPEND
		public override void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			EventLog elog;

			elog = new EventLog();

			// SET SOURCE
			if (EventLog.SourceExists(Source) == false)
			{
				EventLog.CreateEventSource(Source, "Application");
			}
			elog.Source = Source;

			// RAISING EVENTs
			elog.EnableRaisingEvents = true;

			// WRITE THE ENTRY
			elog.WriteEntry(message, GetEntryTypeFromLogLevel(logLevel));
		}

	}

}