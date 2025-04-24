namespace FinTrack.Application.UseCases.Products.Queries.Responses;

/// <summary>
/// DTO de leitura com os detalhes de um produto.
/// </summary>
public sealed record ProductDetailsResponse(
    Guid Id,
    string Name,
    string Ticker,
    string Type,
    string Category,
    string CurrencyCode
);