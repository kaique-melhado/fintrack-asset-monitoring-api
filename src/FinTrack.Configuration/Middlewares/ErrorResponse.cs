namespace FinTrack.Configuration.Middlewares;

/// <summary>
/// Representa a estrutura padrão de resposta de erro utilizada pela API.
/// Essa estrutura é útil para padronizar retornos em casos de falhas durante o processamento de requisições.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Código de status HTTP retornado pela API (ex: 400, 404, 500).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Mensagem descritiva do erro ocorrido, podendo ser técnica ou orientada ao usuário.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora (em UTC) do momento em que o erro foi gerado pela API.
    /// </summary>
    public DateTime Timestamp { get; set; }
}