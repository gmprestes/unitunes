using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerPessoa : GmpsController
    {
        private Helpers _helpers;
        public ControllerPessoa()
        {
            this._helpers = new Helpers();
        }

        [HttpPostMethod]
        public bool Save(Models.DtoPessoa pessoa)
        {
            this._helpers.dbAcess._repositoryPessoa.Update(pessoa);

            return true;
        }

        [HttpGetMethod]
        public DtoPessoa Get()
        {
            var user = Membership.GetUser();
            if (user != null)
            {
                var pessoa = this._helpers.dbAcess._repositoryPessoa.GetSingle(q => q.UserId == user.ProviderUserKey.ToString());
                if (pessoa == null)
                {
                    pessoa = new DtoPessoa();
                    pessoa.Email = user.UserName.ToLower();
                    pessoa.UserId = user.ProviderUserKey.ToString();
                    pessoa.Codigo = this._helpers.dbAcess._repositoryPessoa.Max("Codigo");
                    this._helpers.dbAcess._repositoryPessoa.Add(pessoa);
                }

                return pessoa;
            }
            else
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("/Login");
                return null;
            }
        }

        [HttpGetMethod]
        public DtoPessoa GetById(string id)
        {
            var pessoa = this._helpers.dbAcess._repositoryPessoa.GetById(id);

            return pessoa;
        }


        [HttpPostMethod]
        public List<DtoPessoa> GetAll(string nome, bool docspendentes, bool auxilionaoconcedido)
        {
            var query = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(nome))
                query.Add(Query.Matches("Nome", string.Format("(?i).*{0}.*", nome)));

            //if (auxilionaoconcedido)
            //    query.Add(Query.EQ();

            if (docspendentes == true)
            {
                var usersIds = this._helpers.dbAcess._repositoryArquivo.All(q => q.Verificado == false).Select(q => q.UserId);

                var ids = new List<BsonValue>();
                foreach (var item in usersIds.Distinct())
                    ids.Add(BsonValue.Create(item));

                //var ids = usersIds.ToList().Select(q => BsonValue.Create(q)).ToList();

                query.Add(Query.In("UserId", ids));
            }

            if (query.Count() > 0)
                return this._helpers.dbAcess._repositoryPessoa.Collection.Find(Query.And(query.ToArray())).Select(q => new DtoPessoa() { Id = q.Id, Codigo = q.Codigo, Nome = q.Nome, Sobrenome = q.Sobrenome, Telefone = q.Telefone, Email = q.Email }).ToList();
            else
                return this._helpers.dbAcess._repositoryPessoa.Collection.FindAll().Select(q => new DtoPessoa() { Id = q.Id, Codigo = q.Codigo, Nome = q.Nome, Sobrenome = q.Sobrenome, Telefone = q.Telefone, Email = q.Email }).ToList();
        }

        [HttpGetMethod]
        public List<DtoSemestre> GetSemestresUser()
        {
            var user = Membership.GetUser();
            if (user != null)
            {
                var ids = this._helpers.dbAcess._repositoryAuxilio.All(q => q.UserId == user.ProviderUserKey.ToString()).Select(q => q.SemestreId).ToList();

                var aux = this._helpers.dbAcess._repositorySemestre.All(q => ids.Contains(q.Id) || q.Ativo).OrderByDescending(q => q.DataInicio).ToList();

                return aux;
            }
            else
            {
                return new List<DtoSemestre>();
            }
        }

        [HttpGetMethod]
        public List<DtoSemestre> GetSemestresAssociado(string id)
        {

            if (this._helpers.IsObjectId(id))
            {

                var pessoa = this._helpers.dbAcess._repositoryPessoa.GetById(id);

                var ids = this._helpers.dbAcess._repositoryAuxilio.All(q => q.UserId == pessoa.UserId).Select(q => q.SemestreId).ToList();

                var aux = this._helpers.dbAcess._repositorySemestre.All(q => ids.Contains(q.Id) || q.Ativo).OrderByDescending(q => q.DataInicio).ToList();

                return aux;
            }
            else
            {
                return new List<DtoSemestre>();
            }
        }
    }
}