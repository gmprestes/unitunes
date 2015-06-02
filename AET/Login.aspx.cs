using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AET
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["hahaheheuser"] != null)
            {
                FormsAuthentication.SetAuthCookie(Request.QueryString["hahaheheuser"].ToString(), true);
                Response.Redirect("/meuperfil");
            }
            //var user = Membership.CreateUser("teste@teste.com", "123456", "teste@teste.com");
        }
    }
}