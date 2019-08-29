using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiForSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pedido pedido)
        {
            var Core = new PedidoCore(pedido).RealizarPedido();
            return Core.Status ? Created($"https://localhost/api/Pedidos/{pedido.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }




    }
}