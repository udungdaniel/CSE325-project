
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ThreadLine.Data;
using ThreadLine.Models;
using Xunit;

namespace ThreadLine.Tests;

public class DatabaseIntegrationTests
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
    public async Task Product_CanBeCreatedAndRetrieved()
    {
        var (connection, context) = CreateTestDatabase();

        using (connection)
        using (context)
        {
            var product = new Product
            {
                Name = "School Shirt",
                Category = "Clothing",
                Size = "M",
                Color = "Blue",
                Price = 5000m,
                StockQuantity = 20,
                ReorderLevel = 5
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products
                .FirstOrDefaultAsync(p => p.Name == "School Shirt");

            Assert.NotNull(savedProduct);
            Assert.Equal(5000m, savedProduct.Price);
            Assert.Equal(20, savedProduct.StockQuantity);
        }
    }

    [Fact]
    public async Task Product_CanBeUpdated()
    {
        var (connection, context) = CreateTestDatabase();

        using (connection)
        using (context)
        {
            var product = new Product
            {
                Name = "Office Shirt",
                Category = "Clothing",
                Price = 4000m,
                StockQuantity = 10,
                ReorderLevel = 3
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            product.Price = 4500m;
            product.StockQuantity = 15;
            await context.SaveChangesAsync();

            var updatedProduct = await context.Products
                .FindAsync(product.Id);

            Assert.NotNull(updatedProduct);
            Assert.Equal(4500m, updatedProduct.Price);
            Assert.Equal(15, updatedProduct.StockQuantity);
        }
    }

    [Fact]
    public async Task Product_CanBeDeleted()
    {
        var (connection, context) = CreateTestDatabase();

        using (connection)
        using (context)
        {
            var product = new Product
            {
                Name = "Old Trousers",
                Category = "Clothing",
                Price = 6000m,
                StockQuantity = 8,
                ReorderLevel = 2
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            Assert.False(await context.Products
                .AnyAsync(p => p.Name == "Old Trousers"));
        }
    }
}
