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

        private ComprasContext _contexto { get; set; }
        public ProdutosController(ComprasContext contexto)
        {
            _contexto = contexto;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto) => Created("", new ProdutoCore(_contexto).Cadastrar(produto));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new ProdutoCore(_contexto).AcharId(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ProdutoCore(_contexto).AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Produto produto ) => Ok(new ProdutoCore(_contexto).Atualizar(produto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ProdutoCore(_contexto).DeletarUm(id);
            return NoContent();
        }
    }
}