using Models;
using MongoDB.Bson;
using Repository;
using SiteMVC.Helpers;
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

        [HttpPost]
        public HttpResponseMessage NovaTransacao(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var transacao = new TransacaoPagamento();
            transacao.DataTransacao = DateTime.Now;
            transacao.Id = ObjectId.GenerateNewId().ToString();
            transacao.UsuarioId = user.Id;
            transacao.ValorTransacao = 5;
            transacao.Cartao = user.CartaoCredito;
            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(transacao))
            };
        }

        [HttpPost]
        public HttpResponseMessage Get(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var transacao = dbAcess._repositoryTransacaoPagamento.GetSingle(q => q.Id == body.arg);

            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(transacao))
            };
        }

        [HttpPost]
        public HttpResponseMessage RealizarTransacao(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var transacao = JSONHelper.Desserializar<TransacaoPagamento>(body.arg);

            var cielo = new TransacaoCartao();
            var info = cielo.GetTemplateTransacao(transacao);
            transacao.TID = cielo.RealizaTransacao(info);
            if (transacao.TID.Length > 2)
            {
                dbAcess._repositoryTransacaoPagamento.Add(transacao);

                user.CartaoCredito = transacao.Cartao;
                user.Credito += transacao.ValorTransacao;
                dbAcess._repositoryUsuario.Update(user);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JSONHelper.Serializar(new object[] { true, transacao.Id }))
                };
            }
            else
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JSONHelper.Serializar(new object[] { false }))
                };


        }
    }
}
