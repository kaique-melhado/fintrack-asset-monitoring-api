using FinTrack.Domain.Enums;
using FinTrack.Domain.Exceptions;
using FinTrack.UnitTests.Domain.Builders;
using FinTrack.UnitTests.Domain.Fixtures;
using FluentAssertions;

namespace FinTrack.UnitTests.Domain.Entities;

public class ProductTests
{
    #region Criação válida

    [Fact(DisplayName = "Deve criar um produto válido com os parâmetros fornecidos")]
    public void Should_Create_Valid_Product_With_Valid_Arguments()
    {
        // Arrange
        var name = "Ação Petrobras";
        var ticker = "PETR4";
        var type = ProductType.Stock;
        var category = ProductCategory.VariableIncome;
        var currency = CurrencyFixture.Default();

        // Act
        var product = new ProductBuilder()
            .WithName(name)
            .WithTicker(ticker)
            .WithType(type)
            .WithCategory(category)
            .WithCurrency(currency)
            .Build();

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Ticker.Should().Be(ticker);
        product.Type.Should().Be(type);
        product.Category.Should().Be(category);
        product.Currency.Code.Should().Be(currency.Code);
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        product.CurrentPrice.Should().Be(0);
    }

    #endregion

    #region Criação inválida

    [Theory(DisplayName = "Deve lançar exceção ao criar produto com nome inválido")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Exception_When_Creating_Product_With_Invalid_Name(string invalidName)
    {
        // Arrange
        var builder = new ProductBuilder().WithName(invalidName);

        // Act
        var act = () => builder.Build();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome do produto é obrigatório.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar produto com ticker inválido")]
    public void Should_Throw_Exception_When_Ticker_Is_Invalid()
    {
        // Arrange
        var builder = new ProductBuilder().WithInvalidTicker();

        // Act
        var act = () => builder.Build();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Ticker do produto é obrigatório.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar produto com moeda nula")]
    public void Should_Throw_Exception_When_Currency_Is_Null()
    {
        // Arrange
        var builder = new ProductBuilder().WithInvalidCurrency();

        // Act
        var act = () => builder.Build();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Moeda do produto é obrigatória.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar produto com tipo inválido")]
    public void Should_Throw_Exception_When_ProductType_Is_Invalid()
    {
        // Arrange
        var builder = new ProductBuilder().WithInvalidType();

        // Act
        var act = () => builder.Build();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Tipo de produto inválido.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar produto com categoria inválida")]
    public void Should_Throw_Exception_When_Category_Is_Invalid()
    {
        // Arrange
        var builder = new ProductBuilder().WithInvalidCategory();

        // Act
        var act = () => builder.Build();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria de produto inválida.");
    }

    #endregion

    #region Atualização de preço

    [Fact(DisplayName = "Deve atualizar o preço do produto corretamente")]
    public void Should_Update_Product_Price_Correctly()
    {
        // Arrange
        var product = new ProductBuilder().Build();

        // Act
        product.UpdatePrice(35.7m);

        // Assert
        product.CurrentPrice.Should().Be(35.7m);
    }

    [Fact(DisplayName = "Deve lançar exceção ao atualizar o preço com valor negativo")]
    public void Should_Throw_Exception_When_Updating_Product_With_Negative_Price()
    {
        // Arrange
        var product = new ProductBuilder().Build();

        // Act
        var act = () => product.UpdatePrice(-10);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O preço deve ser maior que zero.");
    }

    [Fact(DisplayName = "Deve lançar exceção ao atualizar o preço com valor zero")]
    public void Should_Throw_Exception_When_Updating_Product_With_Zero_Price()
    {
        // Arrange
        var product = new ProductBuilder().Build();

        // Act
        var act = () => product.UpdatePrice(0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O preço deve ser maior que zero.");
    }

    [Fact(DisplayName = "Deve manter o último preço ao atualizar múltiplas vezes")]
    public void Should_Keep_Last_Updated_Price()
    {
        // Arrange
        var product = new ProductBuilder().Build();

        // Act
        product.UpdatePrice(10);
        product.UpdatePrice(55);

        // Assert
        product.CurrentPrice.Should().Be(55);
    }

    #endregion
}