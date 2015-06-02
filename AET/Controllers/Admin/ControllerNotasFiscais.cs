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
    public class ControllerNotasFiscais : GmpsController
    {
        private Helpers _helpers;
        public ControllerNotasFiscais()
        {
            this._helpers = new Helpers();
        }

        [HttpPostMethod]
        public List<DtoNotasFiscais> Get(string mes, string ano)
        {
            return this._helpers.dbAcess._repositoryNotasFiscais.All(q => q.Ano == ano && q.Mes == mes).ToList();
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
        public string Excluir(string id)
        {
            try
            {
                this._helpers.dbAcess._repositoryNotasFiscais.Delete(id);
            }
            catch (Exception ex)
            {
                return "ERRO";
            }

            return string.Empty;
        }
    }
}