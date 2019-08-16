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
    // Controller para os produtos.
    public class ProdutosController : ControllerBase
    {
            //Referencia ao contexto.
        private ComprasContext _contexto { get; set; }
        //Construtor contendo o contexto.
        public ProdutosController(ComprasContext contexto) {  _contexto = contexto;  }
        //Método http post para registrar um produto.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto) => Created("", new ProdutoCore(_contexto).Cadastrar(produto));

        //Método http get para buscar um produto por id . 
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new ProdutoCore(_contexto).AcharId(id));
        //Método http get para listra  um produto.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ProdutoCore(_contexto).AcharTodos());
      
        //Método http put para atualizar os dados um produto . 
        [HttpPut("{att}")]
        public async Task<IActionResult> Put([FromBody]Produto produto ) => Ok(new ProdutoCore(_contexto).Atualizar(produto));

        // Método http delete para deletar um produto.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ProdutoCore(_contexto).DeletarUm(id);
            return NoContent();
        }
    }
}