using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces;
using FinTrack.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.UseCases.Products.RegisterProduct;

/// <summary>
/// Handler responsável por processar o comando de registro de produto.
/// </summary>
public sealed class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, RegisterProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterProductCommandHandler> _logger;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="RegisterProductCommandHandler"/>.
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho que expõe os repositórios e gerencia a transação.</param>
    /// <param name="logger">Instância de logger para registrar informações durante o processamento.</param>
    public RegisterProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RegisterProductCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(logger);

        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Manipula o comando <see cref="RegisterProductCommand"/> para registrar um novo produto no sistema.
    /// </summary>
    /// <param name="request">Comando contendo os dados do produto a ser registrado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Retorna os dados do produto registrado.</returns>
    public async Task<RegisterProductResponse> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando registro de produto: {Name} ({Ticker})", request.Name, request.Ticker);
        _logger.LogDebug("Payload recebido: {@Request}", request);

        try
        {
            var existingProduct = await _unitOfWork.Products.GetByTickerAsync(request.Ticker, cancellationToken);
            if (existingProduct is not null)
            {
                _logger.LogWarning("Tentativa de registro com ticker duplicado: {Ticker}", request.Ticker);
                throw new ApplicationException($"Já existe um produto registrado com o ticker '{request.Ticker}'.");
            }

            var currency = new Currency(request.CurrencyCode);

            var product = new Product(
                request.Name,
                request.Ticker,
                request.Type,
                request.Category,
                currency
            );

            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Produto registrado com sucesso. ID: {ProductId}", product.Id);

            return new RegisterProductResponse(product.Id, product.Name, product.Ticker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar produto: {Name} ({Ticker})", request.Name, request.Ticker);
            throw;
        }
    }
}