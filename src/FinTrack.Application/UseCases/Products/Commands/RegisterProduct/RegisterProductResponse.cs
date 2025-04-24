namespace FinTrack.Application.UseCases.Products.Commands.RegisterProduct;

/// <summary>
/// Representa a resposta do caso de uso de registro de produto.
/// </summary>
public sealed record RegisterProductResponse(Guid Id, string Name, string Ticker);