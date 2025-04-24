using FinTrack.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

namespace FinTrack.IntegrationTests.Setup;

/// <summary>
/// Fábrica personalizada de WebApplication para testes de integração com banco de dados isolado via Testcontainers.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder().Build();

    /// <summary>
    /// Obtém a string de conexão atual do banco de dados criado dinamicamente.
    /// </summary>
    public string ConnectionString => _sqlContainer.GetConnectionString();

    /// <summary>
    /// Configura os serviços da aplicação durante a inicialização dos testes.
    /// </summary>
    /// <param name="builder">Builder do host.</param>
    /// <returns>Instância do host configurado.</returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove o DbContext registrado originalmente
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<FinTrackDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            // Injeta um novo contexto com a connection string do container
            services.AddDbContext<FinTrackDbContext>(options =>
            {
                options.UseSqlServer(_sqlContainer.GetConnectionString());
            });
        });

        return base.CreateHost(builder);
    }

    /// <summary>
    /// Inicializa o container do SQL Server antes da execução dos testes.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    /// <summary>
    /// Encerra o container do banco de dados utilizado durante os testes de integração.
    /// </summary>
    public async Task StopSqlContainerAsync()
    {
        await _sqlContainer.StopAsync();
    }

    /// <summary>
    /// Encerra o container após os testes.
    /// </summary>
    async Task IAsyncLifetime.DisposeAsync()
    {
        await _sqlContainer.DisposeAsync();
    }
}