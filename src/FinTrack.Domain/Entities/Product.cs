using FinTrack.Domain.Enums;
using FinTrack.Domain.Exceptions;
using FinTrack.Domain.ValueObjects;

namespace FinTrack.Domain.Entities;

/// <summary>
/// Representa um ativo financeiro, como ações, fundos ou criptomoedas.
/// </summary>
public class Product
{
    /// <summary>Identificador único do produto.</summary>
    public Guid Id { get; private set; }

    /// <summary>Nome do produto (ex: Petrobras PN).</summary>
    public string Name { get; private set; } = default!;

    /// <summary>Código do ticker (ex: PETR4).</summary>
    public string Ticker { get; private set; } = default!;

    /// <summary>Tipo do produto (ação, fundo, cripto, etc).</summary>
    public ProductType Type { get; private set; }

    /// <summary>Categoria de investimento atribuída ao produto.</summary>
    public ProductCategory Category { get; private set; }

    /// <summary>Moeda associada ao produto (ex: BRL, USD).</summary>
    public Currency Currency { get; private set; } = default!;

    /// <summary>Preço atual conhecido do produto.</summary>
    public decimal CurrentPrice { get; private set; }

    /// <summary>Data de cadastro do produto.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Construtor utilizado apenas por frameworks de persistência.
    /// </summary>
    protected Product() { }

    /// <summary>
    /// Cria uma nova instância de produto com validações de negócio.
    /// </summary>
    public Product(string name, string ticker, ProductType type, ProductCategory category, Currency currency)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(ticker))
            throw new DomainException("Ticker do produto é obrigatório.");

        if (currency is null)
            throw new DomainException("Moeda do produto é obrigatória.");

        if (!Enum.IsDefined(typeof(ProductType), type))
            throw new DomainException("Tipo de produto inválido.");

        if (!Enum.IsDefined(typeof(ProductCategory), category))
            throw new DomainException("Categoria de produto inválida.");

        Id = Guid.NewGuid();
        Name = name;
        Ticker = ticker;
        Type = type;
        Category = category;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
        CurrentPrice = 0m;
    }

    /// <summary>
    /// Atualiza o preço atual do produto.
    /// </summary>
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("O preço deve ser maior que zero.");

        CurrentPrice = newPrice;
    }
}