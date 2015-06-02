using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DtoArquivo : Entity
    {
        public string Nome { get; set; }
        public double Tamanho { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataUpload { get; set; }

        public string UserId { get; set; }

        public string TipoArquivo { get; set; }
        public string ExtencaoArquivo { get; set; }

        public bool Verificado { get; set; }

        public bool Aceito { get; set; }

        public byte[] File { get; set; }

        public string FKId { get; set; }
    }
}
