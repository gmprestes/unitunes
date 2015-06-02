using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DtoNotasFiscais : Entity
    {
        public string Mes { get; set; }
        public string Ano { get; set; }

        public string Nome { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataCadastro { get; set; }
    }
}
