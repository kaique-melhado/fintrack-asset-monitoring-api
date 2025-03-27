using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence.Context;

/// <summary>
/// Representa o contexto do banco de dados principal da aplicação.
/// </summary>
public class FinTrackDbContext : DbContext
{
    /// <summary>
    /// Inicializa uma nova instância do <see cref="FinTrackDbContext"/>.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Conjunto de dados de produtos.
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>
    /// Conjunto de dados de históricos de preço.
    /// </summary>
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    /// <summary>
    /// Configura o modelo das entidades.
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinTrackDbContext).Assembly);
    }
}