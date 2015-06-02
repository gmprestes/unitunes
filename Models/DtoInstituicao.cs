using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Serializable]
    public class DtoInstituicao : Entity
    {
        public bool Ativo { get; set; }

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public string Descricao { get; set; }
    }
}
