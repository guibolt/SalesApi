using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesController: ControllerBase
    {
        // propriedade automapper
        private readonly IMapper _mapper;

        // construtor para a utilização do automapper por meio de injeçao de dependecia
        public PromocoesController(IMapper Mapper)
        {
            _mapper = Mapper;
        }
        [HttpGet]
        // método get para buscar todos as promocoes
        public async Task<IActionResult> RetornaTodasPromocoes()
        {
            var Core = new PromocaoCore(_mapper).BuscarTodasPromocoes();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // método get para buscar promocao por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornaPromocao(string id)
        {
            var Core = new PromocaoCore(_mapper).BuscarumaPromocao(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
       //Método get para a exiição por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new PromocaoCore(_mapper).ProdutosPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarPromocao([FromBody] PromocaoView promocao)
        {
            var Core = new PromocaoCore(promocao, _mapper).CadastrarPromocao();
            return Core.Status ? Created($"https://localhost/api/Produtos/{Core.Resultado.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar uma promocão
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPromocao([FromBody]PromocaoView promocao, string id)
        {
            var Core = new PromocaoCore(promocao, _mapper).AtualizarumaPromocao(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPromocao(string id)
        {
            var Core = new PromocaoCore(_mapper).DeletarPromocao(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}