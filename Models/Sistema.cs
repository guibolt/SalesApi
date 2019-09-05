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

        public List <string> ListaCategorias { get; set; } = new List<string>
            {
                "SMARTPHONES Categoria: 1",
                "INFORMATICA Categoria: 2",
                "GAMES Categoria: 3",
                "VESTUARIO Categoria: 4",
                "SAUDE Categoria: 5",
                "FITNESS Categoria: 6",
                "MOVEIS Categoria: 7",
                "BRINQUEDOS Categoria: 8",
                "COSMETICOS Categoria: 9",
                "ELECTRODOMESTICOS Categoria: 10"
            };
        public Sistema() { }
    }
}
