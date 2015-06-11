using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Unituness.Controllers
{
    [Authorize]
    public class PagamentoController : Controller
    {
        //
        // GET: /Pagamento/

        public ActionResult Index()
        {
            return View();
        }

    }
}
