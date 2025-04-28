using System.Reflection;
using FinTrack.Domain.Interfaces;
using FinTrack.Domain.Interfaces.Repositories;
using FinTrack.Infrastructure.Persistence;
using FinTrack.Infrastructure.Persistence.Context;
using FinTrack.Infrastructure.Persistence.Repositories;
using FinTrack.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace FinTrack.Configuration.DependencyInjection;

/// <summary>
/// Classe responsável por orquestrar o registro dos serviços da aplicação.
/// </summary>
public static class IocConfiguration
{
    /// <summary>
    /// Método principal para registrar todas as dependências da aplicação no contêiner de injeção.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configurações da aplicação.</param>
    /// <returns>Instância da <see cref="IServiceCollection"/> com os serviços configurados.</returns>
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services
            .ConfigureApplicationServices()
            .ConfigureInfrastructure(configuration)
            .ConfigureFluentValidation()
            .ConfigureHealthChecks(configuration)
            .ConfigureApi();

        return services;
    }

    /// <summary>
    /// Registra os serviços relacionados à camada Application, como MediatR e Handlers.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    private static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.Load("FinTrack.Application");

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
        });

        return services;
    }

    /// <summary>
    /// Registra os validadores da aplicação usando FluentValidation.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    private static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.Load("FinTrack.Application");

        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }

    /// <summary>
    /// Registra os serviços de infraestrutura, como acesso a arquivos, banco de dados, cache, etc.
    /// </summary>
    /// <param name="services">Coleção de serviços.</param>
    /// <param name="configuration">Configuração da aplicação.</param>
    private static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro do DbContext com string de conexão
        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Log.Fatal("A string de conexão 'POSTGRESQL_CONNECTION' ou 'DefaultConnection' não foi configurada.");
            throw new InvalidOperationException("A string de conexão não foi configurada corretamente.");
        }

        services.AddDbContext<FinTrackDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        // Registro de repositórios
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPriceHistoryRepository, PriceHistoryRepository>();

        // Registro do Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Aplica automaticamente as migrations do Entity Framework Core
        services.AddHostedService<MigrateDatabaseHostedService>();

        return services;
    }

    private static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        // Autorização (necessária mesmo que ainda não use [Authorize])
        services.AddAuthorization();

        // Controllers
        services.AddControllers(); // Garante que os Controllers serão mapeados

        // Swagger / OpenAPI
        services.AddEndpointsApiExplorer();

        //Configuração de Documentação com Swagger
        services.AddSwaggerGen(opts =>
        {
            var applicationAssembly = Assembly.Load("FinTrack.API");
            var xmlFile = $"{applicationAssembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            opts.SwaggerDoc("v1", new OpenApiInfo { Title = "FinTrackAPI", Version = "v1" });
            opts.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    private static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Log.Fatal("A string de conexão 'POSTGRESQL_CONNECTION' ou 'DefaultConnection' não foi configurada.");
            throw new InvalidOperationException("A string de conexão não foi configurada corretamente.");
        }

        services.AddHealthChecks()
            .AddNpgSql(connectionString!, name: "postgresql", tags: new[] { "db", "sql", "infra" });

        return services;
    }
}