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
    public class ClientesController : ControllerBase
    {
        private ComprasContext _contexto { get; set; }
        public ClientesController(ComprasContext contexto)
        {
            _contexto = contexto;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente) =>  Created("", new ClienteCore(_contexto).Cadastrar(cliente));


        [HttpGet("{id}")]
        public IActionResult Get(string id) => Ok(new ClienteCore(_contexto).AcharId(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ClienteCore(_contexto).AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Cliente cliente) => Ok(new ClienteCore(_contexto).Atualizar(cliente));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ClienteCore(_contexto).DeletarUm(id);
            return NoContent();
        }
    }
}