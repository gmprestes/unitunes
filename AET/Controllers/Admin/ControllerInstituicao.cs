using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerInstituicao : GmpsController
    {
        private Helpers _helpers;
        public ControllerInstituicao()
        {
            this._helpers = new Helpers();
        }

        [HttpGetMethod]
        public DtoInstituicao Get(string id)
        {
            if (this._helpers.IsObjectId(id))
                return this._helpers.dbAcess._repositoryInstituicao.GetSingle(q => q.Id == id);
            else
                return new DtoInstituicao();
        }

        [HttpPostMethod]
        public string Save(DtoInstituicao item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    this._helpers.dbAcess._repositoryInstituicao.Add(item);
                    return item.Id;
                }
                else
                {
                    this._helpers.dbAcess._repositoryInstituicao.Update(item);
                    return item.Id;
                }
            }
            catch
            {
                return "ERRO";
            }
        }


        [HttpGetMethod]
        public List<DtoInstituicao> GetAllItens()
        {
            return this._helpers.dbAcess._repositoryInstituicao.All().ToList();
        }

        [HttpGetMethod]
        public List<DtoInstituicao> GetAllItensAtivos()
        {
            return this._helpers.dbAcess._repositoryInstituicao.All(q => q.Ativo).ToList();
        }

        [HttpGetMethod]
        public string Excluir(string id)
        {
            try
            {
                this._helpers.dbAcess._repositoryInstituicao.Delete(id);
            }
            catch (Exception ex)
            {
                return "ERRO";
            }

            return string.Empty;

        }
    }
}