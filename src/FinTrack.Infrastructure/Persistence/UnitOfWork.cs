using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces;
using FinTrack.Domain.Interfaces.Repositories;
using FinTrack.Infrastructure.Persistence.Context;
using FinTrack.Infrastructure.Persistence.Repositories;

namespace FinTrack.Infrastructure.Persistence;

/// <summary>
/// Implementa a unidade de trabalho utilizando Entity Framework Core.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FinTrackDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">Contexto de banco de dados do FinTrack.</param>
    public UnitOfWork(FinTrackDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Products = new ProductRepository(_context);
    }

    /// <inheritdoc />
    public IProductRepository Products { get; }

    /// <inheritdoc />
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}