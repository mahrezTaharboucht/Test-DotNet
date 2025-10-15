using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Mappers;

namespace OrdersApi.UnitTests.Mappers
{
    public class ProductConfigurationMapperTests
    {
        private readonly IProductConfigurationMapper _mapper = new ProductConfigurationMapper();

        [Fact]
        public void ToProductConfiguration_WhenCreateProductConfigurationDtoIsValid_ShouldReturnProductConfigurationEntity()
        {
            // Arrange
            const string productType = "Photo";
            const int numberOfItemsInStack = 1;
            const decimal width = 12m;
            var dto = new CreateProductConfigurationDto
            {
                NumberOfItemsInStack = numberOfItemsInStack,
                ProductType = productType,
                Width = width
            };

            // Act
            var productConf = _mapper.ToProductConfiguration(dto);
           
            // Assert
            Assert.Multiple(
                    () => Assert.NotNull(productConf),
                    () => Assert.Equal(numberOfItemsInStack, productConf.NumberOfItemsInStack),
                    () => Assert.Equal(width, productConf.Width),
                    () => Assert.Equal(productType, productConf.ProductType));
        }

        [Fact]
        public void ToProductConfiguration_WithCreateProductConfigurationDtoIsNull_ShouldReturnNull()
        {
            // Act
            var productConf = _mapper.ToProductConfiguration(null);

            // Assert
            Assert.Null(productConf);        
        }

        [Fact]
        public void ToProductConfigurationDetailResponseDto_WhenProductConfigurationIsValid_ShouldReturnProductConfigurationDetailResponseDto()
        {
            // Arrange
            const string productType = "Mugs";
            const int numberOfItemsInStack = 6;
            const decimal width = 9m;
            var productConf = new ProductConfiguration
            {                
                NumberOfItemsInStack = numberOfItemsInStack,
                ProductType = productType,
                Width = width
            };

            // Act
            var dto = _mapper.ToProductConfigurationDetailResponseDto(productConf);

            // Assert
            Assert.Multiple(
                    () => Assert.NotNull(productConf),
                    () => Assert.Equal(numberOfItemsInStack, productConf.NumberOfItemsInStack),
                    () => Assert.Equal(width, productConf.Width),
                    () => Assert.Equal(productType, productConf.ProductType));
        }

        [Fact]
        public void ToProductConfigurationDetailResponseDto_WhenProductConfigurationIsNull_ShouldReturnNull()
        {
            // Arrange
            const string productType = "Mugs";
            const int numberOfItemsInStack = 6;
            const decimal width = 9m;
            var productConf = new ProductConfiguration
            {
                NumberOfItemsInStack = numberOfItemsInStack,
                ProductType = productType,
                Width = width
            };

            // Act
            var dto = _mapper.ToProductConfigurationDetailResponseDto(null);

            // Assert
            Assert.Null(dto);           
        }        
    }
}
