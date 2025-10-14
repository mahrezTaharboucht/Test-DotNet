using OrdersApi.Entities;

namespace OrdersApi.Interfaces.Services
{
    /// <summary>
    /// Manage product configuration.
    /// </summary>
    public interface IProductConfigurationService
    {
        /// <summary>
        /// Create a product configuration entity.
        /// </summary>
        /// <param name="productConfiguration">Entity.</param>
        Task CreateProductConfiguration(ProductConfiguration productConfiguration);
        
        /// <summary>
        /// Get product configurations.
        /// </summary>
        /// <returns>Product configuration list.</returns>
        Task<IEnumerable<ProductConfiguration>> GetProductConfigurations();
    }
}
