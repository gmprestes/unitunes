using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerLogin : GmpsController
    {

        [HttpPostMethod]
        public bool Auth(string usuario, string senha)
        {
            bool authOk = false;
            try
            {
                if (Membership.ValidateUser(usuario, senha))
                {
                    FormsAuthentication.SetAuthCookie(usuario, true);
                    authOk = true;
                }
            }
            catch
            {
            }
            return authOk;
        }

        [HttpPostMethod]
        public List<object> AlterarSenha(string atual, string nova)
        {
            var retorno = new List<object>();
            try
            {
                var user = Membership.GetUser();

                if (Membership.ValidateUser(user.Email, atual))
                {
                    var senha = user.ResetPassword();
                    user.ChangePassword(senha, nova);

                    //FormsAuthentication.SignOut();

                    retorno.Add(true);
                    retorno.Add(user.Email);
                }
                else
                    retorno.Add(false);
            }
            catch
            {
            }

            return retorno;
        }

        [HttpGetPostMethod]
        public List<object> AlterarSenha2(string nova)
        {
            var retorno = new List<object>();
            try
            {
                var user = Membership.GetUser();

                var senha = user.ResetPassword();
                user.ChangePassword(senha, nova);

                retorno.Add(true);
                retorno.Add(user.Email);
            }
            catch
            {
            }

            return retorno;
        }


        [HttpGetMethod]
        public void Logout()
        {
            try
            {
                //HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
            }
            catch
            {
            }
        }

        [HttpGetMethod]
        public string GetCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        [HttpGetMethod]
        public bool CurrentUserIsAdmin()
        {
            var isAdmin = false;

            var user = Membership.GetUser();

            if (!string.IsNullOrEmpty(user.Comment) && user.Comment == "ADMIN")
                isAdmin = true;

            return isAdmin;
        }

        [HttpGetMethod]
        public string GetNomePessoaCurrentUser()
        {
            var user = Membership.GetUser();
            var _helpers = new Helpers();

            var pessoa = _helpers.dbAcess._repositoryPessoa.GetSingle(q => q.UserId == user.ProviderUserKey.ToString());

            if (pessoa != null)
                return pessoa.Nome;
            else
                return user.ProviderName;

        }
    }
}