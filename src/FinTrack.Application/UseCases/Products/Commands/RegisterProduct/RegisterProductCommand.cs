using FinTrack.Domain.Enums;
using MediatR;

namespace FinTrack.Application.UseCases.Products.Commands.RegisterProduct;

/// <summary>
/// Comando para registrar um novo produto no sistema.
/// </summary>
public sealed record RegisterProductCommand(
    string Name,
    string Ticker,
    ProductType Type,
    ProductCategory Category,
    string CurrencyCode
) : IRequest<RegisterProductResponse>;