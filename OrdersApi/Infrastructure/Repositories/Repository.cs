using Microsoft.EntityFrameworkCore;
using OrdersApi.Infrastructure.Data;
using OrdersApi.Interfaces.Repositories;

namespace OrdersApi.Infrastructure.Repositories
{
    /// <inheritdoc/>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected OrdersApiDbContext OrdersApiDbContext;
        protected DbSet<T> DbSet;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ordersApiDbContext">Database context.</param>
        public Repository(OrdersApiDbContext ordersApiDbContext)
        {
            OrdersApiDbContext = ordersApiDbContext;
            DbSet = OrdersApiDbContext.Set<T>();
        }

        /// <inheritdoc/>
        public async Task AddAsync(T entity)
        {
           await DbSet.AddAsync(entity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChangesAsync()
        {
            return await OrdersApiDbContext.SaveChangesAsync() > 0;
        }
    }
}
