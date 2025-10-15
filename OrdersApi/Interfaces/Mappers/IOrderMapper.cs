using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;

namespace OrdersApi.Interfaces.Mappers
{
    /// <summary>
    /// Order entity/dto mapper. 
    /// </summary>
    public interface IOrderMapper
    {
        /// <summary>
        /// Create Order from CreateOrderDto.
        /// </summary>       
        Order ToOrderEntity(CreateOrderDto dto);

        /// <summary>
        /// Create OrderDetailResponseDto from Order.
        /// </summary>        
        OrderDetailResponseDto ToOrderDetailResponseDto(Order entity);

        /// <summary>
        /// Create CreateOrderResponseDto from Order.
        /// </summary>        
        CreateOrderResponseDto ToCreateOrderResponseDto(Order entity);
    }
}
