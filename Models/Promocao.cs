using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Promocao : Base
    {
        public string Descricao { get; set; }
        public DateTime DataFinal { get; set; }
        public int TaxaDesconto { get; set; }
        public Categorias Categoria { get; set; }
    }
}
