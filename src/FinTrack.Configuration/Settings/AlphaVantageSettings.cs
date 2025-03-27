namespace FinTrack.Configuration.Settings;

/// <summary>
/// Representa as configurações para o cliente Alpha Vantage.
/// </summary>
public class AlphaVantageSettings
{
    /// <summary>
    /// Chave de API utilizada para autenticação nas requisições ao Alpha Vantage.
    /// </summary>
    public string ApiKey { get; set; } = default!;

    /// <summary>
    /// URL base para as requisições à API do Alpha Vantage.
    /// </summary>
    public string BaseUrl { get; set; } = default!;
}