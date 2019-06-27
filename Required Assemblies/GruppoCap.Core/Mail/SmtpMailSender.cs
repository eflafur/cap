using System;
using System.Net;
using System.Net.Mail;

namespace GruppoCap.Mail
{
    public class SmtpMailSender : IMailSender
    {
        // CTOR
        public SmtpMailSender(String host, Int32? port = null)
        {
            Host = host;
            Port = port.GetValueOrDefault(25); // 25 IS THE DEFAULT PORT FOR SMTP PROTOCOL
        }

        public String Host { get; set; }
        public Int32 Port { get; set; }

        public String Username { get; set; }
        public String Password { get; set; }

        public Boolean EnableSsl { get; set; }

        // GET SMTP CLIENT
        public SmtpClient GetSmtpClient()
        {
            SmtpClient client;

            client = new SmtpClient(Host, Port);

            if (Username.IsNullOrWhiteSpace() == false)
            {
                // USERNAME + PASSWORD SPECIFIED -> USE THEM
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Username, Password);
            }

            client.EnableSsl = EnableSsl;

            return client;
        }

        // SEND
        public void Send(MailMessage message, Boolean autoDisposeMessage)
        {
            using (var client = GetSmtpClient())
            {
                client.Send(message);
            }

            if (autoDisposeMessage)
            {
                message.Dispose();
                message = null;
            }
        }
    }
}
