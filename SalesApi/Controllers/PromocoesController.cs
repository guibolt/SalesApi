using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesController: ControllerBase
    {
        [HttpGet]
        // método get para buscar todos as promocoes
        public async Task<IActionResult> RetornaTodasPromocoes()
        {
            var Core = new PromocaoCore().BuscarTodasPromocoes();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // método get para buscar promocao por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornaPromocao(string id)
        {
            var Core = new PromocaoCore().BuscarumaPromocao(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
       //Método get para a exiição por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new PromocaoCore().ProdutosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarPromocao([FromBody] Promocao produto)
        {
            var Core = new PromocaoCore(produto).CadastrarPromocao();
            return Core.Status ? Created($"https://localhost/api/Produtos/{produto.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar uma promocão
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPromocao([FromBody]Promocao promocao, string id)
        {
            var Core = new PromocaoCore().AtualizarumaPromocao(id, promocao);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPromocao(string id)
        {
            var Core = new PromocaoCore().DeletarPromocao(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}