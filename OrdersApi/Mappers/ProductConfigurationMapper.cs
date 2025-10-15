using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;

namespace OrdersApi.Mappers
{
    /// <inheritdoc/>
    public class ProductConfigurationMapper : IProductConfigurationMapper
    {
        /// <inheritdoc/>
        public ProductConfiguration ToProductConfiguration(CreateProductConfigurationDto dto)
        {
            if (dto == null)
            {
                return default;
            }

            return new ProductConfiguration
            {
                NumberOfItemsInStack = dto.NumberOfItemsInStack,
                ProductType = dto.ProductType,
                Width = dto.Width
            };
        }

        /// <inheritdoc/>
        public ProductConfigurationDetailResponseDto ToProductConfigurationDetailResponseDto(ProductConfiguration entity)
        {
            if (entity == null)
            {
                return default;
            }

            return new ProductConfigurationDetailResponseDto
            {
                Id = entity.Id,
                NumberOfItemsInStack = entity.NumberOfItemsInStack,
                ProductType = entity.ProductType,
                Width = entity.Width
            };
        }
    }
}
