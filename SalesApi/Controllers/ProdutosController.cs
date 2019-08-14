using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto) => Created("", new ProdutoCore().Cadastrar(produto));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new ProdutoCore().AcharId(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ProdutoCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Produto produto , string id) => Ok(new ProdutoCore().Atualizar(id));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ProdutoCore().DeletarUm(id);
            return NoContent();
        }
    }
}