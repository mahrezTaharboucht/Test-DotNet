using OrdersApi.Dtos.Orders;

namespace OrdersApi.Interfaces.Services
{
    /// <summary>
    /// Order service.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create an order with id.
        /// </summary>
        Task<CreateOrderResponseDto> CreateOrder(int orderId, CreateOrderDto dto);
        
        /// <summary>
        /// Get order details.
        /// </summary>    
        Task<OrderDetailResponseDto> GetOrder(int orderId);
    }
}
