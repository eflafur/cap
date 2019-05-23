using System;
using System.Net.Mail;
using GruppoCap.Logging;
using GruppoCap.Logging.Base;
using GruppoCap.Mail;
using GruppoCap;

namespace GruppoCap.Core.Mvc.Logging
{

	public class SmtpLogger
		: LoggerBase
	{

		// PRIVATE MEMBERs
		protected Boolean _IncludeLogLevelInMessage = true;
		protected Boolean _IncludeScopeInSubject = true;
		//protected String _SmtpHost = null;
		protected String _FromAddress = null;
		protected String _ToAddress = null;
		protected LogLevel _MaxLogLevelForPriorityLow = LogLevel.Info;
		protected LogLevel _MinLogLevelForPriorityHigh = LogLevel.Error;
		protected IMailSender _MailSender = null;

		#region " CTORs "

		// CTOR
		public SmtpLogger(String smtpHost, String fromAddress, String toAddress)
			: this(new SmtpMailSender(smtpHost), fromAddress, toAddress)
		{
			// EMPTY
		}

		// CTOR
		public SmtpLogger(IMailSender mailSender, String fromAddress, String toAddress)
		{
            // CHECKs
            Ensure.Arg(() => mailSender).IsNotNull();
            Ensure.Arg(() => fromAddress).IsNotNullOrWhiteSpace();
            Ensure.Arg(() => toAddress).IsNotNullOrWhiteSpace();

			_FromAddress = fromAddress;
			_ToAddress = toAddress;

			Subject = null;

			_MailSender = mailSender;
		}

		#endregion

		// GET MAIL PRIORITY
		protected MailPriority GetMailPriority(LogLevel logLevel)
		{
			if (logLevel >= MinLogLevelForPriorityHigh)
				return MailPriority.High;

			if (logLevel <= MaxLogLevelForPriorityLow)
				return MailPriority.Low;

			return MailPriority.Normal;
		}

		// INCLUDE SCOPE IN SUBJECT
		public Boolean IncludeScopeInSubject
		{
			get { return _IncludeScopeInSubject; }
			set { _IncludeScopeInSubject = value; }
		}

		// INCLUDE LOG LEVEL IN MESSAGE
		public Boolean IncludeLogLevelInMessage
		{
			get { return _IncludeLogLevelInMessage; }
			set { _IncludeLogLevelInMessage = value; }
		}

		// MAX LOG LEVEL FOR PRIORITY LOW
		public LogLevel MaxLogLevelForPriorityLow
		{
			get { return _MaxLogLevelForPriorityLow; }
			set { _MaxLogLevelForPriorityLow = value; }
		}

		// MIN LOG LEVEL FOR PRIORITY HIGH
		public LogLevel MinLogLevelForPriorityHigh
		{
			get { return _MinLogLevelForPriorityHigh; }
			set { _MinLogLevelForPriorityHigh = value; }
		}

		// SUBJECT
		public String Subject { get; set; }

    	// APPEND
		public override void Append(String scope, LogLevel logLevel, Exception exceptionOrNull, String message, params Object[] parameters)
		{
			if (_MailSender == null)
				return;

			if (IsLogLevelEnabled(logLevel) == false)
				return;

			String s, m;

			using (MailMessage mail = new MailMessage())
			{
				// FORMAT THE MESSAGE
				//m = String.Format(message, parameters);

                m = message;

				if (IncludeLogLevelInMessage)
				{
					// INCLUDE LOG LEVEL
					m = logLevel.ToString().ToUpperInvariant() + " - " + m;
				}

				// FROM ADDRESS
				mail.From = new MailAddress(_FromAddress);

				// MAIL PRIORITY
				mail.Priority = GetMailPriority(logLevel);

				// TO ADDRESS
				mail.To.Add(_ToAddress ?? String.Empty);

				// SUBJECT
				s = Subject;
				if (String.IsNullOrEmpty(s))
					s = "{message}";

				s = s.Replace("{message}", m);

				mail.Subject = s.Truncate(100, true);

				if (IncludeScopeInSubject)
					mail.Subject = scope + " - " + mail.Subject;

				// BODY
				mail.IsBodyHtml = false;
				mail.Body =
					"MOMENT:"
					+ Environment.NewLine
					+ DateTime.Now.ToString()
					+ Environment.NewLine
					+ Environment.NewLine
					+ "SCOPE:"
					+ Environment.NewLine
					+ scope
					+ Environment.NewLine
					+ Environment.NewLine
                    
					+ "MESSAGE:"
					+ Environment.NewLine
					+ m
				;

                /*
                 
                 * + "OPERATING USER:"
                    + Environment.NewLine
                    + RevoContextHelpers.GetCurrentRevoWebRequest().CurrentUsername
                 * 
                 
                 */



                if (parameters.HasValues())
                {
                    mail.Body =
                                mail.Body
                                + Environment.NewLine
                                + Environment.NewLine
                                + "PARAMETERS:"
                            ;

                    foreach (Object o in parameters)
                    {
                        if (o == null)
                        {
                            mail.Body = mail.Body
                                + Environment.NewLine
                                + "=> {0}".FormatWith("null parameter");
                        }
                        else
                        {
                            mail.Body = mail.Body
                                + Environment.NewLine
                                + "=> {0}".FormatWith(o.ToString());
                        }
                    }
                }

				if (System.Web.HttpContext.Current != null)
				{
					// REFERRER
					try
					{
						Uri referrer;

						referrer = System.Web.HttpContext.Current.Request.UrlReferrer;

						if (referrer != null)
						{
							mail.Body =
								mail.Body
								+ Environment.NewLine
								+ Environment.NewLine
								+ "REFERRER:"
								+ Environment.NewLine
								+ referrer.ToString()
							;
						}
					}
					catch (Exception)
					{
						// EMPTY
					}

					// USER AGENT
					try
					{
						String userAgent;

						userAgent = System.Web.HttpContext.Current.Request.UserAgent;

						if (String.IsNullOrEmpty(userAgent) == false)
						{
							mail.Body =
								mail.Body
								+ Environment.NewLine
								+ Environment.NewLine
								+ "USER AGENT:"
								+ Environment.NewLine
								+ userAgent
							;
						}
					}
					catch (Exception)
					{
						// EMPTY
					}

                    // LOGGED USER
                    try
                    {
                        String loggedUser;

                        loggedUser = RevoContextHelpers.GetCurrentRevoWebRequest().CurrentUsername;

                        if (String.IsNullOrEmpty(loggedUser) == false)
                        {
                            mail.Body =
                                mail.Body
                                + Environment.NewLine
                                + Environment.NewLine
                                + "LOGGED USER:"
                                + Environment.NewLine
                                + loggedUser
                            ;
                        }
                    }
                    catch (Exception)
                    {
                        // EMPTY
                    }
				}

				if (exceptionOrNull != null)
				{
					// INCLUDE THE EXCEPTION IN THE MAIL BODY
					mail.Body =
						mail.Body
						+ Environment.NewLine
						+ Environment.NewLine
						+ "EXCEPTION DETAILs:"
						+ Environment.NewLine
						+ exceptionOrNull.ToString()
					;
				}

				// SEND
				_MailSender.Send(mail, false);
			}
		}

	}

}