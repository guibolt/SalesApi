using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class Pedido : Base
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public Cliente Cliente { get; set; }
        public double ValorTotal { get; set; }

        /// <summary>
        ///Método para realizar o calculo do pedido completo atribuir o valor ao pedido.
        /// </summary>
        public void CalculaTotal()
        {
            Produtos.ForEach(c => ValorTotal += c.SubTotal());
            if (ValorTotal > 300)
                ValorTotal -= ValorTotal * 0.10;
            else if (ValorTotal > 100)
                ValorTotal -= ValorTotal * 0.05;
        }
    }
}
