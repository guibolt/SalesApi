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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente) => Created("", new ClienteCore().Cadastrar(cliente));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new ClienteCore().AcharId(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ClienteCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Cliente cliente, string id) => Ok(new ClienteCore().Atualizar(id));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ClienteCore().DeletarUm(id);
            return NoContent();
        }
    }
}