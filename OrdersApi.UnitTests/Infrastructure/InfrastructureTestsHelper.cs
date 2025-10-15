using Microsoft.EntityFrameworkCore;
using OrdersApi.Infrastructure.Data;
namespace OrdersApi.UnitTests.Infrastructure
{
    public static class InfrastructureTestsHelper
    {
        public static OrdersApiDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<OrdersApiDbContext>()
                .UseInMemoryDatabase(string.Concat("TestDb_", Guid.NewGuid()))
                .Options;

            return new OrdersApiDbContext(options);
        }       
    }
}
