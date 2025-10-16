
using Microsoft.AspNetCore.Mvc.Testing;
using OrdersApi.Dtos;
using OrdersApi.Dtos.ProductConfigurations;
using System.Net;
using System.Net.Http.Json;

namespace OrdersApi.Tests
{
    [Trait("Category", "Integration")]
    public class ProductConfigurationControllerTests : IClassFixture<IntegrationTestsApplication>, IAsyncLifetime
    {       
        private readonly HttpClient _client;
        private readonly IntegrationTestsApplication _factory;

        public ProductConfigurationControllerTests(IntegrationTestsApplication factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAsync_WhenCalled_ShouldReturnProductConfiguration()
        {
            // Act
            var response = await _client.GetAsync($"/productConfiguration");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);                        
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductConfigurationDetailResponseDto>>>();
            Assert.NotNull(result);           
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);   
        }

        
        [Fact]
        public async Task Create_WhenNumberOfItemsInStackIsNotValid_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedError = "The number of items in stack should be greater than 0.";
            var createDto = CreateProductConfigurationDto(numberOfItemsInStack:0);
                     
            // Act
            var response = await _client.PostAsJsonAsync($"/productConfiguration", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);           
        }

        [Fact]
        public async Task Create_WhenProductTypeIsNotValid_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedError = "Product type should be provided.";
            var createDto = CreateProductConfigurationDto(productType:"");

            // Act
            var response = await _client.PostAsJsonAsync($"/productConfiguration", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenProductTypeExist_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedError = "The given product type value already exists.";
            var createDto = CreateProductConfigurationDto(productType: "Mug");

            // Act
            var response = await _client.PostAsJsonAsync($"/productConfiguration", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenWidthIsNotValid_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedError = "Width should be greater than 0.";
            var createDto = CreateProductConfigurationDto(width:-1);

            // Act
            var response = await _client.PostAsJsonAsync($"/productConfiguration", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenProductConfigurationDataIsValid_ShouldCreateConfigurationAndReturnRequiredMinWidth()
        {
            // Arrange
            var expectedNumberOfItemsInStack = 1;
            var expectedWidth = 94m;
            var expectedProductType = "NeMug";
            var createDto = CreateProductConfigurationDto(width: expectedWidth, productType: expectedProductType, numberOfItemsInStack: expectedNumberOfItemsInStack);

            // Act
            var response = await _client.PostAsJsonAsync($"/productConfiguration", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductConfigurationDetailResponseDto>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedWidth, result.Data.Width);
            Assert.Equal(expectedProductType, result.Data.ProductType);
            Assert.Equal(expectedNumberOfItemsInStack, result.Data.NumberOfItemsInStack);
        }

        private static CreateProductConfigurationDto CreateProductConfigurationDto(int numberOfItemsInStack = 1, string productType = "newProduct", decimal width = 100)
        {
            return new CreateProductConfigurationDto
            {
                 NumberOfItemsInStack  = numberOfItemsInStack,
                 ProductType = productType,
                 Width = width

            };
        }
        public Task DisposeAsync() => Task.CompletedTask;      

        public Task InitializeAsync() => Task.CompletedTask;
    }
}