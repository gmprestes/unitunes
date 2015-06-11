using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Usuario : Entity
    {
        public string Login { get; set; }
        public string Senha { get; set; } // Uhu !!! Senha em texto plano
        public double Credito { get; set; }

        public CartaoCredito CartaoCredito { get; set; }
        public List<string> Tokens { get; set; }

        public Usuario()
        {
            this.Credito = 0;
            this.Tokens = new List<string>();
            this.CartaoCredito = new CartaoCredito();
        }
    }
}
