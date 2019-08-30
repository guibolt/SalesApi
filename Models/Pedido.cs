using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class Pedido 
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();

        public Cliente Cliente { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 6);
        public DateTime DataDoPedido { get; set; } = DateTime.Now;
        public double ValorTotal { get; set; }

        // Método para realizar o calcuto atribuir o valor ao pedido.
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
