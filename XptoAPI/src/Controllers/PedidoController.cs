using Microsoft.AspNetCore.Mvc;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;

namespace XptoAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet("cozinha")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosCozinha()
        {
            var pedidos = await _pedidoService.GetPedidosCozinhaAsync();
            return Ok(pedidos);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, StatusPedido status)
        {
            try
            {
                await _pedidoService.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
