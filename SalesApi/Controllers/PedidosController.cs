using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpGet]
        // método get para buscar todos os pedidos
        public async Task<IActionResult> RetornarTodosClientes()
        {
            var Core = new PedidoCore().BuscarTodosPedidos();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }


        // método get para buscar pedido por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornarCliente(string id)
        {
            var Core = new PedidoCore().BuscarProdutoPorId(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        //método get para a exibição por pagina
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new PedidoCore().PedidosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        //método get para efeturar a busca por data
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> RetornoPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new PedidoCore().BuscaPedidoPorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro de um produto
        [HttpPost]
        public async Task<IActionResult> EfetuarPedido([FromBody] Pedido pedido)
        {
            var Core = new PedidoCore(pedido).RealizarUmPedido();
            return Core.Status ? Created($"https://localhost/api/Pedidos/{Core.Resultado.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar produto por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(string id)
        {
            var Core = new PedidoCore().DeletarPedidoPorID(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}