using Microsoft.AspNetCore.Mvc;
using XptoAPI.src.DTOs;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using XptoAPI.src.Services;

namespace XptoAPI.src.Controllers
{
    /// <summary>
    /// Controlador para gerenciar itens do menu.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        /// <summary>
        /// Inicializa uma nova instância do controlador de itens do menu.
        /// </summary>
        /// <param name="menuItemService">Serviço para interação com os itens do menu.</param>
        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        /// <summary>
        /// Retorna uma lista paginada de itens do menu.
        /// </summary>
        /// <param name="pageNumber">Número da página (default: 1)</param>
        /// <param name="pageSize">Quantidade de itens por página (default: 10)</param>
        /// <returns>Lista paginada de itens do menu</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<MenuItem>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be positive numbers.");
            }

            var pagedResult = await _menuItemService.GetPaginatedAsync(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        /// <summary>
        /// Retorna um item de menu pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do item de menu.</param>
        /// <response code="200">Retorna o item de menu encontrado</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetById(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        /// <summary>
        /// Retorna itens de menu filtrados por tipo de refeição.
        /// </summary>
        /// <param name="tipo">Tipo de refeição (ex: CafedaManha, Almoco, Jantar).</param>
        /// <response code="200">Retorna a lista de itens do tipo especificado</response>
        /// <response code="404">Caso não encontre nenhum item do tipo informado</response>
        [HttpGet("tiporefeicao/{tipo}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetByTipoRefeicao(TipoRefeicao tipo)
        {
            var items = await _menuItemService.GetByTipoRefeicaoAsync(tipo);
            if (!items.Any())
            {
                return NotFound();
            }
            return Ok(items);
        }

        /// <summary>
        /// Cria um novo item de menu.
        /// </summary>
        /// <param name="menuItem">Objeto contendo as informações do item de menu.</param>
        /// <response code="201">Retorna o item de menu criado</response>
        /// <response code="400">Erro de validação ou horário não permitido</response>
        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItem menuItem)
        {
            try
            {
                var created = await _menuItemService.CreateAsync(menuItem);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um item de menu existente.
        /// </summary>
        /// <param name="id">Identificador do item de menu.</param>
        /// <param name="menuItem">Objeto contendo os novos dados de item de menu.</param>
        /// <response code="204">Confirma atualização sem retornar conteúdo</response>
        /// <response code="400">Erro de validação ou horário não permitido</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return BadRequest();
            }

            try
            {
                await _menuItemService.UpdateAsync(menuItem);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Exclui um item de menu pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do item de menu.</param>
        /// <response code="204">Confirma exclusão do item de menu</response>
        /// <response code="404">Item não encontrado</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}