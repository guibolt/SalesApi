using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class PedidoView 
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public string ClienteId { get; set; }
    }
}
