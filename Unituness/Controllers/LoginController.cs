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
                    return Redirect("/Home");
                else
                    return Redirect(Request.QueryString["ReturnUrl"]);
            }
            else
                ViewBag.Mensagem = "Erro ao realizar login";

            return View();
        }

        public ActionResult NewUser()
        {
            return View();
        }
    }
}
