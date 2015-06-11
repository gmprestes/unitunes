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
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Validade { get; set; }

        public string Numero { get; set; }

        public string CVC { get; set; }

        public string NomeTitular { get; set; }

        public string Bandeira { get; set; }
    }
}
