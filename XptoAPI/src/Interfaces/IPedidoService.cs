using ErrorOr;
using XptoAPI.src.Models;

namespace XptoAPI.src.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetPedidosCozinhaAsync();
        Task<ErrorOr<Updated>> UpdateStatusAsync(int id, StatusPedido status);
        Task<ErrorOr<Pedido>> CreatePedidoAsync(Pedido pedido);
        Task<ErrorOr<Pedido>> GetByIdAsync(int id);
    }
}
