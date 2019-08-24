using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Sistema
    {
        // Model para armazenar os dados
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public List<PedidoCliente> PedidoClientes { get; set; } = new List<PedidoCliente>();
        public Sistema() { }
    }
}
