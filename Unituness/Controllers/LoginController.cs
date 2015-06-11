using Models;
using MongoDB.Bson;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SysAdmin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {

            if (string.IsNullOrEmpty(form["user"]) || string.IsNullOrEmpty(form["pass"]))
                ViewBag.Mensagem = "Erro ao realizar login";
            else
            {
                var dbAcess = DBAcess.GetInstance();

                var user = dbAcess._repositoryUsuario.GetSingle(q => q.Login == form["user"] && q.Senha == form["pass"]);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Login, true);

                    var token = ObjectId.GenerateNewId().ToString();
                    user.Tokens.Add(token);
                    Session["UserToken"] = token;
                    dbAcess._repositoryUsuario.Update(user);

                    if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                        return Redirect("/Admin");
                    else
                        return Redirect(Request.QueryString["ReturnUrl"]);
                }
                else
                    ViewBag.Mensagem = "Usuario ou senha invalidos.";
            }


            return View();
        }

        public ActionResult Delete()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home");
        }

        public ActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(FormCollection form)
        {
            var dbAcess = DBAcess.GetInstance();

            if (string.IsNullOrEmpty(form["nome"]))
                ViewBag.Mensagem = "Informe seu nome";
            else if (string.IsNullOrEmpty(form["email"]))
                ViewBag.Mensagem = "Informe seu email";
            else if (string.IsNullOrEmpty(form["user"]))
                ViewBag.Mensagem = "Informe um nome de usuario";
            else if (string.IsNullOrEmpty(form["pass"]))
                ViewBag.Mensagem = "Informe uma senha";
            else if (dbAcess._repositoryUsuario.Exists(q => q.Login == form["user"]))
                ViewBag.Mensagem = "Já existe uma conta para este usuario";
            else
            {
                var user = new Usuario()
                {
                    Login = form["user"],
                    Senha = form["pass"],
                };

                dbAcess._repositoryUsuario.Add(user);

                FormsAuthentication.SignOut();

                return Redirect("/Login");
            }

            return View();
        }
    }
}
