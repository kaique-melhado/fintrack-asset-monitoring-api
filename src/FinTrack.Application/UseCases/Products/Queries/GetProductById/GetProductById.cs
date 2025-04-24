using FinTrack.Application.UseCases.Products.Queries.Responses;
using MediatR;

namespace FinTrack.Application.UseCases.Products.Queries.GetProductById;

/// <summary>
/// Consulta para buscar um produto por ID.
/// </summary>
/// <param name="Id">Identificador único do produto.</param>
public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailsResponse>;