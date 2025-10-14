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
        /// Save changes in Db.
        /// </summary>
        /// <returns>True if save succeeded.</returns>
        Task<bool> SaveChangesAsync();
    }
}
