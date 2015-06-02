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
    public class ControllerSemestre : GmpsController
    {
        private Helpers _helpers;
        public ControllerSemestre()
        {
            this._helpers = new Helpers();
        }

        [HttpGetMethod]
        public DtoSemestre Get(string id)
        {
            if (this._helpers.IsObjectId(id))
                return this._helpers.dbAcess._repositorySemestre.GetSingle(q => q.Id == id);
            else
                return new DtoSemestre() { DataInicio = DateTime.Now, DataTermino = DateTime.Now.AddMonths(6) };
        }

        [HttpPostMethod]
        public string Save(DtoSemestre item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    this._helpers.dbAcess._repositorySemestre.Add(item);
                    return item.Id;
                }
                else
                {
                    this._helpers.dbAcess._repositorySemestre.Update(item);
                    return item.Id;
                }
            }
            catch
            {
                return "ERRO";
            }
        }


        [HttpGetMethod]
        public List<DtoSemestre> GetAllItens()
        {
            return this._helpers.dbAcess._repositorySemestre.All().ToList();
        }

        [HttpGetMethod]
        public string Excluir(string id)
        {
            try
            {
                this._helpers.dbAcess._repositorySemestre.Delete(id);
            }
            catch (Exception ex)
            {
                return "ERRO";
            }

            return string.Empty;

        }
    }
}