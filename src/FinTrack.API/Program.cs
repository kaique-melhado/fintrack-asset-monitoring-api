using FinTrack.Configuration.Logging;
using FinTrack.Configuration.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog
builder.Host.UseSerilogConfiguration();

// Injeção de dependências da aplicação
builder.Services.AddConfigurations(builder.Configuration);

var app = builder.Build();

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

// Execução da aplicação
app.Run();