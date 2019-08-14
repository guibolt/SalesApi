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
    public class ItensController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item) => Created("", new ItemCore().Cadastrar(item));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new ItemCore().AcharId(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new ItemCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Item item, string id) => Ok(new ItemCore().Atualizar(id));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new ItemCore().DeletarUm(id);
            return NoContent();
        }
    }
}