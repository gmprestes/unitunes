﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Unituness.Controllers
{
    [Authorize]
    public class PagamentoController : Controller
    {
        //
        // GET: /Pagamento/

        public ActionResult Index()
        {
            if (Session["UserToken"] == null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();

                return Redirect("/login?ReturnUrl=/pagamento");
            }


            ViewBag.UserToken = Session["UserToken"].ToString();

            return View();
        }

        public ActionResult Recibo()
        {
            return View();
        }

        public ActionResult Recibo_Midia()
        {
            return View();
        }

    }
}
