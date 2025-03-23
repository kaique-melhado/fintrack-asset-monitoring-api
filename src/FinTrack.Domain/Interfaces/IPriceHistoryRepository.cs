using FinTrack.Domain.Entities;

namespace FinTrack.Domain.Interfaces;

/// <summary>
/// Define o contrato para persistência e consulta de histórico de preços.
/// </summary>
public interface IPriceHistoryRepository
{
    /// <summary>Obtém todo o histórico de preços de um determinado produto.</summary>
    Task<IEnumerable<PriceHistory>> GetByProductIdAsync(Guid productId);

    /// <summary>Adiciona um novo registro de histórico de preços.</summary>
    Task AddAsync(PriceHistory history);
}