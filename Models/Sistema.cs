using System.Collections.Generic;

namespace Model
{
    // Model para armazenar os dados
    public class Sistema
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public List<Promocao> Promocoes { get; set; } = new List<Promocao>();
        public Sistema() { }
    }
}
