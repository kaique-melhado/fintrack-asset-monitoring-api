using FinTrack.Domain.Exceptions;
using FinTrack.Domain.ValueObjects;
using FluentAssertions;

namespace FinTrack.UnitTests.Domain.ValueObjects;

public class CurrencyTests
{
    [Theory(DisplayName = "Deve criar uma moeda válida com código ISO de 3 letras")]
    [InlineData("usd")]
    [InlineData("BRL")]
    [InlineData("eur")]
    public void Should_Create_Valid_Currency(string code)
    {
        // Act
        var currency = new Currency(code);

        // Assert
        currency.Code.Should().Be(code.ToUpper());
    }

    [Fact(DisplayName = "Deve normalizar código da moeda para letras maiúsculas")]
    public void Should_Normalize_Currency_Code_To_Uppercase()
    {
        var currency = new Currency("gbp");
        currency.Code.Should().Be("GBP");
    }

    [Theory(DisplayName = "Deve lançar exceção ao criar moeda com código inválido")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("abcd")]
    [InlineData("12")]
    public void Should_Throw_Exception_When_Currency_Code_Is_Invalid(string invalidCode)
    {
        // Act
        var act = () => new Currency(invalidCode);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Informe um código de moeda válido com 3 letras.");
    }

    [Fact(DisplayName = "Deve considerar duas moedas com o mesmo código como iguais")]
    public void Should_Consider_Currencies_With_Same_Code_As_Equal()
    {
        // Arrange
        var currency1 = new Currency("usd");
        var currency2 = new Currency("USD");

        // Assert
        currency1.Should().Be(currency2);
        currency1.Equals(currency2).Should().BeTrue();
        currency1.Equals(currency1).Should().BeTrue();
        currency1.GetHashCode().Should().Be(currency2.GetHashCode());
    }

    [Fact(DisplayName = "Deve considerar moedas com códigos diferentes como distintas")]
    public void Should_Consider_Different_Currencies_As_Not_Equal()
    {
        // Arrange
        var currency1 = new Currency("USD");
        var currency2 = new Currency("BRL");

        // Assert
        currency1.Should().NotBe(currency2);
        currency1.Equals(currency2).Should().BeFalse();
        currency1.GetHashCode().Should().NotBe(currency2.GetHashCode());
    }

    [Fact(DisplayName = "Deve lançar exceção ao criar moeda com espaços entre letras")]
    public void Should_Throw_Exception_When_Code_Has_Spaces_Inside()
    {
        // Arrange
        var act = () => new Currency(" u s ");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Informe um código de moeda válido com 3 letras.");
    }
}