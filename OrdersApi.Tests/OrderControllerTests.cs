
using Microsoft.AspNetCore.Mvc.Testing;
using OrdersApi.Dtos;
using OrdersApi.Dtos.Orders;
using System.Net;
using System.Net.Http.Json;

namespace OrdersApi.Tests
{
    [Trait("Category", "Integration")]
    public class OrderControllerTests : IClassFixture<IntegrationTestsApplication>, IAsyncLifetime
    {       
        private readonly HttpClient _client;
        private readonly IntegrationTestsApplication _factory;

        public OrderControllerTests(IntegrationTestsApplication factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Create_WhenOrderDataIsValid_ShouldCreateOrderAndReturnRequiredMinWidth()
        {
            // Arrange
            var orderId = 1;
            var expectedMinWidth = 94m;
            var createDto = CreateOrderWithOneItem();

            // Act
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);                        
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CreateOrderResponseDto>>();
            Assert.NotNull(result);           
            Assert.NotNull(result.Data);
            Assert.Equal(expectedMinWidth, result.Data.RequiredBinWidth);                 
                       
            var getResponse = await _client.GetAsync($"/order/{orderId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Fact]
        public async Task Create_WhenOrderExist_ShouldReturnBadRequest()
        {
            // Arrange
            var orderId = 2;
            var expectedError = "The order already exist.";
            var createDto = CreateOrderWithOneItem();
            
            // Create the order
            await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Act - create an order with the same Id.
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);           
        }

        [Fact]
        public async Task Create_WhenOrderIdIsNotValid_ShouldReturnBadRequest()
        {
            // Arrange
            var orderId = 0;
            var expectedError = "The order Id should be greater than 0.";
            var createDto = CreateOrderWithOneItem();

            // Act
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenOrderHasNoItems_ShouldReturnBadRequest()
        {
            // Arrange
            var orderId = 3;
            var expectedError = "Order items should contain at least one element.";
            var createDto = CreateOrderWithOneItem(addItem:false);

            // Act
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenOrderItemHasInvalidQuantity_ShouldReturnBadRequest()
        {
            // Arrange
            var orderId = 4;
            var expectedError = "Item quantity should be greater than 0.";
            var createDto = CreateOrderWithOneItem(quantity:0);

            // Act
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }

        [Fact]
        public async Task Create_WhenOrderItemHasInvalidProductType_ShouldReturnBadRequest()
        {
            // Arrange
            var orderId = 4;
            var expectedError = "Unknown product type.";
            var createDto = CreateOrderWithOneItem(productType: "NewUnknownProduct");

            // Act
            var response = await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.Contains(expectedError, result.Errors);
            Assert.Equal(Utils.ExpectedValidationErrorMessage, result.Message);
        }


        [Fact]
        public async Task GetAsync_WhenOrderIdIsValid_ShouldReturnOrder()
        {
            // Arrange
            var orderId = 5;
            var expectedMinWidth = 94m;
            var expectedItemCount = 1;
            var expectedItemType = "Mug";
            var expectedItemQuantity = 2;
            var createDto = CreateOrderWithOneItem();
            
            await _client.PutAsJsonAsync($"/order/{orderId}", createDto);

            // Act
            var response = await _client.GetAsync($"/order/{orderId}");            

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderDetailResponseDto>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);            
            Assert.Equal(expectedMinWidth, result.Data.RequiredBinWidth);
            Assert.Equal(expectedItemCount, result.Data.Items.Count);
            Assert.Equal(expectedItemType, result.Data.Items.First().ProductType);
            Assert.Equal(expectedItemQuantity, result.Data.Items.First().Quantity);
        }

        [Fact]
        public async Task GetAsync_WhenOrderIdIsNotKnown_ShouldReturnNotFound()
        {
            // Arrange
            var orderId = 6;
            var expectedOrderErrorMessage = "The order 6 not found.";
            var expectedOrderError = "Order not found.";

            // Act
            var response = await _client.GetAsync($"/order/{orderId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Errors);            
            Assert.Contains(expectedOrderErrorMessage, result.Errors);
            Assert.Equal(expectedOrderError, result.Message);
        }

        private static CreateOrderDto CreateOrderWithOneItem(bool addItem = true, string productType = "Mug", int quantity = 2)
        {
            var dto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>()
            };

            if (addItem)
            {
                dto.Items.Add(new CreateOrderItemDto { ProductType = productType, Quantity = quantity });
            }

            return dto;
        }

        public async Task DisposeAsync()
        {
            // Clean db data
            await Utils.CleanDbOrders(_factory);            
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}