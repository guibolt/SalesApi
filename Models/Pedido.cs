using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
   public class Pedido 
    {
        public Cliente Cliente { get; set; }
        public List<Item> Itens { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataDoPedido { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Finalizado { get; set; }
    }
}
