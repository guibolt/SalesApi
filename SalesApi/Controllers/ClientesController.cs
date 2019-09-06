using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        [HttpGet]
        // método get para buscar todos clientes
        public async Task<IActionResult> RetornaTodosClientes()
        {
            var Core = new ClienteCore().BuscarTodosClientes();
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // método get para buscar cliente por id
        [HttpGet("{id}")]
        public async Task<IActionResult> RetonaCliente(string id)
        {
            var Core = new ClienteCore().BuscarCliente(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        //método para efetuar a busca por paginação
        [HttpGet("Paginas")]
        public async Task<IActionResult> RetornoPorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new ClienteCore().ClentesPorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        //método para efetuar a busca por data.
        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> RetornoPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new ClienteCore().BuscaClientePorData(DataComeco, DataFim);
            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);
        }

        // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] Cliente cliente)
        {
            var Core = new ClienteCore(cliente).CadastrarCliente();
            return Core.Status ? Created($"https://localhost/api/Clientes/{cliente.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método put para atualizar um cliente
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPorId([FromBody]Cliente cliente, string id)
        {
            var Core = new ClienteCore().AtualizarCliente(id, cliente);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // método para deletar cliente por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(string id)
        {
            var Core = new ClienteCore().DeletarCliente(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}