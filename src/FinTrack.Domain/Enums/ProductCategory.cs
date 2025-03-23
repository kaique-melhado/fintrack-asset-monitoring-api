namespace FinTrack.Domain.Enums;

/// <summary>
/// Categorias financeiras dos produtos com base em perfil de risco e retorno.
/// </summary>
public enum ProductCategory
{
    /// <summary>Produtos de renda variável, como ações e criptos.</summary>
    VariableIncome = 1,

    /// <summary>Produtos com rendimento previsível, como títulos do governo.</summary>
    FixedIncome = 2,

    /// <summary>Fundos que misturam diversos tipos de ativos.</summary>
    Multimarket = 3,

    /// <summary>Produtos vinculados a moedas estrangeiras.</summary>
    Currency = 4
}