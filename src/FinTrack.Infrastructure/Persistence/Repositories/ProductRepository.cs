using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Repositories;
using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repositório responsável por acessar e manipular dados da entidade <see cref="Product"/> no banco de dados.
/// </summary>
public sealed class ProductRepository : IProductRepository
{
    private readonly FinTrackDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ProductRepository"/>.
    /// </summary>
    /// <param name="context">Contexto de banco de dados do EF Core.</param>
    public ProductRepository(FinTrackDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Product?> GetByTickerAsync(string ticker, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Ticker == ticker, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        await _context.Products.AddAsync(product, cancellationToken);
    }

    /// <inheritdoc />
    public Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        _context.Products.Update(product);
        return Task.CompletedTask;
    }
}