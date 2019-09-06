using System.Threading.Tasks;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ClientesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [HttpGet]
        // método get para buscar todos clientes
        public async Task<IActionResult> RetornaTodosClientes()
        {

            var Core = new ClienteCore(_mapper).BuscarTodosClientes();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // método get para buscar cliente por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetonaCliente(string id)
        {
            var Core = new ClienteCore(_mapper).BuscarCliente(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        //método para efetuar a busca por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new ClienteCore(_mapper).ClentesPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        //método para efetuar a busca por data.
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> RetornoPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new ClienteCore(_mapper).BuscaClientePorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] ClienteView cliente)
        {
            var Core = new ClienteCore(cliente).CadastrarCliente();
            return Core.Status ? Created($"https://localhost/api/Clientes/{Core.Resultado.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar um cliente
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPorId([FromBody]ClienteView cliente, string id)
        {
            var Core = new ClienteCore(cliente, _mapper).AtualizarCliente(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar cliente por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(string id)
        {
            var Core = new ClienteCore(_mapper).DeletarCliente(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}