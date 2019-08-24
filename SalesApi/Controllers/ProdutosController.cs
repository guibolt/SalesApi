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

       // Método post para cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            var Core = new ProdutoCore(produto).CadastrarProduto();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{produto.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");
        }
        // método get para buscar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new ProdutoCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        }
        // método get para buscar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ProdutoCore().AcharTodos().Resultado);
        // método put para atualizar um produto
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Produto produto, string id) => Ok(new ProdutoCore().AtualizarUm(id, produto).Resultado);
        // método para deletar por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new ProdutoCore().DeletarId(id));
        
    }
}