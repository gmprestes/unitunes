namespace MongoRepository
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Configuration;
    using System.Web;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private bool disposed = false;

        /// <summary>
        /// MongoCollection field.
        /// </summary>
        private MongoCollection<T> collection;

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        #region Contructors
        /// <summary>
        /// Metodo que possui parametro para informar atributo para função de max
        /// </summary>
        /// <param name="url"></param>
        /// <param name="atributeForMaxFunction">Nome do atributo da entidade a ser usado na função de Max</param>
        public MongoRepository(MongoUrl url)
        {
            this.Init(url);
        }

        private void Init(MongoUrl url)
        {
            //this.collection = Util.GetCollection<T>(url);

            var client = new MongoClient(url);
            var server = client.GetServer();
            var settings = new MongoDatabaseSettings(server, url.DatabaseName);
            var dataBase = server.GetDatabase(url.DatabaseName, settings);

            this.collection = dataBase.GetCollection<T>(typeof(T).Name);
        }

        #endregion

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager<T>
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        public MongoCollection<T> Collection
        {
            get
            {
                return this.collection;
            }
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The string representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public T GetById(string id)
        {
            if (typeof(T).IsSubclassOf(typeof(Entity)))
            {
                return this.collection.FindOneByIdAs<T>(new ObjectId(id));
            }

            return this.collection.FindOneByIdAs<T>(id);
        }

        /// <summary>
        /// Returns a single T by the given criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>A single T matching the criteria.</returns>
        public T GetSingle(Expression<Func<T, bool>> criteria)
        {
            return this.collection.AsQueryable<T>().Where(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Returns the list of T where it matches the criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>IQueryable of T.</returns>
        public IQueryable<T> All(Expression<Func<T, bool>> criteria)
        {
            return this.collection.AsQueryable<T>().Where(criteria);
        }

        /// <summary>
        /// Returns All the records of T.
        /// </summary>
        /// <returns>IQueryable of T.</returns>
        public IQueryable<T> All()
        {
            return this.collection.AsQueryable<T>();
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public T Add(T entity)
        {

            #region Evitar Duplicidades -  GAMBIARRA NIVEL SENIOR
            //var jsonNewEntity = entity.ToJson();
            //jsonNewEntity = jsonNewEntity.Replace(entity.ToJson().Split(',')[0], string.Empty);
            //jsonNewEntity = Regex.Replace(jsonNewEntity, @"ISODate(\(.+?)\)", string.Empty, RegexOptions.IgnoreCase);
            //jsonNewEntity = jsonNewEntity.Replace("null", string.Empty);

            //if (HttpContext.Current != null)
            //    if (HttpContext.Current.Session != null)
            //        if (HttpContext.Current.Session["__last"] != null)
            //        {
            //            var lastEntity = this.GetById(HttpContext.Current.Session["__last"].ToString());

            //            if (lastEntity != null && lastEntity.Id != null)
            //            {
            //                var jsonLastEntity = lastEntity.ToJson();
            //                jsonLastEntity = jsonLastEntity.Replace(lastEntity.ToJson().Split(',')[0], string.Empty);
            //                jsonLastEntity = Regex.Replace(jsonLastEntity, @"ISODate(\(.+?)\)", string.Empty, RegexOptions.IgnoreCase);
            //                jsonLastEntity = jsonLastEntity.Replace("null", string.Empty);

            //                if (jsonLastEntity == jsonNewEntity)
            //                    return lastEntity;
            //            }
            //        }
            #endregion

            //this.collection.exi
            this.collection.Insert<T>(entity);

            ////ULTIMO ID SALVO
            //if (HttpContext.Current != null)
            //    if (HttpContext.Current.Session != null)
            //        HttpContext.Current.Session["__last"] = entity.Id;

            return entity;
        }



        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public T Update(T entity)
        {
            //FAZ UPDATE SE EXISTE
            if (entity.Id != null)
            {
                var oldEntity = this.collection.FindOneById(new ObjectId(entity.Id));
                if (oldEntity != null && !string.IsNullOrEmpty(entity.Id))
                {
                    this.collection.Save<T>(entity);
                }
            }

            return entity;
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The string representation of the entity's id.</param>
        public void Delete(string id, bool IgnorarPermissaoUsuario)
        {
            //var entity = this.collection.FindOneById(new ObjectId(id));

            ////var entity = this.collection.FindOneByIdAs<T>(id);

            if (typeof(T).IsSubclassOf(typeof(Entity)))
            {
                this.collection.Remove(Query.EQ("_id", new ObjectId(id)));
            }
            else
            {
                this.collection.Remove(Query.EQ("_id", id));
            }
        }



        public void Delete(string id)
        {
            this.Delete(id, false);
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(T entity)
        {
            this.Delete(entity.Id);
        }

        /// <summary>
        /// Deletes the entities matching the criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        public void Delete(Expression<Func<T, bool>> criteria)
        {
            this.Delete(criteria, false);
        }

        public void Delete(Expression<Func<T, bool>> criteria, bool IgnorarPermissaoUsuario)
        {
            foreach (T entity in this.collection.AsQueryable<T>().Where(criteria))
            {
                this.Delete(entity.Id, IgnorarPermissaoUsuario);
            }
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public long Count()
        {
            return this.collection.Count();
        }

        public long Count(IMongoQuery query)
        {
            return this.collection.Count(query);
        }

        /* O FANTASTICO MUNDO GENERICO COM BAIXA DEPENDENCIA E ALTA SEGREGAÇÃO DE CÓDIGO */
        public int Max(string property)
        {
            int max = 1;

            #region Bsondocuments de agregação de consulta

            var list = new List<BsonDocument>();

            var group = new BsonDocument 
                    { 
                        { "$group", 
                            new BsonDocument 
                                { 
                                    { 
                                        "_id", 0
                                    },
                                    { 
                                        "Max", new BsonDocument 
                                            { 
                                                { 
                                                    "$max","$"+ property
                                                } 
                                             } 
                                    } 
                                }    
                        } 
                    };

            list.Add(group);

            var agregateArgs = new AggregateArgs()
            {
                Pipeline = list
            };

            #endregion

            var result = this.Collection.Aggregate(agregateArgs).FirstOrDefault();

            if (result != null)
                max += result["Max"].AsInt32;

            return max;
        }

        public double MaxDouble(string property)
        {
            double max = 1;

            #region Bsondocuments de agregação de consulta

            var list = new List<BsonDocument>();

            var group = new BsonDocument 
                    { 
                        { "$group", 
                            new BsonDocument 
                                { 
                                    { 
                                        "_id", 0
                                    },
                                    { 
                                        "Max", new BsonDocument 
                                            { 
                                                { 
                                                    "$max","$"+ property
                                                } 
                                             } 
                                    } 
                                }    
                        } 
                    };

            list.Add(group);

            var agregateArgs = new AggregateArgs()
            {
                Pipeline = list
            };

            #endregion

            var result = this.Collection.Aggregate(agregateArgs).FirstOrDefault();

            if (result != null)
                max += result["Max"].AsDouble;

            return max;
        }

        public int Max(IMongoQuery query, string property)
        {
            int max = 1;

            #region Bsondocuments de agregação de consulta

            var list = new List<BsonDocument>();

            var group = new BsonDocument 
                    { 
                        { "$group", 
                            new BsonDocument 
                                { 
                                    { 
                                        "_id", 0
                                    },
                                    { 
                                        "Max", new BsonDocument 
                                            { 
                                                { 
                                                    "$max","$"+ property
                                                } 
                                             } 
                                    } 
                                }    
                        } 
                    };


            var match = new BsonDocument 
                { 
                    { 
                        "$match", 
                        query.ToBsonDocument()
                    } 
                };


            list.Add(match);
            list.Add(group);

            var agregateArgs = new AggregateArgs()
            {
                Pipeline = list
            };

            #endregion

            var result = this.Collection.Aggregate(agregateArgs).FirstOrDefault();

            if (result != null)
                max += result["Max"].AsInt32;

            return max;
        }

        public double SumDouble(IMongoQuery query, string column)
        {
            var list = new List<BsonDocument>();
            var group = new BsonDocument 
                { 
                    { "$group", 
                        new BsonDocument 
                            { 
                                { "_id", 0
                                             
                                }, 
                                { 
                                    "Total", new BsonDocument 
                                                 { 
                                                     { 
                                                         "$sum","$" + column
                                                     } 
                                                 } 
                                } 
                            } 
                  } 
                };



            var match = new BsonDocument 
                { 
                    { 
                        "$match", 
                        query.ToBsonDocument()
                    } 
                };


            list.Add(match);
            list.Add(group);

            var agregateArgs = new AggregateArgs();
            agregateArgs.Pipeline = list;

            var result = this.collection.Aggregate(agregateArgs).FirstOrDefault();

            if (result != null)
                return result["Total"].AsDouble;

            return 0;
        }

        public double SumDouble(string column)
        {
            var list = new List<BsonDocument>();
            var group = new BsonDocument 
                { 
                    { "$group", 
                        new BsonDocument 
                            { 
                                { "_id", 0
                                             
                                }, 
                                { 
                                    "Total", new BsonDocument 
                                                 { 
                                                     { 
                                                         "$sum","$" + column
                                                     } 
                                                 } 
                                } 
                            } 
                  } 
                };


            list.Add(group);

            var agregateArgs = new AggregateArgs();
            agregateArgs.Pipeline = list;

            var result = this.collection.Aggregate(agregateArgs).FirstOrDefault();

            if (result != null)
                return result["Total"].AsDouble;

            return 0;
        }



        /// <summary>
        /// Checks if the entity exists for given criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>true when an entity matching the criteria exists, false otherwise.</returns>
        public bool Exists(Expression<Func<T, bool>> criteria)
        {
            return this.collection.AsQueryable<T>().Any(criteria);
        }

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        /// on the same connection. The return value of this method implements IDisposable and can be placed in a using
        /// statement (in which case RequestDone will be called automatically when leaving the using statement). 
        /// </summary>
        /// <returns>A helper object that implements IDisposable and calls RequestDone() from the Dispose method.</returns>
        /// <remarks>
        ///     <para>
        ///         Sometimes a series of operations needs to be performed on the same connection in order to guarantee correct
        ///         results. This is rarely the case, and most of the time there is no need to call RequestStart/RequestDone.
        ///         An example of when this might be necessary is when a series of Inserts are called in rapid succession with
        ///         SafeMode off, and you want to query that data in a consistent manner immediately thereafter (with SafeMode
        ///         off the writes can queue up at the server and might not be immediately visible to other connections). Using
        ///         RequestStart you can force a query to be on the same connection as the writes, so the query won't execute
        ///         until the server has caught up with the writes.
        ///     </para>
        ///     <para>
        ///         A thread can temporarily reserve a connection from the connection pool by using RequestStart and
        ///         RequestDone. You are free to use any other databases as well during the request. RequestStart increments a
        ///         counter (for this thread) and RequestDone decrements the counter. The connection that was reserved is not
        ///         actually returned to the connection pool until the count reaches zero again. This means that calls to
        ///         RequestStart/RequestDone can be nested and the right thing will happen.
        ///     </para>
        /// </remarks>
        public IDisposable RequestStart()
        {
            return this.collection.Database.RequestStart();
        }

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        /// on the same connection. The return value of this method implements IDisposable and can be placed in a using
        /// statement (in which case RequestDone will be called automatically when leaving the using statement). 
        /// </summary>
        /// <returns>A helper object that implements IDisposable and calls RequestDone() from the Dispose method.</returns>
        /// <param name="slaveOk">Whether queries should be sent to secondary servers.</param>
        /// <remarks>
        ///     <para>
        ///         Sometimes a series of operations needs to be performed on the same connection in order to guarantee correct
        ///         results. This is rarely the case, and most of the time there is no need to call RequestStart/RequestDone.
        ///         An example of when this might be necessary is when a series of Inserts are called in rapid succession with
        ///         SafeMode off, and you want to query that data in a consistent manner immediately thereafter (with SafeMode
        ///         off the writes can queue up at the server and might not be immediately visible to other connections). Using
        ///         RequestStart you can force a query to be on the same connection as the writes, so the query won't execute
        ///         until the server has caught up with the writes.
        ///     </para>
        ///     <para>
        ///         A thread can temporarily reserve a connection from the connection pool by using RequestStart and
        ///         RequestDone. You are free to use any other databases as well during the request. RequestStart increments a
        ///         counter (for this thread) and RequestDone decrements the counter. The connection that was reserved is not
        ///         actually returned to the connection pool until the count reaches zero again. This means that calls to
        ///         RequestStart/RequestDone can be nested and the right thing will happen.
        ///     </para>
        /// </remarks>
        [Obsolete("Use the connectionstring to specify the readpreference; add \"readPreference=X\" where X is one of the following values: primary, primaryPreferred, secondary, secondaryPreferred, nearest. See http://docs.mongodb.org/manual/applications/replication/#read-preference")]
        public IDisposable RequestStart(bool slaveOk)
        {
            return this.collection.Database.RequestStart(slaveOk ? ReadPreference.SecondaryPreferred : ReadPreference.Primary);

        }

        /// <summary>
        /// Lets the server know that this thread is done with a series of related operations.
        /// </summary>
        /// <remarks>
        /// Instead of calling this method it is better to put the return value of RequestStart in a using statement.
        /// </remarks>
        public void RequestDone()
        {
            this.collection.Database.RequestDone();
        }
    }
}