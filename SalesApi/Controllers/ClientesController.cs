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
    // Controller para os clientes.
    public class ClientesController : ControllerBase
    {
        //Construtor contendo o contexto.
        private ComprasContext _contexto { get; set; }
        //Método http post para registrar um cliente.
        public ClientesController(ComprasContext contexto){    _contexto = contexto; }
        //Método http post para registrar um cliente.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente) =>  Created("", new ClienteCore(_contexto).Cadastrar(cliente));

        //Método http get para buscar um cliente por id. 
        [HttpGet("{id}")]
        public IActionResult Get(string id) => Ok(new ClienteCore(_contexto).AcharId(id));
        //Método http get para listra  um cliente.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ClienteCore(_contexto).AcharTodos());

        //Método http put para atualizar os dados um cliente. 
        [HttpPut("{att}")]
        public async Task<IActionResult> Put([FromBody]Cliente cliente) => Ok(new ClienteCore(_contexto).Atualizar(cliente));

        // Método http delete para deletar um cliente.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ClienteCore(_contexto).DeletarUm(id);
            return NoContent();
        }
    }
}