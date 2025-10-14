using OrdersApi.Entities;
using OrdersApi.Infrastructure.Data;

namespace OrdersApi.Infrastructure.Repositories
{
    /// <inheritdoc/>
    public class OrdersRepository : Repository<Order>
    {
        /// <summary>
        /// Orders repository.
        /// </summary>
        /// <param name="ordersApiDbContext">Db context.</param>
        public OrdersRepository(OrdersApiDbContext ordersApiDbContext) : base(ordersApiDbContext)
        {
        }
    }
}
