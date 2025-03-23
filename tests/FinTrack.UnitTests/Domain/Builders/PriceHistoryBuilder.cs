using FinTrack.Domain.Entities;

namespace FinTrack.UnitTests.Domain.Builders;

/// <summary>
/// Construtor para facilitar a criação de instâncias de PriceHistory em testes.
/// Permite configurar campos como preço, data, fonte e produto.
/// </summary>
public class PriceHistoryBuilder
{
    private Guid _productId = new ProductBuilder().Build().Id;
    private decimal _price = 100m;
    private DateTime _date = DateTime.UtcNow;
    private string _source = "AlphaVantage";

    /// <summary>
    /// Define o ID do produto associado ao histórico.
    /// </summary>
    public PriceHistoryBuilder WithProductId(Guid productId)
    {
        _productId = productId;
        return this;
    }

    /// <summary>
    /// Define o preço associado ao histórico.
    /// </summary>
    public PriceHistoryBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    /// <summary>
    /// Define uma data específica para o histórico.
    /// </summary>
    public PriceHistoryBuilder WithDate(DateTime date)
    {
        _date = date;
        return this;
    }

    /// <summary>
    /// Define a fonte de origem do dado de preço (AlphaVantage, YahooFinance).
    /// </summary>
    public PriceHistoryBuilder WithSource(string source)
    {
        _source = source;
        return this;
    }

    /// <summary>
    /// Define um preço inválido para testes negativos.
    /// </summary>
    public PriceHistoryBuilder WithInvalidPrice()
    {
        _price = -1;
        return this;
    }

    /// <summary>
    /// Define uma data no futuro para validar restrições de data.
    /// </summary>
    public PriceHistoryBuilder WithFutureDate(int daysAhead = 1)
    {
        _date = DateTime.UtcNow.AddDays(daysAhead);
        return this;
    }

    /// <summary>
    /// Define uma fonte nula ou vazia para testes negativos.
    /// </summary>
    public PriceHistoryBuilder WithInvalidSource()
    {
        _source = " ";
        return this;
    }

    /// <summary>
    /// Define um ProductId inválido (Guid.Empty).
    /// </summary>
    public PriceHistoryBuilder WithInvalidProductId()
    {
        _productId = Guid.Empty;
        return this;
    }

    /// <summary>
    /// Reseta os valores padrão.
    /// </summary>
    public PriceHistoryBuilder WithDefaults()
    {
        _productId = new ProductBuilder().Build().Id;
        _price = 100m;
        _date = DateTime.UtcNow;
        _source = "AlphaVantage";
        return this;
    }

    /// <summary>
    /// Cria uma nova instância de PriceHistory com os valores definidos.
    /// </summary>
    public PriceHistory Build()
    {
        return new PriceHistory(_productId, _price, _date, _source);
    }
}