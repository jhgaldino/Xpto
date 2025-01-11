using Microsoft.AspNetCore.Mvc;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using ErrorOr; // Adicionado
using XptoAPI.src.Common.Errors; // Adicionado

namespace XptoAPI.src.Controllers
{
    /// <summary>
    /// Controlador para gerenciar pedidos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        /// <summary>
        /// Inicializa uma nova instância do controlador de pedidos.
        /// </summary>
        /// <param name="pedidoService">Serviço para interação com pedidos.</param>
        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Obtém a lista de pedidos para a cozinha.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição: GET /api/Pedido/cozinha
        /// </remarks>
        /// <response code="200">Retorna a lista de pedidos</response>
        [HttpGet("cozinha")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosCozinha()
        {
            var pedidos = await _pedidoService.GetPedidosCozinhaAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Retorna um pedido pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do pedido.</param>
        /// <response code="200">Retorna o pedido encontrado</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetById(int id)
        {
            var result = await _pedidoService.GetByIdAsync(id);

            return result.Match<ActionResult<Pedido>>(
                pedido => Ok(pedido),
                errors => NotFound(new { errors = errors.Select(e => e.Description) })
            );
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedido">Objeto contendo as informações do pedido.</param>
        /// <response code="201">Retorna o pedido criado</response>
        /// <response code="400">Erro de validação nos dados do pedido</response>
        [HttpPost]
        public async Task<ActionResult<Pedido>> CreatePedido(Pedido pedido)
        {
            var result = await _pedidoService.CreatePedidoAsync(pedido);

            return result.Match<ActionResult<Pedido>>(
                pedido => CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido),
                errors => BadRequest(new { errors = errors.Select(e => e.Description) }));
        }

        /// <summary>
        /// Atualiza o status de um pedido.
        /// </summary>
        /// <param name="id">Identificador do pedido a ser atualizado.</param>
        /// <param name="status">Novo status para o pedido.</param>
        /// <response code="204">Status atualizado com sucesso</response>
        /// <response code="400">Erro de validação</response>
        /// <response code="404">Pedido não encontrado</response>
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

        /// <summary>
        /// Exclui um pedido pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do pedido a ser excluído.</param>
        /// <response code="204">Pedido excluído com sucesso</response>
        /// <response code="400">Erro de validação</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pedidoService.DeleteAsync(id);

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