using Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    // Model de promoção
    public class Promocao : Base
    {
        public string Descricao { get; set; }
        public DateTime DataFinal { get; set; }
        public double TaxaDesconto { get; set; }
        public Categorias Categoria { get; set; }
        public bool Concluida { get; set; } = false;

        /// <summary>
        /// método para promover a mudança no valor do produto.
        /// </summary>
        /// <param name="produto"></param>
        public void MudaValor(Produto produto)
        {
            var taxa = TaxaDesconto/100;
            produto.Preco -= produto.Preco * taxa;
        }

       /// <summary>
       /// Método para realizar a validação da promoção, baseado na data final.
       /// </summary>
       /// <returns></returns>
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
