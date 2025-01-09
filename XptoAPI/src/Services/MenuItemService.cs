using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;

namespace XptoAPI.src.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly XptoContext _context;

        public MenuItemService(XptoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems!.ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems!.FindAsync(id);
        }

        public async Task<IEnumerable<MenuItem>> GetByTipoRefeicaoAsync(TipoRefeicao tipoRefeicao)
        {
            return await _context.MenuItems!
                .Where(m => m.TipoRefeicao == tipoRefeicao)
                .ToListAsync();
        }

        public async Task<MenuItem> CreateAsync(MenuItem menuItem)
        {
            _context.MenuItems!.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.Entry(menuItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var menuItem = await GetByIdAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems!.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}