using FluentValidation;
using MediatR;

namespace FinTrack.Application.Behaviors;

/// <summary>
/// Comportamento do MediatR que executa validações do FluentValidation antes de processar um comando.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ValidationBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="validators">Lista de validadores do FluentValidation.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
                throw new ValidationException(failures);
        }

        return await next();
    }
}