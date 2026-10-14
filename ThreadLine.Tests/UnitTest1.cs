
using ThreadLine.Services;
using Xunit;
namespace ThreadLine.Tests;

public class TaxCalculatorTests
{
    private readonly TaxCalculator _calculator = new();

    [Fact]
    public void CalculateTax_ReturnsCorrectTax()
    {
        var tax = _calculator.CalculateTax(1000m, 7.5m);

        Assert.Equal(75m, tax);
    }

    [Fact]
    public void CalculateTotal_IncludesTax()
    {
        var total = _calculator.CalculateTotal(1000m, 7.5m);

        Assert.Equal(1075m, total);
    }

    [Fact]
    public void CalculateTax_ZeroRate_ReturnsZero()
    {
        var tax = _calculator.CalculateTax(1000m, 0m);

        Assert.Equal(0m, tax);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void CalculateTax_InvalidRate_Throws(decimal rate)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _calculator.CalculateTax(100m, rate));
    }
}