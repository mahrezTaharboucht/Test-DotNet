using OrdersApi.Entities;
using OrdersApi.Services.Itf;

namespace OrdersApi.UnitTests.Mocks
{
    internal class ProductConfigurationMockService : IProductConfigurationService
    {
        private readonly IEnumerable<ProductConfiguration> _productsConfig = new List<ProductConfiguration>()
        {
            new ProductConfiguration
            {
                Id = 1,
                ProductType = "photoBook", 
                Width = 19, 
                NumberOfItemsInStack = 1,
            },

            new ProductConfiguration
            {
                Id = 2,
                ProductType = "calendar",
                Width = 10,
                NumberOfItemsInStack = 1,
            },

             new ProductConfiguration
            {
                Id = 3,
                ProductType = "canvas",
                Width = 16,
                NumberOfItemsInStack = 1,
            },

              new ProductConfiguration
            {
                Id = 4,
                ProductType = "cards",
                Width = 4.7m,
                NumberOfItemsInStack = 1,
            },

               new ProductConfiguration
            {
                Id = 5,
                ProductType = "mug",
                Width = 94,
                NumberOfItemsInStack = 4,
            }
        };
        public Task CreateProductConfiguration(ProductConfiguration productConfiguration)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductConfiguration>> GetProductConfigurations()
        {
            return Task.FromResult(_productsConfig);
        }
    }
}
