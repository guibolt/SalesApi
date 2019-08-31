using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        // método get para buscar todos
        public async Task<IActionResult> Get() => Ok(new ProdutoCore().AcharTodos().Resultado);

        // método get para buscar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new ProdutoCore().BuscarUmProduto(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
       
        [HttpGet("Paginas")]
        public async Task<IActionResult> PorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new ProdutoCore().ProdutosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            // verifico se pagina que o usuario pediu é valida, se nao retorno um BadRequest
            if (Core.Resultado.Count == 0)
                return BadRequest("Essa pagina não existe!");
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            var Core = new ProdutoCore(produto).CadastrarUmProduto();
            return Core.Status ? Created($"https://localhost/api/Produtos/{produto.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar um produto
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Produto produto, string id)
        {
            var Core = new ProdutoCore().AtualizarUmProduto(id, produto);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Core = new ProdutoCore().DeletarProdutoPorId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}