using OrdersApi.Dtos.ProductConfigurations;

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
        Task CreateProductConfiguration(CreateProductConfigurationDto productConfiguration);
        
        /// <summary>
        /// Get product configurations.
        /// </summary>       
        Task<IEnumerable<ProductConfigurationDetailResponseDto>> GetProductConfigurations();
    }
}
