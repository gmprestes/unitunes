using AET.Code;
using GmpsMvcController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace AET
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            RouteTable.Routes.Add(new Route("request/{controller}/{action}/{*index}", new GmpsMvcRouteHandler()));
            RouteTable.Routes.Add(new Route("ajaxarquivos/{metodo}", new HttpHandlerRoute("~/Ajax/uploadarquivos.ashx")));

            routes.MapPageRoute("login", "login", "~/Login.aspx");
            routes.MapPageRoute("cadastro", "cadastro", "~/Cadastro.aspx");

            routes.MapPageRoute("meuperfil", "meuperfil", "~/Default.aspx");
            routes.MapPageRoute("solicitacaoauxilio", "auxilio", "~/Aulixios.aspx");
            routes.MapPageRoute("alterarsenha", "alterarsenha", "~/AlterarSenha.aspx");


            // CADASTRO SEMESTRES
            routes.MapPageRoute("semestresediacao", "semestres/edit/{*id}", "~/Restricted/Admin/View/Semestres/Edit.aspx");
            routes.MapPageRoute("semestreslista", "semestres/list", "~/Restricted/Admin/View/Semestres/List.aspx");

            // CADASTRO INSTITUICOES
            routes.MapPageRoute("instituicoesediacao", "instituicoes/edit/{*id}", "~/Restricted/Admin/View/InstituicoesEnsino/Edit.aspx");
            routes.MapPageRoute("instituicoeslista", "instituicoes/list", "~/Restricted/Admin/View/InstituicoesEnsino/List.aspx");


            // CADASTRO ASSOCIADOS
            routes.MapPageRoute("associadosedicao", "associado/edit/{*id}", "~/Restricted/Admin/View/Associados/Edit.aspx");
            routes.MapPageRoute("associadoslista", "associado/list", "~/Restricted/Admin/View/Associados/List.aspx");

            // Relatorios
            routes.MapPageRoute("relatorios", "relatorios", "~/Restricted/Admin/View/relatorios/Edit.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}