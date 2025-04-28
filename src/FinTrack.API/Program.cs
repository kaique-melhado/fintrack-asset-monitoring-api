using FinTrack.Configuration.Logging;
using FinTrack.Configuration.DependencyInjection;
using FinTrack.Configuration.Middlewares;

/// <summary>
/// Classe de entrada da aplica��o.
/// Utilizada para inicializa��o do host e testes de integra��o.
/// </summary>
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configura��o do Serilog
        builder.Host.UseSerilogConfiguration();

        // Inje��o de depend�ncias e servi�os customizados da aplica��o
        builder.Services.AddConfigurations(builder.Configuration);

        var app = builder.Build();

        // Middleware para tratamento de exce��es
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            // Carrega segredos do usu�rio para a configura��o durante o desenvolvimento.
            builder.Configuration.AddUserSecrets<Program>();

            // Ativa o Swagger apenas em ambiente de desenvolvimento
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Middleware para redirecionamento HTTPS e controle de autoriza��o
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Endpoint de HealthCheck para monitoramento da API e infraestrutura
        app.MapHealthChecks("/health");

        // Mapeamento dos endpoints baseados em controllers
        app.MapControllers();

        // Execu��o da aplica��o
        app.Run();
    }
}