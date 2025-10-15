using System.Linq.Expressions;

namespace OrdersApi.Interfaces.Repositories
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Entities list.</returns>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// Get entity by Id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity instance.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add an entity.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Check if an entity exists.
        /// </summary>
        /// <param name="id">Entity Id.</param>
        /// <returns>True if the entity was found.</returns>
        Task<bool> Exists(int id);

        /// <summary>
        /// Get an entity using a predicate.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="predicate">Predicate.</param>
        /// <returns>Entity or null.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Save changes in Db.
        /// </summary>
        /// <returns>True if save succeeded.</returns>
        Task<bool> SaveChangesAsync();
    }
}
