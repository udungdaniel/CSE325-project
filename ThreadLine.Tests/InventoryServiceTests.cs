
using Xunit;
using ThreadLine.Models;
using ThreadLine.Services;

namespace ThreadLine.Tests;

public class InventoryServiceTests
{
    private readonly InventoryService _service = new();

    [Fact]
    public void DeductStock_ReducesStockCorrectly()
    {
        var product = new Product
        {
            Name = "Shirt",
            Category = "Clothing",
            Price = 5000m,
            StockQuantity = 10
        };

        _service.DeductStock(product, 3);

        Assert.Equal(7, product.StockQuantity);
    }

    [Fact]
    public void DeductStock_InsufficientStock_Throws()
    {
        var product = new Product
        {
            Name = "Shirt",
            Category = "Clothing",
            Price = 5000m,
            StockQuantity = 2
        };

        Assert.Throws<InvalidOperationException>(
            () => _service.DeductStock(product, 3));

        Assert.Equal(2, product.StockQuantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeductStock_InvalidQuantity_Throws(int quantity)
    {
        var product = new Product
        {
            Name = "Shirt",
            Category = "Clothing",
            Price = 5000m,
            StockQuantity = 10
        };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _service.DeductStock(product, quantity));
    }
}