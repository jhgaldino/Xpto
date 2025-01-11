using FluentValidation;
using XptoAPI.src.Models;

namespace XptoAPI.src.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("ID do usuário é obrigatório");

            RuleFor(x => x.DataHoraPedido)
                .NotEmpty()
                .WithMessage("Data/hora do pedido é obrigatória");


            RuleFor(x => x.TipoRefeicao)
                .IsInEnum()
                .WithMessage("Tipo de refeição inválido");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido");

            RuleFor(x => x.Itens)
                .NotEmpty()
                .WithMessage("Pedido deve conter pelo menos um item")
                .Must(itens => itens.Count <= 10)
                .WithMessage("Pedido não pode ter mais que 10 itens");
        }
    }
}
