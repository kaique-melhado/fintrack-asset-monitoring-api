using FinTrack.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace FinTrack.Configuration.Middlewares;

/// <summary>
/// Middleware responsável por capturar exceções não tratadas durante o processamento das requisições HTTP,
/// realizando o log do erro e retornando uma resposta JSON padronizada com o status e a mensagem.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">Delegate que representa o próximo middleware no pipeline.</param>
    /// <param name="logger">Instância do logger para registrar erros.</param>
    /// <param name="environment">Informações sobre o ambiente da aplicação.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Executa o middleware no pipeline de requisição HTTP, capturando qualquer exceção lançada durante a execução.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Trata a exceção capturada, define o status HTTP apropriado e escreve uma resposta padronizada em JSON.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP.</param>
    /// <param name="exception">Exceção capturada durante a execução do pipeline.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var stackTrace = _environment.IsDevelopment() ? exception.StackTrace : null;

        var (statusCode, responseMessage) = exception switch
        {
            DomainException domainEx => (HttpStatusCode.BadRequest, domainEx.Message),
            ApplicationException appEx => (HttpStatusCode.BadRequest, appEx.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado."),
            SqlException sqlEx => (HttpStatusCode.InternalServerError, "Erro ao acessar o banco de dados."),
            _ => (HttpStatusCode.InternalServerError, "Erro interno inesperado. Tente novamente mais tarde.")
        };

        _logger.LogError(exception, "Erro tratado no middleware: {Message} | Stack Trace: {StackTrace}", responseMessage, stackTrace);

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = responseMessage,
            StackTrace = stackTrace,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}