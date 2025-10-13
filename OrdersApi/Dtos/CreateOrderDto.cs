namespace OrdersApi.Dtos
{
    public class CreateOrderDto
    {
        public int OrderId { get; set; }
        public List<OrderItem> Products { get; set; } = new List<OrderItem>();
    }
}
