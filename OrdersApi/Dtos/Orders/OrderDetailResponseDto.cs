namespace OrdersApi.Dtos.Orders
{
    public class OrderDetailResponseDto : CreateOrderResponseDto
    {       
        public List<OrderItemDetailResponseDto> Items { get; set; } = [];        
    }
}
