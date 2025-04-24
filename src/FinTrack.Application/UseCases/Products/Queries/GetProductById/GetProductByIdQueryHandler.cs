using FinTrack.Application.UseCases.Products.Queries.GetProductById;
using FinTrack.Application.UseCases.Products.Queries.Responses;
using FinTrack.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.UseCases.Products.Queries;

/// <summary>
/// Handler responsável por processar a consulta de produto por ID.
/// </summary>
public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="GetProductByIdQueryHandler"/>.
    /// </summary>
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ProductDetailsResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando consulta de produto por ID: {ProductId}", request.Id);

        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            _logger.LogWarning("Produto não encontrado para o ID: {ProductId}", request.Id);
            throw new KeyNotFoundException("Produto não encontrado.");
        }

        return new ProductDetailsResponse(
            product.Id,
            product.Name,
            product.Ticker,
            product.Type.ToString(),
            product.Category.ToString(),
            product.Currency.Code
        );
    }
}