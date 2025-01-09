using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;

namespace XptoAPI.src.Services
{
    public class PedidoService : IPedidoService
{
    private readonly XptoContext _context;

    public PedidoService(XptoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetPedidosCozinhaAsync()
    {
        return await _context.Pedidos!
            .Where(p => p.Status == StatusPedido.Recebido || p.Status == StatusPedido.EmPreparacao)
            .OrderBy(p => p.DataHoraPedido)  // Ordenação por hora do pedido
            .Include(p => p.Itens)
            .ToListAsync();
    }

    public async Task UpdateStatusAsync(int id, StatusPedido novoStatus)
    {
        var pedido = await _context.Pedidos!.FindAsync(id);
        if (pedido == null)
            throw new InvalidOperationException("Pedido não encontrado");

        // Validar transições de status
        if (!IsValidStatusTransition(pedido.Status, novoStatus))
            throw new InvalidOperationException("Transição de status inválida");

        pedido.Status = novoStatus;
        if (novoStatus == StatusPedido.Pronto)
            pedido.PedidoCompleto = true;

        await _context.SaveChangesAsync();
    }

    private bool IsValidStatusTransition(StatusPedido statusAtual, StatusPedido novoStatus)
    {
        return statusAtual switch
        {
            StatusPedido.Recebido => novoStatus == StatusPedido.EmPreparacao || novoStatus == StatusPedido.Cancelado,
            StatusPedido.EmPreparacao => novoStatus == StatusPedido.Pronto || novoStatus == StatusPedido.Cancelado,
            StatusPedido.Pronto => novoStatus == StatusPedido.Entregue,
            _ => false
        };
    }
}
}
