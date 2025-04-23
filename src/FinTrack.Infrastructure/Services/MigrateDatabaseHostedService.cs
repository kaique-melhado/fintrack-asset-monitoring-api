using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FinTrack.Infrastructure.Persistence.Context;

namespace FinTrack.Infrastructure.Services;

/// <summary>
/// Responsável por aplicar automaticamente as migrations do Entity Framework Core ao iniciar a aplicação. 
/// Garante que o banco de dados esteja atualizado com a estrutura mais recente.
/// </summary>
public sealed class MigrateDatabaseHostedService : IHostedService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<MigrateDatabaseHostedService> _logger;

    /// <summary>
    /// Inicializa uma nova instância da <see cref="MigrateDatabaseHostedService"/>.
    /// </summary>
    /// <param name="sp">Provedor de serviços da aplicação.</param>
    /// <param name="logger">Logger para registrar informações da migração.</param>
    public MigrateDatabaseHostedService(IServiceProvider sp, ILogger<MigrateDatabaseHostedService> logger)
    {
        _sp = sp ?? throw new ArgumentNullException(nameof(sp));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Executa o processo de migração do banco de dados ao iniciar o serviço.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Uma <see cref="Task"/> de operação assíncrona.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando migração do banco de dados...");

        using var scope = _sp.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FinTrackDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);

        _logger.LogInformation("Migração concluída com sucesso.");
    }

    /// <summary>
    /// Método chamado durante o encerramento do serviço.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Uma <see cref="Task"/> de operação assíncrona concluída.</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}