using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Mail
{
    public interface IMailSender
    {
        void Send(MailMessage message, Boolean autoDisposeMessage);
    }
}
