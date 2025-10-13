namespace OrdersApi.Entities
{
    public class ProductConfiguration
    {
        public int Id {  get; set; }
        public string ProductType { get; set; }
        public decimal Width { get; set; }
        public int NumberOfItemsInStack { get; set; }
    }
}
