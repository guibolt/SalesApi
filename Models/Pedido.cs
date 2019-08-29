using System;
using System.Collections.Generic;

namespace Model
{
    public class Pedido 
    {
<<<<<<< HEAD
        public List<Produto> Produtos { get; set; } = new List<Produto>();
=======
        // Model de pedido
>>>>>>> 7787b64ef79fb821fa293c96d5d5f33295c2e7ac
        public Cliente Cliente { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 6);
        public DateTime DataDoPedido { get; set; } = DateTime.Now;
        public double ValorTotal { get; set; }

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
