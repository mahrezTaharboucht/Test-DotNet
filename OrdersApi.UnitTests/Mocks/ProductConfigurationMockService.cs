using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.UnitTests.Mocks
{
    internal class ProductConfigurationMockService : IProductConfigurationService
    {
        private readonly IEnumerable<ProductConfigurationDetailResponseDto> _productsConfig = new List<ProductConfigurationDetailResponseDto>()
        {
            new ProductConfigurationDetailResponseDto
            {
                Id = 1,
                ProductType = "photoBook", 
                Width = 19, 
                NumberOfItemsInStack = 1,
            },

            new ProductConfigurationDetailResponseDto
            {
                Id = 2,
                ProductType = "calendar",
                Width = 10,
                NumberOfItemsInStack = 1,
            },

             new ProductConfigurationDetailResponseDto
            {
                Id = 3,
                ProductType = "canvas",
                Width = 16,
                NumberOfItemsInStack = 1,
            },

              new ProductConfigurationDetailResponseDto
            {
                Id = 4,
                ProductType = "cards",
                Width = 4.7m,
                NumberOfItemsInStack = 1,
            },

               new ProductConfigurationDetailResponseDto
            {
                Id = 5,
                ProductType = "mug",
                Width = 94,
                NumberOfItemsInStack = 4,
            }
        };
      
        public Task<ProductConfigurationDetailResponseDto> CreateProductConfiguration(CreateProductConfigurationDto productConfiguration)
        {
            throw new NotImplementedException();
        }               

        Task<IEnumerable<ProductConfigurationDetailResponseDto>> IProductConfigurationService.GetProductConfigurations()
        {
            return Task.FromResult(_productsConfig);
        }
    }
}
