using OrdersApi.Entities;
using OrdersApi.Infrastructure.Data;

namespace OrdersApi.Infrastructure.Repositories
{
    /// <inheritdoc/>
    /// <summary>
    /// Orders repository.
    /// </summary>
    /// <param name="ordersApiDbContext">Db context.</param>
    public class OrdersRepository(OrdersApiDbContext ordersApiDbContext) : Repository<Order>(ordersApiDbContext)
    {
    }
}
