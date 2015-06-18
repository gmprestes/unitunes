using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Unituness.Controllers
{
    public class MidiaController : Controller
    {
        //
        // GET: /Midia/
        public ActionResult Detalhe()
        {
            if (Session["UserToken"] != null)
            {
                ViewBag.UserToken = Session["UserToken"].ToString();

            }


            return View();
        }

        public ActionResult List()
        {
            if (Session["UserToken"] == null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();

                return Redirect("/login?ReturnUrl=/midia/list");
            }


            ViewBag.UserToken = Session["UserToken"].ToString();
            return View();
        }

        public ActionResult Edit()
        {
            if (Session["UserToken"] == null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();

                return Redirect("/login?ReturnUrl=/midia/list");
            }


            ViewBag.UserToken = Session["UserToken"].ToString();
            return View();
        }
    }
}
