using System.Collections.Generic;

namespace Model
{
    public class Sistema
    {
        // Model para armazenar os dados
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public Sistema() { }
    }
}
