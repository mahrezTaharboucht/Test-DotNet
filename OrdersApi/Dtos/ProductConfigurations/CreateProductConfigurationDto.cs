namespace OrdersApi.Dtos.ProductConfigurations
{
    public class CreateProductConfigurationDto
    {
        public string ProductType { get; set; }
        public decimal Width { get; set; }
        public int NumberOfItemsInStack { get; set; }
    }
}
