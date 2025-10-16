using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Infrastructure.Data;

namespace OrdersApi.Tests
{
    public static class Utils
    {
        public const string ExpectedValidationErrorMessage = "Validation error.";

        public static async Task CleanDbOrders(IntegrationTestsApplication factory) 
        {
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersApiDbContext>();
            dbContext.Orders.RemoveRange(dbContext.Orders);
            await dbContext.SaveChangesAsync();
        }
        public static async Task CleanDbProductConfiguration(IntegrationTestsApplication factory)
        {
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersApiDbContext>();
            dbContext.ProductConfigurations.RemoveRange(dbContext.ProductConfigurations);
            await dbContext.SaveChangesAsync();
        }
    }
}
