using FinTrack.Domain.Entities;
using FinTrack.UnitTests.Domain.Builders;
using FluentAssertions;

namespace FinTrack.UnitTests.Domain.Entities;

public class PriceHistoryTests
{
    private const decimal ValidPrice = 100;
    private const string ValidSource = "AlphaVantage";

    #region Criação válida

    [Fact(DisplayName = "Deve criar histórico de preço válido com dados corretos")]
    public void Should_Create_Valid_PriceHistory_With_Valid_Data()
    {
        // Arrange
        var product = new ProductBuilder().Build();
        var price = 120.45m;
        var date = new DateTime(2023, 10, 15);

        // Act
        var history = new PriceHistoryBuilder()
            .WithProductId(product.Id)
            .WithPrice(price)
            .WithDate(date)
            .WithSource(ValidSource)
            .Build();

        // Assert
        history.Should().NotBeNull();
        history.ProductId.Should().Be(product.Id);
        history.Price.Should().Be(price);
        history.Date.Should().Be(date);
        history.Source.Should().Be(ValidSource);
        history.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Histórico de preços deve ter ID único")]
    public void Should_Assign_Unique_Id_To_PriceHistory()
    {
        // Arrange
        var h1 = CreateValidHistory();
        var h2 = CreateValidHistory();

        // Assert
        h1.Id.Should().NotBeEmpty();
        h2.Id.Should().NotBeEmpty();
        h1.Id.Should().NotBe(h2.Id);
    }

    [Fact(DisplayName = "Deve aceitar datas passadas para histórico de preços")]
    public void Should_Allow_Past_Dates()
    {
        // Arrange
        var pastDate = new DateTime(2005, 12, 1);

        // Act
        var act = () => new PriceHistoryBuilder()
            .WithDate(pastDate)
            .Build();

        // Assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "Deve criar histórico válido com método auxiliar de construção")]
    public void Should_Create_Valid_PriceHistory_With_Helper()
    {
        // Arrange
        var history = CreateValidHistory();

        // Assert
        history.Should().NotBeNull();
        history.Price.Should().Be(ValidPrice);
        history.Source.Should().Be(ValidSource);
    }

    #endregion

    #region Criação inválida

    [Theory(DisplayName = "Deve lançar exceção ao criar histórico com fonte nula ou vazia")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Throw_Exception_When_Source_Is_Invalid(string invalidSource)
    {
        // Act
        var act = () => new PriceHistoryBuilder()
            .WithSource(invalidSource!)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("A fonte do histórico de preço deve ser informada.*");
    }

    [Theory(DisplayName = "Deve lançar exceção ao criar histórico com preço zero ou negativo")]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_Throw_Exception_When_Price_Is_Invalid(decimal invalidPrice)
    {
        // Act
        var act = () => new PriceHistoryBuilder()
            .WithPrice(invalidPrice)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("O preço deve ser maior que zero.*");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar histórico com data futura")]
    public void Should_Throw_Exception_When_Date_Is_In_The_Future()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(2);

        // Act
        var act = () => new PriceHistoryBuilder()
            .WithDate(futureDate)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("A data do histórico não pode ser no futuro.*");
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar histórico com data um segundo no futuro")]
    public void Should_Throw_Exception_When_Date_Is_Just_Ahead_Of_Now()
    {
        // Arrange
        var slightlyFuture = DateTime.UtcNow.AddSeconds(1.5);

        // Act
        var act = () => new PriceHistoryBuilder()
            .WithDate(slightlyFuture)
            .Build();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("A data do histórico não pode ser no futuro.*");
    }

    #endregion

    #region Helpers

    private static PriceHistory CreateValidHistory()
    {
        return new PriceHistoryBuilder()
            .WithPrice(ValidPrice)
            .WithSource(ValidSource)
            .Build();
    }

    #endregion
}