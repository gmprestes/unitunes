/// <summary>
/// Classe referente comunicação com o banco MongoDb
/// Centraliza todas as instancias do objetos de acesso ao DB
/// Possibilita a instanciação e desinstanciação em tempo de execução de repositorios
/// 
/// Autor: Guilherme Prestes da Silva
/// Data: 20/11/2013
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoRepository;
using MongoDB.Driver;
using System.Configuration;
using Models;

namespace Repository
{
    public class DBAcess
    {
        private MongoUrl MongoUrl;

        #region Construtores

        /// <summary>
        /// Construtor que inicializa todos os repositorios
        /// </summary>
        public DBAcess(MongoUrl mongoUrl)
        {
            this.MongoUrl = mongoUrl;
        }
        #endregion

        #region Mongo Repositorios Getters

        public MongoRepository<DtoPessoa> _repositoryPessoa
        {
            get
            {
                if (this._repositoryPessoaAtributo == null)
                    this._repositoryPessoaAtributo = new MongoRepository<DtoPessoa>(this.MongoUrl);

                return this._repositoryPessoaAtributo;
            }
        }

        public MongoRepository<DtoArquivo> _repositoryArquivo
        {
            get
            {
                if (this._repositoryArquivoAtributo == null)
                    this._repositoryArquivoAtributo = new MongoRepository<DtoArquivo>(this.MongoUrl);


                return this._repositoryArquivoAtributo;
            }
        }

        public MongoRepository<DtoSemestre> _repositorySemestre
        {
            get
            {
                if (this._repositorySemestreAtributo == null)
                    this._repositorySemestreAtributo = new MongoRepository<DtoSemestre>(this.MongoUrl);


                return this._repositorySemestreAtributo;
            }
        }

        public MongoRepository<DtoInstituicao> _repositoryInstituicao
        {
            get
            {
                if (this._repositoryInstituicaoAtributo == null)
                    this._repositoryInstituicaoAtributo = new MongoRepository<DtoInstituicao>(this.MongoUrl);


                return this._repositoryInstituicaoAtributo;
            }
        }

        public MongoRepository<DtoNotasFiscais> _repositoryNotasFiscais
        {
            get
            {
                if (this._repositoryNotasFiscaisAtributo == null)
                    this._repositoryNotasFiscaisAtributo = new MongoRepository<DtoNotasFiscais>(this.MongoUrl);

                return this._repositoryNotasFiscaisAtributo;
            }
        }

        public MongoRepository<DtoAuxilio> _repositoryAuxilio
        {
            get
            {
                if (this._repositoryAuxilioAtributo == null)
                    this._repositoryAuxilioAtributo = new MongoRepository<DtoAuxilio>(this.MongoUrl);


                return this._repositoryAuxilioAtributo;
            }
        }

        #endregion

        #region Mongo Repositorios Atributos

        private MongoRepository<DtoPessoa> _repositoryPessoaAtributo;

        private MongoRepository<DtoArquivo> _repositoryArquivoAtributo;

        private MongoRepository<DtoSemestre> _repositorySemestreAtributo;

        private MongoRepository<DtoInstituicao> _repositoryInstituicaoAtributo;

        private MongoRepository<DtoNotasFiscais> _repositoryNotasFiscaisAtributo;

        private MongoRepository<DtoAuxilio> _repositoryAuxilioAtributo;



        #endregion

    }
}