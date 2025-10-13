namespace OrdersApi.Dtos
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public List<OrderItem> Products { get; set; } = new List<OrderItem>();
        public int RequiredBinWidth { get; set; }
    }
}
