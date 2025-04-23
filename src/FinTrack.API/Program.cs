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

        // Configura Serilog
        builder.Host.UseSerilogConfiguration();

        // Inje��o de depend�ncias da aplica��o
        builder.Services.AddConfigurations(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();

        // Middleware do Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // HTTPS + Auth
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Mapeamento de endpoints de controllers
        app.MapControllers();

        // Execu��o da aplica��o
        app.Run();
    }
}