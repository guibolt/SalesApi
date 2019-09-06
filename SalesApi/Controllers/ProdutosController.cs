using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ProdutosController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        // método get para buscar todos produtos
        public async Task<IActionResult> RetornaTodosProdutos()
        {
            var Core = new ProdutoCore(_mapper).BuscarTodosProdutos();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        [HttpGet("Categorias")]
        // método get para exibir as categorias possiveis
        public async Task<IActionResult> RetornaCategorias()
        {
            var Core = new ProdutoCore(_mapper).ExibirCategorias();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método get para buscar produto por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornaUmProduto(string id)
        {
            var Core = new ProdutoCore(_mapper).BuscarUmProduto(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
       
        //Método get para a exibição por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPaginacao([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new ProdutoCore(_mapper).ProdutosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        //método get para efeturar a busca por data
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> RetornoPorData([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new ProdutoCore(_mapper).BuscaProdutoPorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarProduto([FromBody] ProdutoView produto)
        {
            var Core = new ProdutoCore(produto,_mapper).CadastrarUmProduto();
            return Core.Status ? Created($"https://localhost/api/Produtos/{Core.Resultado.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar um produto
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto([FromBody]ProdutoView produto, string id)
        {
            var Core = new ProdutoCore(produto,_mapper).AtualizarUmProduto(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar produto por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(string id)
        {
            var Core = new ProdutoCore(_mapper).DeletarProdutoPorId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}