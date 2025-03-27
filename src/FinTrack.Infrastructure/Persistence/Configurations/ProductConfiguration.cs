using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configurações da entidade <see cref="Product"/> para o EF Core.
/// Define mapeamentos de propriedades, restrições e relacionamentos.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configura a entidade <see cref="Product"/> no modelo do EF Core.
    /// </summary>
    /// <param name="builder">Construtor usado para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.Category)
            .IsRequired();

        builder.Property(p => p.CurrentPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.OwnsOne(p => p.Currency, cb =>
        {
            cb.Property(c => c.Code)
              .IsRequired()
              .HasColumnName("CurrencyCode")
              .HasMaxLength(3);
        });
    }
}