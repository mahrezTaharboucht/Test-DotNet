using OrdersApi.Dtos.Orders;
namespace OrdersApi.Interfaces.Services
{
    /// <summary>
    /// Bin width calculator.
    /// </summary>
    public interface IBinWidthCalculator
    {
        /// <summary>
        /// Calculate the minimum required bin width for a given order items list. 
        /// </summary>        
        public Task<decimal> CalculateBinMinWidth(List<CreateOrderItemDto> items);
    }
}
