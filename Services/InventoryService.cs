
using ThreadLine.Models;

namespace ThreadLine.Services;

public class InventoryService
{
    public void DeductStock(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be greater than zero.");
        }

        if (product.StockQuantity < quantity)
        {
            throw new InvalidOperationException(
                $"Insufficient stock for {product.Name}.");
        }

        product.StockQuantity -= quantity;
    }
}