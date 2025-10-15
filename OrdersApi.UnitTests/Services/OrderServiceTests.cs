using Moq;
using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Interfaces.Services;
using OrdersApi.Services;
using System.ComponentModel.DataAnnotations;

namespace OrdersApi.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Order>> _orderRepositoryMock;
        private readonly Mock<IBinWidthCalculator> _widthCalculatorMock;
        private readonly Mock<IOrderMapper> _orderMapperMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IRepository<Order>>();
            _widthCalculatorMock = new Mock<IBinWidthCalculator>();
            _orderMapperMock = new Mock<IOrderMapper>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _widthCalculatorMock.Object, _orderMapperMock.Object);
        }

        [Fact]
        public void Ctor_WhenOrderRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new OrderService(null, _widthCalculatorMock.Object, _orderMapperMock.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());            
        }

        [Fact]
        public void Ctor_WhenWidthCalculatorIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new OrderService(_orderRepositoryMock.Object, null, _orderMapperMock.Object);
                        
            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public void Ctor_WhenOrderMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new OrderService(_orderRepositoryMock.Object, _widthCalculatorMock.Object, null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]        
        public async Task CreateOrder_WhenOrderIdIsInvalid_ShouldThrowValidationException(int orderId)
        {
            // Arrange
            var expectedErrorMessage = "The order Id should be greater than 0.";
            var dto = new CreateOrderDto();

            // Act
            var act = async () => await _orderService.CreateOrder(orderId, dto);

            // Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => act());
            Assert.Equal(expectedErrorMessage, ex.Message);            
        }

        [Fact]
        public async Task CreateOrder_WhenOrderAlreadyExists_ShouldThrowValidationException()
        {
            // Arrange
            var expectedErrorMessage = "The order already exist.";
            var orderId = 1;
            var dto = new CreateOrderDto();
            _orderRepositoryMock.Setup(x => x.Exists(orderId)).ReturnsAsync(true);

            // Act
            var act = async () => await _orderService.CreateOrder(orderId, dto);

            // Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => act());
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public async Task CreateOrder_WhenValidRequest_ShouldCallRequiredServices()
        {
            // Arrange
            var orderId = 1;
            var dto = new CreateOrderDto();
            var order = new Order();
            _orderRepositoryMock.Setup(x => x.Exists(orderId)).ReturnsAsync(false);
            _orderMapperMock.Setup(x => x.ToOrderEntity(dto)).Returns(order);
            _widthCalculatorMock.Setup(x => x.CalculateBinMinWidth(dto.Items)).ReturnsAsync(100);

            // Act
            await _orderService.CreateOrder(orderId, dto);

            // Assert
            _widthCalculatorMock.Verify(x => x.CalculateBinMinWidth(dto.Items), Times.Once);
            _orderRepositoryMock.Verify(x => x.AddAsync(order), Times.Once);
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _orderMapperMock.Verify(x => x.ToCreateOrderResponseDto(order), Times.Once);
        }
    }
}
