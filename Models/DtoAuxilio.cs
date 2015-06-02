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
    public class DtoAuxilio : Entity
    {
        public bool Concedido { get; set; }

        public string SemestreId { get; set; }

        public string InstituicaoId { get; set; }

        public string UserId { get; set; }

        public string Curso { get; set; }

        public string Observacoes { get; set; }

        public string NumMatricula { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataDoPedido { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataConcessao { get; set; }

        public DtoAuxilioDisciplina[] Disciplinas { get; set; }
    }

    [Serializable]
    public class DtoAuxilioDisciplina : Entity
    {
        public bool EAD { get; set; }

        public string Nome { get; set; }

        public bool[] DiasSemana { get; set; }

        public string Turno { get; set; }

        public bool TransporteIda { get; set; }

        public bool TransporteVolta { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataInicio { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataTermino { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime[] DatasEncontrosPresenciais { get; set; }

        public string Observacoes { get; set; }


        public DtoAuxilioDisciplina()
        {
            this.DiasSemana = new bool[7];
            this.DatasEncontrosPresenciais = new DateTime[0];
        }
    }

    public enum TurnoDisciplina
    {
        Matutino,
        Tarde,
        Verpertino,
        Noturno,
    }
}
