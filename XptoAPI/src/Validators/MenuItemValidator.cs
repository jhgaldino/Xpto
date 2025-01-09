using FluentValidation;
using XptoAPI.src.Models;
namespace XptoAPI.src.Validators
{
    public class MenuItemValidator : AbstractValidator<MenuItem>
    {
        public MenuItemValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome do item é obrigatório")
                .MaximumLength(100)
                .WithMessage("Nome não pode ter mais que 100 caracteres");

            RuleFor(x => x.Tipo)
                .IsInEnum()
                .WithMessage("Tipo de item inválido");

            RuleFor(x => x.TipoRefeicao)
                .IsInEnum()
                .WithMessage("Tipo de refeição inválido");
        }
    }
}
