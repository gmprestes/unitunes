using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class CartaoCredito : Entity
    {
        public string ValidadeMes { get; set; }

        public string ValidadeAno { get; set; }

        public string Numero { get; set; }

        public string CVC { get; set; }

        public string NomeTitular { get; set; }

        public string Bandeira { get; set; }

        public CartaoCredito()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.Bandeira = "visa";
            this.ValidadeMes = "01";
            this.ValidadeAno = "2015";
        }
    }
}
