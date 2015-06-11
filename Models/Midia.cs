using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Midia : Entity
    {
        public string Descricao { get; set; }
        public string Nome { get; set; } // Uhu !!! Senha em texto plano
        public double Preco { get; set; }

        public string AutorId { get; set; }

        public Midia()
        {
            this.Preco = 0;
        }
    }
}
