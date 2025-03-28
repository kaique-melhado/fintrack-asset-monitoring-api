using FinTrack.Domain.Interfaces.Repositories;

namespace FinTrack.Domain.Interfaces;

/// <summary>
/// Define um contrato para realizar operações de unidade de trabalho (UoW),
/// garantindo que múltiplas ações sejam executadas dentro de uma única transação.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Referência ao repositorio de produto.
    /// </summary>
    IProductRepository Products { get; }

    /// <summary>
    /// Persiste as alterações pendentes no contexto atual.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task CommitAsync(CancellationToken cancellationToken);
}