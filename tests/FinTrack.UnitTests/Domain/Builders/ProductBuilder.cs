using FinTrack.Domain.Entities;
using FinTrack.Domain.Enums;
using FinTrack.Domain.ValueObjects;
using FinTrack.UnitTests.Domain.Fixtures;

namespace FinTrack.UnitTests.Domain.Builders;

/// <summary>
/// Construtor para facilitar a criação de instâncias de Product em testes.
/// Permite configurar cenários positivos e negativos de forma legível e reutilizável.
/// </summary>
public class ProductBuilder
{
    private const string DefaultName = "Ação Petrobras";
    private const string DefaultTicker = "PETR4";
    private const ProductType DefaultType = ProductType.Stock;
    private const ProductCategory DefaultCategory = ProductCategory.VariableIncome;
    private static readonly Currency DefaultCurrency = CurrencyFixture.Default();

    private string _name = DefaultName;
    private string _ticker = DefaultTicker;
    private ProductType _type = DefaultType;
    private ProductCategory _category = DefaultCategory;
    private Currency _currency = DefaultCurrency;

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithInvalidName()
    {
        _name = "   ";
        return this;
    }

    public ProductBuilder WithTicker(string ticker)
    {
        _ticker = ticker;
        return this;
    }

    public ProductBuilder WithInvalidTicker()
    {
        _ticker = "   ";
        return this;
    }

    public ProductBuilder WithType(ProductType type)
    {
        _type = type;
        return this;
    }

    public ProductBuilder WithInvalidType()
    {
        _type = (ProductType)999;
        return this;
    }

    public ProductBuilder WithCategory(ProductCategory category)
    {
        _category = category;
        return this;
    }

    public ProductBuilder WithInvalidCategory()
    {
        _category = (ProductCategory)999;
        return this;
    }

    public ProductBuilder WithCurrency(Currency currency)
    {
        _currency = currency;
        return this;
    }

    public ProductBuilder WithInvalidCurrency()
    {
        _currency = null!;
        return this;
    }

    public ProductBuilder WithDefaults()
    {
        _name = DefaultName;
        _ticker = DefaultTicker;
        _type = DefaultType;
        _category = DefaultCategory;
        _currency = DefaultCurrency;
        return this;
    }

    public Product Build()
    {
        return new Product(_name, _ticker, _type, _category, _currency);
    }
}