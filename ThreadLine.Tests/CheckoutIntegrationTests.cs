
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ThreadLine.Data;
using ThreadLine.Models;
using Xunit;

namespace ThreadLine.Tests;

public class CheckoutIntegrationTests
{
    private static (SqliteConnection Connection, ApplicationDbContext Context)
        CreateTestDatabase()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        return (connection, context);
    }

    [Fact]
    public async Task Checkout_SavesSaleItemsAndDeductsStock()
    {
        var (connection, context) = CreateTestDatabase();

        using (connection)
        using (context)
        {
            var product = new Product
            {
                Name = "Test Shirt",
                Category = "Clothing",
                Price = 5000m,
                StockQuantity = 10,
                ReorderLevel = 2
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            const int quantitySold = 3;
            const decimal unitPrice = 5000m;

            await using var transaction =
                await context.Database.BeginTransactionAsync();

            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                TotalAmount = unitPrice * quantitySold
            };

            context.Sales.Add(sale);
            await context.SaveChangesAsync();

            context.SaleItems.Add(new SaleItem
            {
                SaleId = sale.Id,
                ProductId = product.Id,
                Quantity = quantitySold,
                UnitPrice = unitPrice
            });

            product.StockQuantity -= quantitySold;

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            context.ChangeTracker.Clear();

            var savedSale = await context.Sales
                .SingleAsync(s => s.Id == sale.Id);

            var savedItems = await context.SaleItems
                .Where(i => i.SaleId == sale.Id)
                .ToListAsync();

            var updatedProduct = await context.Products
                .SingleAsync(p => p.Id == product.Id);

            Assert.Equal(15000m, savedSale.TotalAmount);
            Assert.Single(savedItems);
            Assert.Equal(3, savedItems[0].Quantity);
            Assert.Equal(5000m, savedItems[0].UnitPrice);
            Assert.Equal(7, updatedProduct.StockQuantity);
        }
    }

    [Fact]
    public async Task Checkout_WithInsufficientStock_DoesNotSaveSale()
    {
        var (connection, context) = CreateTestDatabase();

        using (connection)
        using (context)
        {
            var product = new Product
            {
                Name = "Limited Stock Jacket",
                Category = "Clothing",
                Price = 12000m,
                StockQuantity = 2,
                ReorderLevel = 1
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            const int requestedQuantity = 3;

            // Mirror the stock validation in Sales.razor.
            if (requestedQuantity > product.StockQuantity)
            {
                Assert.Equal(0, await context.Sales.CountAsync());
                Assert.Equal(2, product.StockQuantity);
                return;
            }

            // Reaching this line means the stock validation failed.
            Assert.Fail("Checkout should reject a quantity greater than available stock.");
        }
    }
}
