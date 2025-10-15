using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;

namespace OrdersApi.Mappers
{
    /// <inheritdoc/>
    public class OrderMapper : IOrderMapper
    {
        /// <inheritdoc/>
        public CreateOrderResponseDto ToCreateOrderResponseDto(Order entity)
        {
            if (entity == null)
            {
                return default;
            }
            
            return new CreateOrderResponseDto
            {
                RequiredBinWidth = entity.RequiredBinWidth                
            };
        }

        /// <inheritdoc/>
        public OrderDetailResponseDto ToOrderDetailResponseDto(Order entity)
        {
            if (entity == null)
            {
                return default;
            }

            return new OrderDetailResponseDto
            {
                RequiredBinWidth = entity.RequiredBinWidth,
                Items = ToDto(entity.Items)
            };
        }

        /// <inheritdoc/>
        public Order ToOrderEntity(CreateOrderDto dto)
        {
            if (dto == null)
            {
                return default;
            }

            return new Order
            {
                Items = ToEntity(dto.Items)
            };
        }

        private List<OrderItemDetailResponseDto> ToDto(ICollection<OrderItem> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
            {
                return Enumerable.Empty<OrderItemDetailResponseDto>().ToList();
            }

            return orderItems.Select(i => ToDto(i)).ToList();
        }

        private OrderItemDetailResponseDto ToDto(OrderItem product)
        {
            return new OrderItemDetailResponseDto
            {
                ProductType = product.ProductType,
                Quantity = product.Quantity
            };
        }

        private ICollection<OrderItem> ToEntity(List<CreateOrderItemDto> products)
        {
            if (products == null || !products.Any())
            {
                return Enumerable.Empty<OrderItem>().ToList();
            }

            return products.Select(p => ToEntity(p)).ToList();
        }

        private OrderItem ToEntity(CreateOrderItemDto product)
        {
            return new OrderItem
            {
                ProductType = product.ProductType,
                Quantity = product.Quantity
            };
        }
    }
}
