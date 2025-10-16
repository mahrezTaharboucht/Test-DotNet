using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersApi.Controllers;
using OrdersApi.Dtos;
using OrdersApi.Dtos.Orders;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.UnitTests.Controllers
{
    [Trait("Category", "Unit")]
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController _orderController;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController = new OrderController(_orderServiceMock.Object);
        }

        [Fact]
        public void Ctor_WhenOrderServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var act = () => new OrderController(null);

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public async Task GetById_WhenOrderExists_ShouldReturnOrderDetail()
        {
            // Arrange
            var orderId = 1;
            var requiredBinWidth = 100;
            var orderDetail = new OrderDetailResponseDto
            {                
                RequiredBinWidth = requiredBinWidth
            };

            _orderServiceMock.Setup(x => x.GetOrder(orderId)).ReturnsAsync(orderDetail);

            // Act
            var result = await _orderController.GetById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<OrderDetailResponseDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(requiredBinWidth, response.Data.RequiredBinWidth);            
        }

        [Fact]
        public async Task GetById_WhenOrderDoesNotExist_ShouldReturnFailureResponse()
        {
            // Arrange
            var orderId = 999;
            var expectedMessage = "Order not found.";
            var expectedErrorMessage = "The order 999 not found.";
            _orderServiceMock.Setup(x => x.GetOrder(orderId)).ReturnsAsync((OrderDetailResponseDto)null);

            // Act
            var result = await _orderController.GetById(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal(expectedMessage, response.Message);
            Assert.Contains(expectedErrorMessage, response.Errors);            
        }

        [Fact]
        public async Task Create_WhenCalledWithValidData_ShouldReturnCreateOrderResponse()
        {
            // Arrange
            var orderId = 1;
            var requiredBinWidth = 14;
            var dto = new CreateOrderDto();
            var response = new CreateOrderResponseDto { RequiredBinWidth = requiredBinWidth };
            _orderServiceMock.Setup(x => x.CreateOrder(orderId, dto)).ReturnsAsync(response);

            // Act
            var result = await _orderController.Create(orderId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<CreateOrderResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(requiredBinWidth, apiResponse.Data.RequiredBinWidth);
        }
    }
}
