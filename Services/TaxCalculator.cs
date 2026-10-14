
namespace ThreadLine.Services;

public class TaxCalculator
{
    public decimal CalculateTax(decimal subtotal, decimal taxRatePercent)
    {
        if (subtotal < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(subtotal), "Subtotal cannot be negative.");
        }

        if (taxRatePercent < 0 || taxRatePercent > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(taxRatePercent),
                "Tax rate must be between 0 and 100.");
        }

        return Math.Round(
            subtotal * taxRatePercent / 100m,
            2,
            MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTotal(decimal subtotal, decimal taxRatePercent)
    {
        return subtotal + CalculateTax(subtotal, taxRatePercent);
    }
}