using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class PedidoView 
    {
        public List<Produto> Produtos { get; set; }
        public string ClienteId { get; set; }
    }
}
