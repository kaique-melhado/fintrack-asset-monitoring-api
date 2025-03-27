using FinTrack.Domain.Entities;

namespace FinTrack.Domain.Interfaces.Repositories;

/// <summary>
/// Define o contrato para persistência e consulta de produtos.
/// </summary>
public interface IProductRepository
{
    /// <summary>Obtém um produto pelo ID.</summary>
    /// <param name="id">Identificador do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Obtém um produto pelo ticker do produto.</summary>
    /// <param name="ticker">Ticker do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task<Product?> GetByTickerAsync(string ticker, CancellationToken cancellationToken);

    /// <summary>Retorna todos os produtos.</summary>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Adiciona um novo produto ao repositório.</summary>
    /// <param name="product">Produto a ser adicionado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task AddAsync(Product product, CancellationToken cancellationToken);

    /// <summary>Atualiza um produto existente no repositório.</summary>
    /// <param name="product">Produto a ser atualizado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}