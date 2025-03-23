namespace Produto.Application.Validators;

using FluentValidation;

using Produto.Application.Dtos;

internal class ProdutoValidator : AbstractValidator<ProdutoDto>
{
    public ProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório")
            .Length(3, 100)
            .WithMessage("O nome do produto deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição do produto deve ter no máximo 500 caracteres");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("O valor do produto deve ser maior que zero")
            .PrecisionScale(16, 2, true)
            .WithMessage("O valor do produto deve ter no máximo 2 casas decimais e 16 dígitos no total");

        RuleFor(x => x.Estoque)
          .GreaterThanOrEqualTo(0)
          .WithMessage("O valor do produto deve ser maior ou igual a zero");
    }
}