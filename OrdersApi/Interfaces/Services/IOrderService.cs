using OrdersApi.Dtos;

namespace OrdersApi.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrder(CreateOrderDto dto);
        Task<OrderResponseDto> GetOrder(int orderId);
    }
}
