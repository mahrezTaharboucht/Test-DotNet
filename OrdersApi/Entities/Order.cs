namespace OrdersApi.Entities
{
    public class Order
    {
        public Order() 
        {
            Items = new HashSet<OrderItem>();
        }
        public int Id { get; set; }
        public decimal RequiredBinWidth { get; set; }
        public ICollection<OrderItem> Items { get; set; }        
    }
}
