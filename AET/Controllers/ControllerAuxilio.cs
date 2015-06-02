using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerAuxilio : GmpsController
    {
        private Helpers _helpers;
        public ControllerAuxilio()
        {
            this._helpers = new Helpers();
        }

        [HttpPostMethod]
        public bool Save(DtoAuxilio auxilio)
        {
            this._helpers.dbAcess._repositoryAuxilio.Update(auxilio);

            return true;
        }

        [HttpGetMethod]
        public DtoAuxilio Get(string semestreid)
        {
            var user = Membership.GetUser();
            var auxilio = this._helpers.dbAcess._repositoryAuxilio.GetSingle(q => q.SemestreId == semestreid && q.UserId == user.ProviderUserKey.ToString());

            if (auxilio == null)
            {
                auxilio = new DtoAuxilio();
                auxilio.SemestreId = semestreid;
                auxilio.UserId = user.ProviderUserKey.ToString();

                this._helpers.dbAcess._repositoryAuxilio.Add(auxilio);
            }

            return auxilio;
        }


        [HttpGetMethod]
        public DtoAuxilio GetAssociado(string semestreid, string pessoaid)
        {
            if (this._helpers.IsObjectId(pessoaid))
            {
                var pessoa = this._helpers.dbAcess._repositoryPessoa.GetById(pessoaid);
                var auxilio = this._helpers.dbAcess._repositoryAuxilio.GetSingle(q => q.SemestreId == semestreid && q.UserId == pessoa.UserId);
                if (auxilio == null)
                {
                    auxilio = new DtoAuxilio();
                    auxilio.SemestreId = semestreid;
                    auxilio.UserId = pessoa.UserId;

                    this._helpers.dbAcess._repositoryAuxilio.Add(auxilio);
                }

                return auxilio;
            }
            else
                return new DtoAuxilio();
        }
    }
}