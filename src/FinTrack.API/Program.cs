using FinTrack.Configuration.Logging;
using FinTrack.Configuration.DependencyInjection;
using FinTrack.Configuration.Middlewares;

/// <summary>
/// Classe de entrada da aplicação.
/// Utilizada para inicialização do host e testes de integração.
/// </summary>
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuração do Serilog
        builder.Host.UseSerilogConfiguration();

        // Injeção de dependências e serviços customizados da aplicação
        builder.Services.AddConfigurations(builder.Configuration);

        var app = builder.Build();

        // Middleware para tratamento de exceções
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            // Carrega segredos do usuário para a configuração durante o desenvolvimento.
            builder.Configuration.AddUserSecrets<Program>();

            // Ativa o Swagger apenas em ambiente de desenvolvimento
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware para redirecionamento HTTPS e controle de autorização
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Endpoint de HealthCheck para monitoramento da API e infraestrutura
        app.MapHealthChecks("/health");

        // Mapeamento dos endpoints baseados em controllers
        app.MapControllers();

        // Execução da aplicação
        app.Run();
    }
}