﻿using FluentValidation;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using XptoAPI.src.Data;
using XptoAPI.src.Interfaces;
using XptoAPI.src.Models;
using XptoAPI.src.Common.Errors;
using XptoAPI.src.Common.Validators;

namespace XptoAPI.src.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly XptoContext _context;
        private readonly IValidator<Pedido> _validator;

        public PedidoService(XptoContext context, IValidator<Pedido> validator)
        {
            _context = context;
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Pedido>> GetPedidosCozinhaAsync()
        {
            var pedidosAtivos = await _context.Pedidos!
                .Where(p => p.Status == StatusPedido.Recebido ||
                            p.Status == StatusPedido.EmPreparacao) // Remove pedido pronto somente da exibicao, nao do banco
                .OrderBy(p => p.DataHoraPedido)
                .Include(p => p.Itens)
                .ToListAsync();

            foreach (var pedido in pedidosAtivos)
            {
                pedido.Itens = pedido.Itens
                    .OrderBy(item => GetOrdemTipoItem(item.Tipo))
                    .ToList();
            }

            return pedidosAtivos;
        }

        private static int GetOrdemTipoItem(TipodeItemMenu tipo)
        {
            return tipo switch
            {
                TipodeItemMenu.Bebida => 1,
                TipodeItemMenu.Acompanhamento => 2,
                TipodeItemMenu.PratoPrincipal => 3,
                TipodeItemMenu.Sobremesa => 4,
                _ => 99
            };
        }

        public async Task<ErrorOr<Pedido>> CreatePedidoAsync(Pedido pedido)
        {
            var validationResult = await _validator.ValidateAsync(pedido);

            if (!validationResult.IsValid)
            {
                return Error.Validation("Pedido.ValidationError",
                    string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            // Alteração: usar o novo método ValidarHorario
            var (isHorarioValido, mensagem) = DataHoraValidator.ValidarHorario(pedido.TipoRefeicao, pedido.DataHoraPedido);
            if (!isHorarioValido)
            {
                return Error.Validation("Pedido.HorarioNaoPermitido", mensagem);
            }

            // Validar  o horário de funcionamento
            var (isFuncionamentoValido, mensagemFuncionamento) = DataHoraValidator.ValidarHorarioFuncionamento(pedido.DataHoraPedido);
            if (!isFuncionamentoValido)
            {
                return Error.Validation("Pedido.HorarioNaoPermitido", mensagemFuncionamento);
            }

            // Buscar os itens existentes do banco
            var itemIds = pedido.Itens.Select(i => i.Id).ToList();
            var itensExistentes = await _context.MenuItems!
                .Where(m => itemIds.Contains(m.Id))
                .ToListAsync();

            // Substituir os itens do pedido pelos existentes
            pedido.Itens = itensExistentes;

            _context.Pedidos!.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<ErrorOr<Updated>> UpdateStatusAsync(int id, StatusPedido status)
        {
            var pedido = await _context.Pedidos!.FindAsync(id);

            if (pedido is null)
            {
                return await Task.FromResult<ErrorOr<Updated>>(Errors.Pedido.PedidoNaoEncontrado);
            }

            if (!IsValidStatusTransition(pedido.Status, status))
            {
                return await Task.FromResult<ErrorOr<Updated>>(Errors.Pedido.TransicaoStatusInvalida);
            }

            pedido.Status = status;
            // Atualiza pedidoCompleto quando o status é Pronto
            if (status == StatusPedido.Pronto)
            {
                pedido.PedidoCompleto = true;
            }

            await _context.SaveChangesAsync();
            return await Task.FromResult<ErrorOr<Updated>>(Result.Updated);
        }

        public async Task<ErrorOr<Pedido>> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos!.FindAsync(id);
            if (pedido is null)
            {
                return Error.NotFound("Pedido.NotFound", "Pedido não encontrado");
            }
            return pedido;
        }

        public async Task<ErrorOr<Updated>> DeleteAsync(int id)
        {
            var pedido = await _context.Pedidos!.FindAsync(id);
            if (pedido is null)
            {
                return await Task.FromResult<ErrorOr<Updated>>(Errors.Pedido.PedidoNaoEncontrado);
            }

            _context.Pedidos!.Remove(pedido);
            await _context.SaveChangesAsync();
            return await Task.FromResult<ErrorOr<Updated>>(Result.Updated);
        }

        private static bool IsValidStatusTransition(StatusPedido statusAtual, StatusPedido novoStatus)
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
