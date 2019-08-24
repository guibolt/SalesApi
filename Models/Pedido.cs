using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class Pedido 
    {
        // Model de pedido
        public Cliente Cliente { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataDoPedido { get; set; } = DateTime.Now;
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Finalizado { get; set; }
    }
}
