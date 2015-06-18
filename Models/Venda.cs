using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Venda : Entity
    {
        public string MidiaId { get; set; }
        public string UserId { get; set; }
        public DateTime DataVenda { get; set; }
        public double ValorVenda { get; set; }
        public double TaxaUnitunes { get; set; }
        public double ValorAutor { get; set; }
    }
}
