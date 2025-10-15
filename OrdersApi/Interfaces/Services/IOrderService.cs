using OrdersApi.Dtos.Orders;

namespace OrdersApi.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDetailResponseDto> CreateOrder(CreateOrderDto dto);
        Task<OrderDetailResponseDto> GetOrder(int orderId);
    }
}
