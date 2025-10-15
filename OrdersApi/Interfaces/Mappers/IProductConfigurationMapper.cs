using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;

namespace OrdersApi.Interfaces.Mappers
{
    /// <summary>
    /// Product configuration entity/dto mapper. 
    /// </summary>
    public interface IProductConfigurationMapper
    {
        /// <summary>
        /// Create ProductConfiguration from CreateProductConfigurationDto.      
        /// </summary>
        ProductConfiguration ToProductConfiguration(CreateProductConfigurationDto dto);

        /// <summary>
        /// Create ProductConfigurationDetailResponseDto from ProductConfiguration.
        /// </summary>        
        ProductConfigurationDetailResponseDto ToProductConfigurationDetailResponseDto(ProductConfiguration entity);
    }
}
