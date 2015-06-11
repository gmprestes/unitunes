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

        public MongoRepository<DtoArquivo> _repositoryArquivo
        {
            get
            {
                if (this._repositoryArquivoAtributo == null)
                    this._repositoryArquivoAtributo = new MongoRepository<DtoArquivo>(this.MongoUrl);


                return this._repositoryArquivoAtributo;
            }
        }

        #endregion

        #region Mongo Repositorios Atributos

        private MongoRepository<DtoArquivo> _repositoryArquivoAtributo;

        #endregion

    }
}