using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração da entidade <see cref="PriceHistory"/> para o Entity Framework.
/// </summary>
public sealed class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    /// <summary>
    /// Configura o mapeamento da entidade <see cref="PriceHistory"/> com a tabela no banco de dados.
    /// </summary>
    /// <param name="builder">Construtor de tipo da entidade.</param>
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.ToTable("PriceHistories");

        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Id)
            .IsRequired();

        builder.Property(ph => ph.ProductId)
            .IsRequired();

        builder.Property(ph => ph.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(ph => ph.Date)
            .IsRequired();

        builder.Property(ph => ph.Source)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(ph => ph.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}