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
    public class ControllerArquivo : GmpsController
    {
        private Helpers _helpers;
        public ControllerArquivo()
        {
            this._helpers = new Helpers();
        }

        [HttpGetMethod]
        public List<DtoArquivo> GetAllFilesPerfil()
        {
            var user = Membership.GetUser();
            if (user != null)
            {
                var itens = this._helpers.dbAcess._repositoryArquivo.All(q => q.UserId == user.ProviderUserKey.ToString() &&
                    (q.TipoArquivo == TipoArquivo.ComprovanteIdentidadePerfil.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteEnderecoPerfil.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteTituloEleitor.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteCPF.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteCertidaoNascimento.ToString()));


                return itens.Select(q => new DtoArquivo { Id = q.Id, Nome = q.Nome, Tamanho = q.Tamanho, TipoArquivo = q.TipoArquivo, DataUpload = q.DataUpload, Verificado = q.Verificado, Aceito = q.Aceito }).OrderByDescending(q => q.DataUpload).ToList();
            }
            else
                return new List<DtoArquivo>();
        }

        [HttpGetMethod]
        public List<DtoArquivo> GetAllFilesSemestre(string id)
        {
            var user = Membership.GetUser();
            if (user != null && this._helpers.IsObjectId(id))
            {
                var itens = this._helpers.dbAcess._repositoryArquivo.All(q => q.UserId == user.ProviderUserKey.ToString() && q.FKId == id && (q.TipoArquivo == TipoArquivo.ComprovanteMatriculaSemestre.ToString() || q.TipoArquivo == TipoArquivo.ComprovanteNotasUltimoSemestre.ToString()));
                return itens.Select(q => new DtoArquivo { Id = q.Id, Nome = q.Nome, Tamanho = q.Tamanho, TipoArquivo = q.TipoArquivo, DataUpload = q.DataUpload, Verificado = q.Verificado, Aceito = q.Aceito }).OrderByDescending(q => q.DataUpload).ToList();
            }
            else
                return new List<DtoArquivo>();
        }


        [HttpGetMethod]
        public List<DtoArquivo> GetAllFilesSemestreAssociado(string id, string pessoaid)
        {
            if (this._helpers.IsObjectId(pessoaid) && this._helpers.IsObjectId(id))
            {
                var pessoa = this._helpers.dbAcess._repositoryPessoa.GetById(pessoaid);

                var itens = this._helpers.dbAcess._repositoryArquivo.All(q => q.UserId == pessoa.UserId && q.FKId == id && (q.TipoArquivo == TipoArquivo.ComprovanteMatriculaSemestre.ToString() || q.TipoArquivo == TipoArquivo.ComprovanteNotasUltimoSemestre.ToString()));

                return itens.Select(q => new DtoArquivo { Id = q.Id, Nome = q.Nome, Tamanho = q.Tamanho, TipoArquivo = q.TipoArquivo, DataUpload = q.DataUpload, Verificado = q.Verificado, Aceito = q.Aceito }).OrderByDescending(q => q.DataUpload).ToList();
            }
            else
                return new List<DtoArquivo>();
        }


        [HttpGetMethod]
        public List<DtoArquivo> GetAllFilesPessoa(string id)
        {
            if (this._helpers.IsObjectId(id))
            {
                var pessoa = this._helpers.dbAcess._repositoryPessoa.GetById(id);

                var itens = this._helpers.dbAcess._repositoryArquivo.All(q => q.UserId == pessoa.UserId &&
               (q.TipoArquivo == TipoArquivo.ComprovanteIdentidadePerfil.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteEnderecoPerfil.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteTituloEleitor.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteCPF.ToString() ||
                    q.TipoArquivo == TipoArquivo.ComprovanteCertidaoNascimento.ToString()));

                return itens.Select(q => new DtoArquivo { Id = q.Id, Nome = q.Nome, Tamanho = q.Tamanho, TipoArquivo = q.TipoArquivo, DataUpload = q.DataUpload, Verificado = q.Verificado, Aceito = q.Aceito }).OrderByDescending(q => q.DataUpload).ToList();
            }
            else
                return new List<DtoArquivo>();
        }


        [HttpPostMethod]
        public bool SaveArquivo(DtoArquivo arquivo)
        {
            bool retorno = false;
            if (arquivo != null && this._helpers.IsObjectId(arquivo.Id))
            {
                var file = this._helpers.dbAcess._repositoryArquivo.GetById(arquivo.Id);
                if (file != null)
                {
                    file.Aceito = arquivo.Aceito;

                    if (file.Aceito)
                        file.Verificado = true;
                    else
                        file.Verificado = arquivo.Verificado;

                    this._helpers.dbAcess._repositoryArquivo.Update(file);

                    retorno = true;
                }
            }

            return retorno;
        }
    }
}