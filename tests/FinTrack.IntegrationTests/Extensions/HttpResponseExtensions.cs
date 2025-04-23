using System.Net;
using System.Text.Json;
using FinTrack.Configuration.Middlewares;
using FluentAssertions;

namespace FinTrack.IntegrationTests.Extensions;

/// <summary>
/// Métodos de extensão para facilitar a verificação de respostas HTTP em testes de integração.
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// Valida se a resposta de erro segue o padrão do middleware de exceção da aplicação.
    /// </summary>
    /// <param name="response">Resposta HTTP da requisição.</param>
    /// <param name="expectedMessage">Mensagem esperada na resposta de erro.</param>
    /// <param name="expectedStatusCode">Status HTTP esperado (padrão: 400).</param>
    public static async Task ShouldContainErrorResponse(this HttpResponseMessage response, string expectedMessage, int expectedStatusCode = 400)
    {
        response.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        error.Should().NotBeNull();
        error!.Message.Should().Be(expectedMessage);
        error.StatusCode.Should().Be(expectedStatusCode);
        error.Timestamp.Should().BeOnOrBefore(DateTime.UtcNow);
    }

    /// <summary>
    /// Valida se a resposta de erro segue o padrão de validação automática (FluentValidation).
    /// </summary>
    /// <param name="response">Resposta HTTP da requisição.</param>
    /// <param name="expectedError">Mensagem de erro esperada em qualquer campo.</param>
    public static async Task ShouldContainFluentValidationError(this HttpResponseMessage response, string expectedError)
    {
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();

        var document = JsonDocument.Parse(content);
        document.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue("A resposta deve conter a propriedade 'errors'.");

        var flattenedErrors = errors.EnumerateObject()
            .SelectMany(property => property.Value.EnumerateArray().Select(v => v.GetString()))
            .ToList();

        flattenedErrors.Should().Contain(expectedError);
    }
}