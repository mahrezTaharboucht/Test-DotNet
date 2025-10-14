namespace OrdersApi.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
