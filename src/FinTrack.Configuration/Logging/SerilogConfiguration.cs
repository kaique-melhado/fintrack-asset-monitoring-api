using Microsoft.Extensions.Hosting;
using Serilog;

namespace FinTrack.Configuration.Logging;

/// <summary>
/// Classe responsável por configurar o Serilog.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Adiciona a configuração padrão do Serilog ao host builder.
    /// </summary>
    /// <param name="builder">Host builder da aplicação.</param>
    /// <returns>Instância configurada do host builder.</returns>
    public static IHostBuilder UseSerilogConfiguration(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, services, loggerConfig) =>
        {
            var configuration = context.Configuration;
            if (configuration == null)
            {
                Log.Fatal("A configuração do Serilog não pôde ser carregada.");
                throw new InvalidOperationException("Falha ao carregar a configuração do Serilog.");
            }

            var env = context.HostingEnvironment;

            loggerConfig
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/fintrack-log.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                );
        });
    }
}