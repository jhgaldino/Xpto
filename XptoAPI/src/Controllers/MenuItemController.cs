using Microsoft.AspNetCore.Mvc;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using XptoAPI.src.Services;

namespace XptoAPI.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetAll()
        {
            var items = await _menuItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetById(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();
                
            return Ok(item);
        }

        [HttpGet("tiporefeicao/{tipo}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetByTipoRefeicao(TipoRefeicao tipo)
        {
            var items = await _menuItemService.GetByTipoRefeicaoAsync(tipo);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> Create(MenuItem menuItem)
        {
            var created = await _menuItemService.CreateAsync(menuItem);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MenuItem menuItem)
        {
            if (id != menuItem.Id)
                return BadRequest();

            await _menuItemService.UpdateAsync(menuItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}