using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    class PedidoCliente
    {
        public int ClienteId { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public Cliente Cliente { get; set; }
    }
}
