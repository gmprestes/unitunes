using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    public enum TipoArquivo
    {
        ComprovanteEnderecoPerfil,
        ComprovanteIdentidadePerfil,
        ComprovanteTituloEleitor,
        ComprovanteCPF,
        ComprovanteCertidaoNascimento,


        ComprovanteMatriculaSemestre,
        ComprovanteNotasUltimoSemestre,

    }
    public class DtoPessoa : Entity
    {
        public bool EmailVerificado { get; set; }
        public bool CadastroVerificado { get; set; }
        public bool NaoAssociado { get; set; }

        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string TituloEleitoral { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Logradouro { get; set; }
        public string LogradouroNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }

        public string UserId { get; set; }


    }
}
