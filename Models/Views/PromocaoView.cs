using Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PromocaoView
    {
        public string Descricao { get; set; }
        public DateTime DataFinal { get; set; }
        public double TaxaDesconto { get; set; }
        public Categorias Categoria { get; set; }
    }
}
