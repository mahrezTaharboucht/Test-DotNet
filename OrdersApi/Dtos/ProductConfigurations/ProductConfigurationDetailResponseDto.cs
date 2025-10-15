namespace OrdersApi.Dtos.ProductConfigurations
{
    public class ProductConfigurationDetailResponseDto
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public decimal Width { get; set; }
        public int NumberOfItemsInStack { get; set; }
    }
}
