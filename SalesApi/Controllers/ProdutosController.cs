using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            var Core = new ProdutoCore(produto).CadastrarProduto();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{produto.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new ProdutoCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ProdutoCore().AcharTodos().Resultado);

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Produto produto, string id) => Ok(new ProdutoCore().AtualizarUm(id, produto).Resultado);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new ProdutoCore().DeletarId(id));
        
    }
}