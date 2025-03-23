using FinTrack.Domain.ValueObjects;

namespace FinTrack.UnitTests.Domain.Fixtures;

/// <summary>
/// Fornece instâncias de moedas válidas para testes.
/// </summary>
public static class CurrencyFixture
{
    private static readonly string[] ValidCodes = { "BRL", "USD", "EUR", "JPY", "GBP" };
    private static readonly Random Randomizer = new();

    /// <summary>
    /// Retorna uma moeda fixa (BRL).
    /// </summary>
    public static Currency Default() => new("BRL");

    /// <summary>
    /// Retorna uma moeda aleatória válida para testes.
    /// </summary>
    public static Currency Random()
    {
        var code = ValidCodes[Randomizer.Next(ValidCodes.Length)];
        return new Currency(code);
    }

    /// <summary>
    /// Retorna uma moeda com o código especificado.
    /// </summary>
    /// <param name="code">Código da moeda desejada.</param>
    public static Currency WithCode(string code) => new(code);
}