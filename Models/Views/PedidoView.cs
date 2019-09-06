using System;
using System.Collections.Generic;

namespace Model
{
    // Model de pedido
    public class PedidoView 
    {
        public List<Produto> Produto{ get; set; } = new List<Produto>();
        public string ClienteId { get; set; }
    }
}
