using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Mappers;

namespace OrdersApi.UnitTests.Mappers
{
    [Trait("Category", "Unit")]
    public class OrderMapperTests
    {
        private readonly IOrderMapper _mapper = new OrderMapper();
            
        [Fact]
        public void ToOrderEntity_WhenCreateOrderDtoIsProvided_ShouldReturnOrderEntity()
        {
            // Arrange
            const string productType = "Mug";
            const int quantity = 1;
            var dto = new CreateOrderDto
            {
                 Items = new List<CreateOrderItemDto>
                 {
                     new CreateOrderItemDto
                     {
                          ProductType = productType,
                          Quantity = quantity
                     }
                 }
            };

            // Act
            var order = _mapper.ToOrderEntity(dto);
            var orderItem = order?.Items.FirstOrDefault();

            // Assert
            Assert.Multiple(
                    () => Assert.NotNull(order),
                    () => Assert.NotNull(orderItem),
                    () => Assert.Single(order.Items),
                    () => Assert.Equal(productType, orderItem.ProductType),
                    () => Assert.Equal(quantity, orderItem.Quantity));
        }

        [Fact]
        public void ToOrderEntity_WhenCreateOrderDtoIsNull_ShouldReturnNull()
        {
            // Act
            var order = _mapper.ToOrderEntity(null);

            // Assert
            Assert.Null(order);           
        }

        [Fact]
        public void ToOrderDetailResponseDto_WhenOrderIsValid_ShouldReturnOrderDetailResponseDto()
        {
            // Arrange
            const string productType = "Cards";
            const int quantity = 3;
            const decimal width = 12m;
            var order = new Order
            {                
                RequiredBinWidth = width,
                Items = new List<OrderItem>
                 {
                     new OrderItem
                     {                          
                          ProductType = productType,
                          Quantity = quantity
                     }
                 }
            };

            // Act
            var orderDto = _mapper.ToOrderDetailResponseDto(order);
            var orderItemDto = orderDto?.Items.FirstOrDefault();

            // Assert
            Assert.Multiple(
                    () => Assert.NotNull(orderDto),
                    () => Assert.NotNull(orderItemDto),
                    () => Assert.Equal(width, orderDto.RequiredBinWidth),
                    () => Assert.Single(order.Items),
                    () => Assert.Equal(productType, orderItemDto.ProductType),
                    () => Assert.Equal(quantity, orderItemDto.Quantity));
        }

        [Fact]
        public void ToOrderDetailResponseDto_WhenOrderIsNull_ShouldReturnNull()
        {
            // Act
            var orderDto = _mapper.ToOrderDetailResponseDto(null);

            // Assert
            Assert.Null(orderDto);            
        }

        [Fact]
        public void ToCreateOrderResponseDto_WhenOrderIsValid_ShouldReturnCreateOrderResponseDto()
        {
            // Arrange
            const decimal width = 11m;
            var order = new Order
            {
                RequiredBinWidth = width                
            };

            // Act
            var orderDto = _mapper.ToCreateOrderResponseDto(order);

            // Assert
            Assert.Equal(width, orderDto.RequiredBinWidth);       
        }

        [Fact]
        public void ToCreateOrderResponseDto_WhenOrderIsNull_ShouldReturnNull()
        {
            // Act
            var orderDto = _mapper.ToCreateOrderResponseDto(null);

            // Assert
            Assert.Null(orderDto);
        }
    }
}
