using Microsoft.EntityFrameworkCore;
using OrdersApi.Entities;
using OrdersApi.Infrastructure.Data;
namespace OrdersApi.Helpers
{
    /// <summary>
    /// Helper used to init database.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Add configuration data.
        /// </summary>       
        public static void Seed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OrdersApiDbContext>();
                db.Database.Migrate();

                if (!db.ProductConfigurations.Any())
                {
                    db.ProductConfigurations.Add(new ProductConfiguration { ProductType = "photoBook", NumberOfItemsInStack = 1, Width = 19 });
                    db.ProductConfigurations.Add(new ProductConfiguration { ProductType = "calendar", NumberOfItemsInStack = 1, Width = 10 });
                    db.ProductConfigurations.Add(new ProductConfiguration { ProductType = "canvas", NumberOfItemsInStack = 1, Width = 16 });
                    db.ProductConfigurations.Add(new ProductConfiguration { ProductType = "cards", NumberOfItemsInStack = 1, Width = 4.7m });
                    db.ProductConfigurations.Add(new ProductConfiguration { ProductType = "mug", NumberOfItemsInStack = 4, Width = 94 });
                    db.SaveChanges();
                }
            }
        }
    }
}
