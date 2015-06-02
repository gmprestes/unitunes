using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerCadastro : GmpsController
    {

        private Helpers _helpers;
        public ControllerCadastro()
        {
            this._helpers = new Helpers();
        }

        [HttpPostMethod]
        public object[] EfetuaCadastro(string usuario, string cpf,string pass)
        {
            try
            {
                cpf = Utils.CompleteLength(cpf, "0", 11, true);

                if (!this._helpers.CPFValido(cpf))
                    return new object[] { false, "O CPF informado é invalido." };
                else if (!this._helpers.EmailValido(usuario))
                    return new object[] { false, "O EMAIL informado é invalido." };
                else
                {

                    var pessoa = this._helpers.dbAcess._repositoryPessoa.GetSingle(q => q.CPF == cpf);
                    if (pessoa != null)
                    {
                        var user = Membership.GetUser(pessoa.UserId);
                        return new object[] { false, string.Format("Já existe uma pessoa cadastrada para este CPF sob o email {0}. Em caso de duvidas entre em contato com a diretoria da AET.", user.Email) };
                    }
                    else
                    {
                        var user = Membership.GetUser(usuario);
                        if (user != null)
                        {
                            pessoa = this._helpers.dbAcess._repositoryPessoa.GetSingle(q => q.UserId == user.ProviderUserKey.ToString());
                            return new object[] { false, string.Format("Já existe uma pessoa cadastrada para este email sob o CPF {0}. Em caso de duvidas entre em contato com a diretoria da AET.", cpf) };
                        }

                        //var senha = Guid.NewGuid().ToString().Substring(0, 6);

                        user = Membership.CreateUser(usuario, pass, usuario);

                        this._helpers.dbAcess._repositoryPessoa.Add(new DtoPessoa()
                        {
                            Nome = usuario,
                            Email = usuario,
                            CPF = cpf,
                            UserId = user.ProviderUserKey.ToString(),
                            Codigo = this._helpers.dbAcess._repositoryPessoa.Max("Codigo")
                        });

                        var mail = new SendMail();

                        mail.Body = mail.TemplateCadastro(usuario, pass);
                        mail.MailTo = usuario;
                        mail.MailCC = new List<string> { "cadastro@aettupandi.com.br" };
                        mail.Subject = "Cadastro ao Sistema da AET";
                        mail.MailFrom = "cadastro@aettupandi.com.br";
                        mail.Send();

                        return new object[] { true, "Usuario criado com sucesso. Você receberá um email com a confirmação desta operação. Utilize a pagina de login para entrar no sistema." };
                    }
                }
            }
            catch
            {
                return new object[] { false, "Não foi possivel criar este usuario. Tente novamente em alguns instantes." };
            }
        }
    }
}