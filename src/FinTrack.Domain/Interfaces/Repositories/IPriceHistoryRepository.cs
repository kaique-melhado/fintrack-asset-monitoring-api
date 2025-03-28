using FinTrack.Domain.Entities;

namespace FinTrack.Domain.Interfaces.Repositories;

/// <summary>
/// Define o contrato para persistência e consulta de histórico de preços.
/// </summary>
public interface IPriceHistoryRepository
{
    /// <summary>
    /// Obtém todos os registros de histórico de preços de um determinado produto.
    /// </summary>
    /// <param name="productId">ID do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task<IEnumerable<PriceHistory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona um novo registro de histórico de preços.
    /// </summary>
    /// <param name="history">Instância do histórico de preços a ser adicionada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task AddAsync(PriceHistory history, CancellationToken cancellationToken);
}