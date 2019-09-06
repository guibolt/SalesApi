using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Promocao : Base
    {
        public string Descricao { get; set; }
        public DateTime DataFinal { get; set; }
        public double TaxaDesconto { get; set; }
        public Categorias Categoria { get; set; }
        public bool Concluida { get; set; } = false;

        //método para promover a mudança no valor do produto.
        public void MudaValor(Produto produto)
        {
            var taxa = TaxaDesconto/100;
            produto.Preco -= produto.Preco * taxa;
        }

        //Método para realizar a validacao da promocao
       public bool ValidaPromocao()
        {
            if (DateTime.Now >= DataFinal || Concluida)
            {
                Concluida = true;
                return false;
            }

            return true;
        }
    }
}
