using OrdersApi.Dtos;

namespace OrdersApi.Services.Itf
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrder(CreateOrderDto dto);
        Task<OrderResponseDto> GetOrder(int orderId);
    }
}
