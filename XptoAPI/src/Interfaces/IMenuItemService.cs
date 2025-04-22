using XptoAPI.src.Models;
using XptoAPI.src.DTOs;

namespace XptoAPI.src.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<IEnumerable<MenuItem>> GetByTipoRefeicaoAsync(TipoRefeicao tipoRefeicao);
        Task<MenuItem> CreateAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(int id);
        Task<PaginatedList<MenuItem>> GetPaginatedAsync(int pageNumber, int pageSize);
    }
}