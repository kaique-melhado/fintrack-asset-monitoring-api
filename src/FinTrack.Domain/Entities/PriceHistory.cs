using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Registro histórico de preço de um determinado produto.
/// </summary>
public class PriceHistory
{
    /// <summary>Identificador único do histórico.</summary>
    public Guid Id { get; private set; }

    /// <summary>ID do produto associado.</summary>
    public Guid ProductId { get; private set; }

    /// <summary>Data do registro do preço.</summary>
    public DateTime Date { get; private set; }

    /// <summary>Valor do preço registrado.</summary>
    public decimal Price { get; private set; }

    /// <summary>Fonte de onde o preço foi obtido (ex: AlphaVantage).</summary>
    public string Source { get; private set; } = default!;

    /// <summary>
    /// Construtor utilizado apenas por frameworks de persistência.
    /// </summary>
    protected PriceHistory() { }

    /// <summary>
    /// Cria uma nova entrada de histórico de preço.
    /// </summary>
    public PriceHistory(Guid productId, decimal price, DateTime date, string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("A fonte do histórico de preço deve ser informada.", nameof(source));

        if (price <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.", nameof(price));

        if (date > DateTime.UtcNow)
            throw new ArgumentException("A data do histórico não pode ser no futuro.", nameof(date));

        Id = Guid.NewGuid();
        ProductId = productId;
        Price = price;
        Date = date;
        Source = source;
    }
}