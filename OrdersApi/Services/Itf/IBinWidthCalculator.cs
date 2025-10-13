using OrdersApi.Entities;
namespace OrdersApi.Services.Itf
{
    /// <summary>
    /// Bin width calculator.
    /// </summary>
    public interface IBinWidthCalculator
    {
        /// <summary>
        /// Calculate the minimum required bin width for a given order items list. 
        /// </summary>
        /// <param name="items">Order items.</param>
        /// <returns>Minimum width.</returns>
        public Task<decimal> CalculateBinMinWidth(List<OrderItem> items);
    }
}
