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

        private static DBAcess _instance;

        #region Construtores

        /// <summary>
        /// Construtor que inicializa todos os repositorios
        /// </summary>
        private DBAcess()
        {
            this.MongoUrl = new MongoUrl(string.Format("mongodb://{0}:{1}@{2}:27017/{3}", "admin", "123456", "192.168.2.99", "unitunes"));
        }

        public static DBAcess GetInstance()
        {
            if(_instance == null)
                _instance = new DBAcess();

            return _instance;
        }

        #endregion

        #region Mongo Repositorios Getters

        public MongoRepository<Usuario> _repositoryUsuario
        {
            get
            {
                if (this._repositoryUsuarioAtributo == null)
                    this._repositoryUsuarioAtributo = new MongoRepository<Usuario>(this.MongoUrl);


                return this._repositoryUsuarioAtributo;
            }
        }

        public MongoRepository<Midia> _repositoryMidia
        {
            get
            {
                if (this._repositoryMidiaAtributo == null)
                    this._repositoryMidiaAtributo = new MongoRepository<Midia>(this.MongoUrl);


                return this._repositoryMidiaAtributo;
            }
        }

        public MongoRepository<TransacaoPagamento> _repositoryTransacaoPagamento
        {
            get
            {
                if (this._repositoryTransacaoPagamentoAtributo == null)
                    this._repositoryTransacaoPagamentoAtributo = new MongoRepository<TransacaoPagamento>(this.MongoUrl);


                return this._repositoryTransacaoPagamentoAtributo;
            }
        }

        #endregion

        #region Mongo Repositorios Atributos

        private MongoRepository<Usuario> _repositoryUsuarioAtributo;

        private MongoRepository<TransacaoPagamento> _repositoryTransacaoPagamentoAtributo;

        private MongoRepository<Midia> _repositoryMidiaAtributo;

        #endregion

    }
}