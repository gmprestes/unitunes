using Models;
using Repository;
using SysAdmin.API;
using SysAdmin.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Unituness.Controllers.API
{
    public class MidiaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Get(AjaxBodyData body)
        {

            return new HttpResponseMessage();
        }

        [HttpPost]
        public HttpResponseMessage IniciarCompra(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var midia = dbAcess._repositoryMidia.GetSingle(q => q.Id == body.arg);

            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(new object[] { midia, midia.Preco < user.Credito, user.Credito }))
            };
        }


        [HttpPost]
        public HttpResponseMessage GetAll(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));
            if (user == null)
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JSONHelper.Serializar(new List<Midia>()))
                };

            var midias = dbAcess._repositoryMidia.All();
            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(midias))
            };
        }
    }
}
