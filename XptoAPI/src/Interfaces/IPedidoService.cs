using XptoAPI.src.Models;

namespace XptoAPI.src.Interfaces
{
    public interface IPedidoService
    {
    Task<IEnumerable<Pedido>> GetPedidosCozinhaAsync();
    Task UpdateStatusAsync(int id, StatusPedido status);
    }
}
