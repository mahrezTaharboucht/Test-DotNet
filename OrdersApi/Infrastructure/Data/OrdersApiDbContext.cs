using Microsoft.EntityFrameworkCore;
using OrdersApi.Entities;

namespace OrdersApi.Infrastructure.Data
{
    public class OrdersApiDbContext : DbContext
    {
        public OrdersApiDbContext(DbContextOptions<OrdersApiDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductConfiguration> ProductConfigurations { get; set; }        
    }
}
