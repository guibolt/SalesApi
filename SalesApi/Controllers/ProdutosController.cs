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
        // método get para buscar todos produtos
        public async Task<IActionResult> RetornaTodosProdutos()
        {
            var Core = new ProdutoCore().BuscarTodosProdutos();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        [HttpGet("Categorias")]
        // método get para exibir as categorias possiveis
        public async Task<IActionResult> RetornaCategorias()
        {
            var Core = new ProdutoCore().ExibirCategorias();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método get para buscar produto por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornaUmProduto(string id)
        {
            var Core = new ProdutoCore().BuscarUmProduto(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
       
        //Método get para a exibição por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPaginacao([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new ProdutoCore().ProdutosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        //método get para efeturar a busca por data
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> RetornoPorData([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new ProdutoCore().BuscaProdutoPorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarProduto([FromBody] Produto produto)
        {
            var Core = new ProdutoCore(produto).CadastrarUmProduto();
            return Core.Status ? Created($"https://localhost/api/Produtos/{produto.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar um produto
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto([FromBody]Produto produto, string id)
        {
            var Core = new ProdutoCore().AtualizarUmProduto(id, produto);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar produto por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(string id)
        {
            var Core = new ProdutoCore().DeletarProdutoPorId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}