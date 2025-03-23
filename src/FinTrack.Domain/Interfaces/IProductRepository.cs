using FinTrack.Domain.Entities;

namespace FinTrack.Domain.Interfaces;

/// <summary>
/// Define o contrato para persistência e consulta de produtos financeiros.
/// </summary>
public interface IProductRepository
{
    /// <summary>Obtém um produto pelo ID.</summary>
    Task<Product?> GetByIdAsync(Guid id);

    /// <summary>Obtém um produto pelo seu código de ação.</summary>
    Task<Product?> GetByTickerAsync(string ticker);

    /// <summary>Retorna todos os produtos.</summary>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>Adiciona um novo produto.</summary>
    Task AddAsync(Product product);

    /// <summary>Atualiza um produto existente.</summary>
    Task UpdateAsync(Product product);
}

