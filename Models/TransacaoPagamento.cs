using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TransacaoPagamento : Entity
    {

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataTransacao { get; set; }

        public double ValorTransacao { get; set; }

        public CartaoCredito Cartao { get; set; }

        public Usuario Usuario { get; set; }
    }
}
