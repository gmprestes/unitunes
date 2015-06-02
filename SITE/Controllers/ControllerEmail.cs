using GmpsMvcController;
using GmpsMvcController.Controller;
using SITE.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SITE.Controllers
{
    public class ControllerEmail : GmpsController
    {
        public ControllerEmail()
        {
        }

        [HttpGetMethod]
        public string Enviar(DtoEmail email)
        {
            var mail = new SendMail();
            mail.Subject = email.Assunto;
            mail.MailTo = "aet.tupandi@gmail.com";
            mail.MailFrom = email.Email;
            mail.Body = string.Format("Telefone : {0}<br/>Mensagem : {1}", email.Telefone, email.Mensagem);
            mail.Send();

            return "Email enviado com sucesso. Em breve entraremos em contato.";
        }
    }

    public class DtoEmail
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
    }
}