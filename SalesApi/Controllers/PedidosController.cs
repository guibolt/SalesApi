using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpGet]
        // método get para buscar todos
        public async Task<IActionResult> Get() => Ok(new PedidoCore().AcharTodos().Resultado);

        // método get para buscar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PedidoCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        [HttpGet("Paginas")]
        public async Task<IActionResult> PorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new PedidoCore().PorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            // verifico se pagina que o usuario pediu é valida, se nao retorno um BadRequest
            if (Core.Resultado.Count == 0)
                return BadRequest("Essa pagina não existe!");
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> AcharPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new PedidoCore().BuscaPorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pedido pedido)
        {
            var Core = new PedidoCore(pedido).RealizarPedido();
            return Core.Status ? Created($"https://localhost/api/Pedidos/{pedido.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Core = new ProdutoCore().DeletarId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}