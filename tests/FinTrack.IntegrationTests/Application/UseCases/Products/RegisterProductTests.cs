using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FinTrack.Application.UseCases.Products.Commands.RegisterProduct;
using FinTrack.Configuration.Middlewares;
using FinTrack.Domain.Enums;
using FinTrack.IntegrationTests.Extensions;
using FinTrack.IntegrationTests.Setup;
using FluentAssertions;

namespace FinTrack.IntegrationTests.Application.UseCases.Products;

public class RegisterProductTests : IntegrationTestBase
{
    private readonly IFixture _fixture;

    public RegisterProductTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _fixture = new Fixture();
    }

    [Theory(DisplayName = "Deve registrar produtos válidos com sucesso")]
    [InlineData("PETR4", "Petrobras")]
    [InlineData("VALE3", "Vale S.A.")]
    [InlineData("ITUB4", "Itaú Unibanco")]
    public async Task Should_Register_Valid_Products(string ticker, string name)
    {
        // Arrange
        var command = new RegisterProductCommand(
            Name: name,
            Ticker: ticker,
            Type: ProductType.Stock,
            Category: ProductCategory.VariableIncome,
            CurrencyCode: "BRL"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<RegisterProductResponse>();
        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.Ticker.Should().Be(ticker);
        result.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Deve retornar erro ao registrar produto com ticker duplicado")]
    public async Task Should_Return_Error_When_Ticker_Is_Duplicated()
    {
        // Arrange
        var ticker = "DUPL4";
        var command = new RegisterProductCommand(
            Name: "Produto Original",
            Ticker: ticker,
            Type: ProductType.Stock,
            Category: ProductCategory.VariableIncome,
            CurrencyCode: "BRL"
        );

        var firstResponse = await _client.PostAsJsonAsync("/api/products", command);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Act
        var duplicateCommand = command with { Name = "Produto Duplicado" };
        var duplicateResponse = await _client.PostAsJsonAsync("/api/products", duplicateCommand);

        // Assert
        var errorResponse = await duplicateResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse.Should().NotBeNull();
        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse.Message.Should().Be($"Já existe um produto registrado com o ticker '{ticker}'.");
    }

    [Theory(DisplayName = "Deve retornar erro ao registrar produto com nome inválido")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Should_Return_Error_When_Name_Is_Invalid(string? invalidName)
    {
        // Arrange
        var command = new RegisterProductCommand(
            Name: invalidName!,
            Ticker: "VALID11",
            Type: ProductType.Stock,
            Category: ProductCategory.VariableIncome,
            CurrencyCode: "BRL"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await response.ShouldContainFluentValidationError("O nome do produto é obrigatório.");
    }

    [Theory(DisplayName = "Deve retornar erro ao registrar produto com código de moeda inválido")]
    [InlineData("", "O código da moeda é obrigatório.")]
    [InlineData("BR", "O código da moeda deve conter 3 letras.")]
    [InlineData("BRLZ", "O código da moeda deve conter 3 letras.")]
    [InlineData(null, "O código da moeda é obrigatório.")]
    public async Task Should_Return_Error_When_CurrencyCode_Is_Invalid(string? invalidCode, string expectedError)
    {
        // Arrange
        var command = new RegisterProductCommand(
            Name: "Produto de Teste",
            Ticker: "VALID22",
            Type: ProductType.Stock,
            Category: ProductCategory.VariableIncome,
            CurrencyCode: invalidCode!
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await response.ShouldContainFluentValidationError(expectedError);
    }

    [Theory(DisplayName = "Deve retornar erro ao registrar produto com tipo inválido")]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(99)]
    public async Task Should_Return_Error_When_ProductType_Is_Invalid(int productType)
    {
        // Arrange
        var payload = new
        {
            name = "Produto Inválido",
            ticker = "INV01",
            type = productType,
            category = 1,
            currencyCode = "BRL"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await response.ShouldContainFluentValidationError("Tipo de produto inválido.");
    }

    [Theory(DisplayName = "Deve retornar erro ao registrar produto com categoria inválida")]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(99)]
    public async Task Should_Return_Error_When_ProductCategory_Is_Invalid(int productCategory)
    {
        // Arrange
        var payload = new
        {
            name = "Produto Inválido",
            ticker = "INV01",
            type = 1,
            category = productCategory,
            currencyCode = "BRL"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await response.ShouldContainFluentValidationError("Categoria de produto inválida.");
    }

    [Fact(DisplayName = "Deve retornar erro interno ao ocorrer falha de persistência")]
    public async Task Should_Return_InternalServerError_When_Persistence_Fails()
    {
        // Arrange
        var command = new RegisterProductCommand(
            Name: "Novo Produto",
            Ticker: "FAIL01",
            Type: ProductType.Stock,
            Category: ProductCategory.VariableIncome,
            CurrencyCode: "BRL"
        );

        await _factory.StopSqlContainerAsync();

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        await response.ShouldContainErrorResponse("Erro interno inesperado. Tente novamente mais tarde.", (int)HttpStatusCode.InternalServerError);
    }
}