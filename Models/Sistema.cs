using System.Collections.Generic;

namespace Model
{
    public class Sistema
    {
        // Model para armazenar os dados
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
<<<<<<< HEAD
=======
        public List<PedidoCliente> PedidoClientes { get; set; } = new List<PedidoCliente>();
>>>>>>> 7787b64ef79fb821fa293c96d5d5f33295c2e7ac
        public Sistema() { }
    }
}
