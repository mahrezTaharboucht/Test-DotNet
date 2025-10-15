namespace OrdersApi.Dtos.Orders
{
    public class CreateOrderItemDto
    {
        public string ProductType { get; set; }
        public int Quantity { get; set; }
    }
}
