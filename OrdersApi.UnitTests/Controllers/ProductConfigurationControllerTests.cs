using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersApi.Controllers;
using OrdersApi.Dtos;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.UnitTests.Controllers
{
    [Trait("Category", "Unit")]
    public class ProductConfigurationControllerTests
    {
        private readonly Mock<IProductConfigurationService> _serviceMock;
        private readonly ProductConfigurationController _productConfigurationService;

        public ProductConfigurationControllerTests()
        {
            _serviceMock = new Mock<IProductConfigurationService>();
            _productConfigurationService = new ProductConfigurationController(_serviceMock.Object);
        }

        [Fact]
        public void Ctor_WhenServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new ProductConfigurationController(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());           
        }

        [Fact]
        public async Task GetAll_WhenCalled_ShouldReturnConfigurationsDetails()
        {
            // Arrange
            const int itemsCount = 2; 
            var productConfigurations = new List<ProductConfigurationDetailResponseDto>
            {
                new ProductConfigurationDetailResponseDto(),
                new ProductConfigurationDetailResponseDto()
            };
            _serviceMock.Setup(x => x.GetProductConfigurations()).ReturnsAsync(productConfigurations);

            // Act
            var result = await _productConfigurationService.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<ProductConfigurationDetailResponseDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(itemsCount, response.Data.Count());
        }

        [Fact]
        public async Task Create_WhenCalledWithValidData_ShouldReturnSuccessResponse()
        {
            // Arrange
            const int id = 1;
            const string productType = "Mug";
            const decimal width = 90;
            const int numberOfItemsInStack = 4;
            var dto = new CreateProductConfigurationDto();
            var response = new ProductConfigurationDetailResponseDto
            {
                NumberOfItemsInStack = numberOfItemsInStack,
                ProductType = productType,
                Width = width,
                Id = id
            };
            _serviceMock.Setup(x => x.CreateProductConfiguration(dto)).ReturnsAsync(response);

            // Act
            var result = await _productConfigurationService.Create(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ProductConfigurationDetailResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(id, apiResponse.Data.Id);
            Assert.Equal(productType, apiResponse.Data.ProductType);
            Assert.Equal(width, apiResponse.Data.Width);
            Assert.Equal(numberOfItemsInStack, apiResponse.Data.NumberOfItemsInStack);
        }
    }
}
