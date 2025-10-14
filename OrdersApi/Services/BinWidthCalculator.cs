using OrdersApi.Entities;
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
        /// <param name="productConfigurationService">Product configuration service instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if configuration service is null.</exception>
        public BinWidthCalculator(IProductConfigurationService productConfigurationService) 
        {
            if (productConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(productConfigurationService));
            }

            _productConfigurationService = productConfigurationService;
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateBinMinWidth(List<OrderItem> items)
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
                        Width = producConfig?.Width,
                        NumberOfItemsInStack = producConfig?.NumberOfItemsInStack
                    };

                }).ToList();

            foreach (var item in groupedItems)
            {
                var requiredStacksAsDecimal = item.Quantity / (decimal)item.NumberOfItemsInStack;
                // Ceil to the nearest integer 
                var requiredStacks = (int)Math.Ceiling(requiredStacksAsDecimal);

                width = width + (decimal)(requiredStacks * item.Width);
            }

            return width;
        }
    }
}
