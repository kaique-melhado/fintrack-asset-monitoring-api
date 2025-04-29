using Microsoft.Extensions.Configuration;
using Serilog;

namespace FinTrack.Configuration.Extensions;

internal static class SafeConfigurationExtensions
{
    /// <summary>
    /// Obtém a string de conexão segura baseada no ambiente atual.
    /// </summary>
    /// <param name="configuration">Configurações da aplicação.</param>
    /// <returns>String de conexão do banco de dados.</returns>
    public static string GetSafeConnectionString(this IConfiguration configuration)
    {
        // Primeiramente tenta pegar da variável de ambiente do GitHub Actions
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Não Informado";
        var environmentConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION");

        // Se a variável existir, usa ela
        if (!string.IsNullOrWhiteSpace(environmentConnectionString))
        {
            return environmentConnectionString;
        }

        // Se não, tenta buscar do appsettings + user-secrets
        var appsettingsConnectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(appsettingsConnectionString))
        {
            return appsettingsConnectionString;
        }

        // Se ainda assim não achar, lança erro claro
        Log.Fatal("A string de conexão 'POSTGRESQL_CONNECTION' ou 'DefaultConnection' não foi configurada. Ambiente atual: {EnvironmentName}", environmentName);
        throw new InvalidOperationException("A connection string não foi encontrada nem na variável de ambiente nem no appsettings. Verifique a configuração.");
    }
}