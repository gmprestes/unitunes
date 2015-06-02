using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for SendMail
/// </summary>
/// 
namespace SITE.Code
{
    public class SendMail
    {

        public string MailFrom
        {
            get;
            set;
        }

        public string MailTo
        {
            get;
            set;
        }

        public List<string> MailCC
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public List<Attachment> Anexos
        {
            get;
            set;
        }

        public SendMail()
        {
            this.MailCC = new List<string>();
            this.Anexos = new List<Attachment>();
        }

        public bool Send()
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(this.MailTo);

                mail.From = new MailAddress(this.MailFrom);

                mail.Subject = this.Subject;
                mail.IsBodyHtml = true;

                mail.Body = this.Body;

                SmtpClient smtp = new SmtpClient();
                NetworkCredential credentials = new NetworkCredential("sendmail@6nti.com.br", "dY7WaXi");

                foreach (string item in this.MailCC)
                    mail.CC.Add(item);

                foreach (Attachment anexo in Anexos)
                    mail.Attachments.Add(anexo);


                smtp.Host = "6nti.com.br";
                smtp.UseDefaultCredentials = false;
                smtp.Port = 25;
                smtp.Credentials = credentials;
                smtp.Send(mail);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}