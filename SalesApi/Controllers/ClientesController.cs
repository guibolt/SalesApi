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

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            var Core = new ClienteCore(cliente).CadastrarProduto();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{cliente.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new ClienteCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ClienteCore().AcharTodos().Resultado);

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Cliente cliente, string id) => Ok(new ClienteCore().AtualizarUm(id, cliente).Resultado);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new ClienteCore().DeletarId(id));
        
    }
}