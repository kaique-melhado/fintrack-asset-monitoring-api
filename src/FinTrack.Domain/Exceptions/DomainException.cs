namespace FinTrack.Domain.Exceptions;

/// <summary>
/// Exceção utilizada para regras de negócio no domínio.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="DomainException"/>.
    /// </summary>
    /// <param name="message">Mensagem descrevendo o erro de negócio.</param>
    public DomainException(string message) : base(message) { }
}