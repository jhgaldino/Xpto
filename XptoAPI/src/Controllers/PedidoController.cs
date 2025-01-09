using Microsoft.AspNetCore.Mvc;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using ErrorOr; // Adicionado
using XptoAPI.src.Common.Errors; // Adicionado

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

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var result = await _pedidoService.GetByIdAsync(id);

            return result.Match<ActionResult<Pedido>>(
                pedido => Ok(pedido),
                errors => NotFound(new { errors = errors.Select(e => e.Description) })
            );
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CreatePedido(Pedido pedido)
        {
            var result = await _pedidoService.CreatePedidoAsync(pedido);

            return result.Match<ActionResult<Pedido>>(
                pedido => CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido),
                errors => BadRequest(new { errors = errors.Select(e => e.Description) }));
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, StatusPedido status)
        {
            var result = await _pedidoService.UpdateStatusAsync(id, status);

            return result.Match<IActionResult>(
                _ => NoContent(),
                onError: errors => errors[0].Type switch
                {
                    ErrorType.NotFound => NotFound(errors),
                    _ => BadRequest(new { errors = errors.Select(e => e.Description) })
                });
        }
    }
}
