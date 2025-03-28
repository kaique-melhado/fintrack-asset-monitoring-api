using FluentValidation;

namespace FinTrack.Application.UseCases.Products.RegisterProduct;

/// <summary>
/// Validador para garantir que os dados do comando de registro de produto estejam corretos.
/// </summary>
public sealed class RegisterProductCommandValidator : AbstractValidator<RegisterProductCommand>
{
    /// <summary>
    /// Inicializa uma nova instância do <see cref="RegisterProductCommandValidator"/> 
    /// aplicando as regras de validação para o comando de registro de produto.
    /// </summary>
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Ticker)
            .NotEmpty().WithMessage("O ticker é obrigatório.")
            .MaximumLength(20).WithMessage("O ticker deve ter no máximo 20 caracteres.")
            .Matches("^[A-Za-z0-9]+$").WithMessage("O ticker deve conter apenas letras e números, sem espaços ou caracteres especiais.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("O código da moeda é obrigatório.")
            .Length(3).WithMessage("O código da moeda deve conter 3 letras.")
            .Matches("^[A-Za-z]{3}$").WithMessage("O código da moeda deve conter apenas letras.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de produto inválido.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Categoria de produto inválida.");
    }
}