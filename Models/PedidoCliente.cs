using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    class PedidoCliente
    {
        //Classe com o intuito de amarrar o produto e o cliente 
        public int ClienteId { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public Cliente Cliente { get; set; }
    }
}
