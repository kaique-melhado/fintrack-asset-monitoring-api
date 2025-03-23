namespace FinTrack.Domain.Enums;

/// <summary>
/// Tipos disponíveis de produtos financeiros.
/// </summary>
public enum ProductType
{
    /// <summary>Ações listadas em bolsa.</summary>
    Stock = 1,

    /// <summary>Títulos de renda fixa.</summary>
    FixedIncome = 2,

    /// <summary>Fundos de índice negociados em bolsa.</summary>
    ETF = 3,

    /// <summary>Fundos de investimento em geral.</summary>
    Fund = 4,

    /// <summary>Criptomoedas como Bitcoin ou Ethereum.</summary>
    Crypto = 5
}