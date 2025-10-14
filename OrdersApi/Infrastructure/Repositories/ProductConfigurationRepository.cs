using OrdersApi.Entities;
using OrdersApi.Infrastructure.Data;

namespace OrdersApi.Infrastructure.Repositories
{
    /// <summary>
    /// Product configuration repository.
    /// </summary>
    public class ProductConfigurationRepository : Repository<ProductConfiguration>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ordersApiDbContext">Database context.</param>
        public ProductConfigurationRepository(OrdersApiDbContext ordersApiDbContext) : base(ordersApiDbContext)
        {
        }
    }
}
