using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;

namespace XptoAPI.src.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly XptoContext _context;

        // Configure horários permitidos para cada tipo de refeição
        private static readonly TimeSpan CafeDaManhaInicio = new(6, 0, 0);
        private static readonly TimeSpan CafeDaManhaFim = new(10, 30, 0);
        private static readonly TimeSpan AlmocoInicio = new(11, 30, 0);
        private static readonly TimeSpan AlmocoFim = new(14, 30, 0);

        public MenuItemService(XptoContext context)
        {
            _context = context;
        }

        private bool HorarioPermitido(TipoRefeicao tipoRefeicao)
        {
            var horaAtual = DateTime.Now.TimeOfDay;

            return tipoRefeicao switch
            {
                TipoRefeicao.CafedaManha => horaAtual >= CafeDaManhaInicio && horaAtual <= CafeDaManhaFim,
                TipoRefeicao.Almoco => horaAtual >= AlmocoInicio && horaAtual <= AlmocoFim,
                _ => false
            };
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
            if (!HorarioPermitido(menuItem.TipoRefeicao))
            {
                throw new InvalidOperationException($"Não é possível criar itens do tipo {menuItem.TipoRefeicao} fora do horário permitido.");
            }

            _context.MenuItems!.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            if (!HorarioPermitido(menuItem.TipoRefeicao))
            {
                throw new InvalidOperationException($"Não é possível atualizar itens do tipo {menuItem.TipoRefeicao} fora do horário permitido.");
            }

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