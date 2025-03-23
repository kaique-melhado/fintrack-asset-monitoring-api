using FinTrack.Domain.Exceptions;

namespace FinTrack.Domain.ValueObjects;

/// <summary>
/// Representa um código de moeda conforme padrão ISO (ex: BRL, USD).
/// </summary>
public class Currency
{
    /// <summary>Código da moeda em 3 letras.</summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Construtor utilizado apenas por frameworks de persistência.
    /// </summary>
    protected Currency() { }

    /// <summary>
    /// Cria uma nova instância de moeda.
    /// </summary>
    /// <param name="code">Código da moeda (ex: BRL).</param>
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            throw new DomainException("Informe um código de moeda válido com 3 letras.");

        Code = code.ToUpper();
    }

    /// <summary>
    /// Retorna o código da moeda como string.
    /// </summary>
    public override string ToString() => Code;

    /// <summary>
    /// Compara duas instâncias de moeda pelo código.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is not Currency other) return false;
        return Code == other.Code;
    }

    /// <summary>
    /// Retorna o hash code baseado no código da moeda.
    /// </summary>
    public override int GetHashCode() => Code.GetHashCode();
}