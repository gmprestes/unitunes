using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AET
{
    public partial class page : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var user = Membership.GetUser("djonatan20101@hotmail.com");
            //var pass = user.ResetPassword();
            //user.ChangePassword(pass, "123456");

            //Roles.AddUserToRole("teste@teste.com", "admin");
        }
    }
}