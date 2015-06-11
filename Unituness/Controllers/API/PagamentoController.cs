using Models;
using MongoDB.Bson;
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
    public class PagamentoController : ApiController
    {

        public PagamentoController()
        {
        }



        [HttpPost]
        private HttpResponseMessage NovaTransacao(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var transacao = new TransacaoPagamento();
            transacao.DataTransacao = DateTime.Now;
            transacao.Id = ObjectId.GenerateNewId().ToString();
            transacao.Cartao = user.CartaoCredito;
            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(transacao))
            };
        }



        [HttpPost]
        public bool RealizarTransacao(AjaxBodyData body)
        {
            var transacao = JSONHelper.Desserializar<TransacaoPagamento>(body.arg);


            return true;
        }
    }
}
