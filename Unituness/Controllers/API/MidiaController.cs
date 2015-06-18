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
    public class MidiaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Get(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            if (user == null)
                return new HttpResponseMessage();

            var midia = dbAcess._repositoryMidia.GetSingle(q => q.Id == body.arg);

            if (midia == null)
                midia = new Midia()
                {
                    AutorId = user.Id,
                    Preco = 0,
                    Id = ObjectId.GenerateNewId().ToString()
                };

            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(midia))
            };
        }

        [HttpPost]
        public HttpResponseMessage Delete(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            if (user == null)
                return new HttpResponseMessage();

            dbAcess._repositoryMidia.Delete(q => q.Id == body.arg);

            return new HttpResponseMessage()
            {
                Content = new StringContent("OK")
            };
        }

        [HttpPost]
        public HttpResponseMessage Save(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            if (user == null)
                return new HttpResponseMessage();

            var midia = JSONHelper.Desserializar<Midia>(body.arg);
            midia.AutorId = user.Id;
            var existe = dbAcess._repositoryMidia.Exists(q => q.Id == midia.Id);
            if (existe)
                dbAcess._repositoryMidia.Update(midia);
            else
                dbAcess._repositoryMidia.Add(midia);

            return new HttpResponseMessage()
            {
                Content = new StringContent(midia.Id)
            };
        }

        [HttpPost]
        public HttpResponseMessage IniciarCompra(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));

            var midia = dbAcess._repositoryMidia.GetSingle(q => q.Id == body.arg);

            var credito = 0d;
            var podeComprar = false;
            if (user != null)
            {
                credito = user.Credito;
                podeComprar = midia.PrecoVenda <= user.Credito;
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(new object[] { midia, podeComprar, credito }))
            };
        }

        [HttpPost]
        public HttpResponseMessage Comprar(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();
            var user = dbAcess._repositoryUsuario.GetSingle(q => q.Tokens.Contains(body.token));
            if (user == null)
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Você precisa estar logoado para comprar")
                };


            var midia = dbAcess._repositoryMidia.GetSingle(q => q.Id == body.arg);

            if (midia.PrecoVenda > user.Credito)
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Você não possui créditos suficientes para esta compra")
                };

            var venda = new Venda()
            {
                MidiaId = midia.Id,
                ValorAutor = midia.Preco,
                ValorVenda = midia.PrecoVenda,
                TaxaUnitunes = midia.TaxaUnitunes,
                DataVenda = DateTime.Now
            };

            dbAcess._repositoryVenda.Add(venda);

            user.Credito -= midia.PrecoVenda;
            dbAcess._repositoryUsuario.Update(user);

            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(new object[] { true, venda.Id }))
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

            var midias = dbAcess._repositoryMidia.All(q => q.AutorId == user.Id);
            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(midias))
            };
        }

        [HttpPost]
        public HttpResponseMessage All(AjaxBodyData body)
        {
            var dbAcess = DBAcess.GetInstance();

            var midias = dbAcess._repositoryMidia.All();
            return new HttpResponseMessage()
            {
                Content = new StringContent(JSONHelper.Serializar(midias))
            };
        }
    }
}
