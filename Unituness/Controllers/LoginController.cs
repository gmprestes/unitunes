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
            if (form["user"] == "user" && form["pass"] == "1234")
            {
                FormsAuthentication.SetAuthCookie(form["user"], true);

                if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                    return Redirect("/Admin");
                else
                    return Redirect(Request.QueryString["ReturnUrl"]);
            }
            else
                ViewBag.Mensagem = "Erro ao realizar login";

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
            if (string.IsNullOrEmpty(form["nome"]))
                ViewBag.Mensagem = "Informe seu nome";
            else if (string.IsNullOrEmpty(form["email"]))
                ViewBag.Mensagem = "Informe seu email";
            else if (string.IsNullOrEmpty(form["user"]))
                ViewBag.Mensagem = "Informe um nome de usuario";
            else if (string.IsNullOrEmpty(form["pass"]))
                ViewBag.Mensagem = "Informe uma senha";
            else
                return Redirect("/Login");

            return View();
        }
    }
}
