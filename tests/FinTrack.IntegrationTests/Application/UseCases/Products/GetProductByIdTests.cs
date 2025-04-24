using System.Net;
using System.Net.Http.Json;
using FinTrack.Application.UseCases.Products.Commands.RegisterProduct;
using FinTrack.Application.UseCases.Products.Queries.Responses;
using FinTrack.IntegrationTests.Extensions;
using FinTrack.IntegrationTests.Setup;
using FluentAssertions;

namespace FinTrack.IntegrationTests.Application.UseCases.Products;

public class GetProductByIdTests : IntegrationTestBase
{
    public GetProductByIdTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact(DisplayName = "Deve retornar um produto existente com sucesso")]
    public async Task Should_Return_Product_When_Id_Is_Valid()
    {
        // Arrange (cria produto para depois consultá-lo)
        var createCommand = new RegisterProductCommand(
            Name: "Produto Teste",
            Ticker: "GET01",
            Type: Domain.Enums.ProductType.Stock,
            Category: Domain.Enums.ProductCategory.VariableIncome,
            CurrencyCode: "BRL"
        );

        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<RegisterProductResponse>();

        // Act
        var response = await _client.GetAsync($"/api/products/{createdProduct!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var product = await response.Content.ReadFromJsonAsync<ProductDetailsResponse>();
        product.Should().NotBeNull();
        product!.Id.Should().Be(createdProduct.Id);
        product.Name.Should().Be("Produto Teste");
        product.Ticker.Should().Be("GET01");
    }

    [Fact(DisplayName = "Deve retornar 404 quando o produto não existir")]
    public async Task Should_Return_NotFound_When_Product_Does_Not_Exist()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/products/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        await response.ShouldContainErrorResponse("Produto não encontrado.", (int)HttpStatusCode.NotFound);
    }
}