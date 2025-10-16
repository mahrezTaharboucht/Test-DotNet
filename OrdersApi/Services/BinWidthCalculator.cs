using OrdersApi.Dtos.Orders;
using OrdersApi.Interfaces.Services;
namespace OrdersApi.Services
{
    /// <inheritdoc/>
    public class BinWidthCalculator : IBinWidthCalculator
    {       
        private readonly IProductConfigurationService _productConfigurationService;
       
        /// <summary>
        /// Ctor.
        /// </summary>
        public BinWidthCalculator(IProductConfigurationService productConfigurationService) 
        {
            ArgumentNullException.ThrowIfNull(productConfigurationService, nameof(productConfigurationService));           
            _productConfigurationService = productConfigurationService;
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateBinMinWidth(List<CreateOrderItemDto> items)
        {
            if (items == null || items.Count == 0)
            {
                return default;
            }

            var width = 0m;
            var productConfigurations = await _productConfigurationService.GetProductConfigurations();
            
            // Group by product type and add configuration data
            var groupedItems = items
                .GroupBy(g => g.ProductType)
                .Select(g => {
                    var producConfig = productConfigurations.FirstOrDefault(pc => pc.ProductType.Equals(g.Key, StringComparison.OrdinalIgnoreCase));
                    return new {
                        Type = g.Key,
                        Quantity = g.Sum(o => o.Quantity),
                        producConfig?.Width,
                        producConfig?.NumberOfItemsInStack
                    };

                }).ToList();

            foreach (var item in groupedItems)
            {
                var requiredStacksAsDecimal = item.Quantity / (decimal)item.NumberOfItemsInStack;
                // Ceil to the nearest integer 
                var requiredStacks = (int)Math.Ceiling(requiredStacksAsDecimal);

                width += (decimal)(requiredStacks * item.Width);
            }

            return width;
        }
    }
}
