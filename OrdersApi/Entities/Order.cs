namespace OrdersApi.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal RequiredBinWidth { get; set; }
    }
}
