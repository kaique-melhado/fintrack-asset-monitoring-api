using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Repositories;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repositório responsável por acessar e manipular dados da entidade <see cref="PriceHistory"/> no banco de dados.
/// </summary>
public sealed class PriceHistoryRepository : IPriceHistoryRepository
{
    private readonly FinTrackDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="PriceHistoryRepository"/>.
    /// </summary>
    /// <param name="context">Contexto de banco de dados do EF Core.</param>
    public PriceHistoryRepository(FinTrackDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PriceHistory>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.PriceHistories
            .AsNoTracking()
            .Where(ph => ph.ProductId == productId)
            .OrderByDescending(ph => ph.Date)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(PriceHistory history, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(history);

        await _context.PriceHistories.AddAsync(history, cancellationToken);
    }
}