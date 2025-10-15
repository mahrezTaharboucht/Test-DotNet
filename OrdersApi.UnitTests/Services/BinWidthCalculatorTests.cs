using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Services;
using OrdersApi.UnitTests.Mocks;

namespace OrdersApi.UnitTests.Services
{
    public class BinWidthCalculatorTests
    {
        private const string PhotoBook = "photoBook";
        private const string Calendar = "calendar";
        private const string Mug = "mug";
        private const string Canvas = "canvas";
        private const string Cards = "cards";

        private readonly ProductConfigurationMockService _productConfigurationMockService = new();
        private readonly BinWidthCalculator _calculator;

        public BinWidthCalculatorTests()
        {
            _calculator = new BinWidthCalculator(_productConfigurationMockService);
        }

        [Fact]
        public void Ctor_WithNullConfigurationService_ThrowsArgumentNullException()
        {
            // Act
            static BinWidthCalculator act() => new(null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public async Task CalculateBinMinWidth_WithEachProductType_ReturnsCorrectWidth()
        {
            // Arrange
            var expectedWidth = 143.7m;
            var orderItems = new List<CreateOrderItemDto>() {
                new() { ProductType = PhotoBook, Quantity = 1 },
                new() { ProductType = Calendar, Quantity = 1 },
                new() { ProductType = Mug, Quantity = 1 },
                new() { ProductType = Canvas, Quantity = 1 },
                new() { ProductType = Cards, Quantity = 1 }
            };

            // Act
            var minWidth = await _calculator.CalculateBinMinWidth(orderItems);

            // Assert
            Assert.Equal(expectedWidth, minWidth);
        }

        [Theory]
        [InlineData(1, "94")] // 1 mug
        [InlineData(4, "94")] // 4 mug
        [InlineData(5, "188")] // 5 mug
        public async Task CalculateBinMinWidth_WithDifferentMugsQuantites_ReturnsCorrectWidth(int mugsQuantity, string expectedWidthAsString)
        {
            // Arrange
            decimal expectedWidth = decimal.Parse(expectedWidthAsString, System.Globalization.CultureInfo.InvariantCulture);
            var orderItems = new List<CreateOrderItemDto>() {
                new() { ProductType = Mug, Quantity = mugsQuantity }
            };

            // Act
            var minWidth = await _calculator.CalculateBinMinWidth(orderItems);

            // Assert
            Assert.Equal(expectedWidth, minWidth);
        }

        [Fact]
        public async Task CalculateBinMinWidth_WithThreeCards_ReturnsCorrectWidth()
        {
            // Arrange
            var expectedWidth = 14.1m;
            var orderItems = new List<CreateOrderItemDto>() {
                new() { ProductType = Cards, Quantity = 3 }
            };

            // Act
            var minWidth = await _calculator.CalculateBinMinWidth(orderItems);

            // Assert
            Assert.Equal(expectedWidth, minWidth);
        }

        [Fact]
        public async Task CalculateBinMinWidth_WithEmptyItemsList_ReturnsDefaultWidth()
        {
            // Arrange
            var expectedWidth = default(decimal);
            var orderItems = new List<CreateOrderItemDto>();

            // Act
            var minWidth = await _calculator.CalculateBinMinWidth(orderItems);

            // Assert
            Assert.Equal(expectedWidth, minWidth);
        }

        [Fact]
        public async Task CalculateBinMinWidth_WithNullItemsList_ReturnsDefaultWidth()
        {
            // Arrange
            var expectedWidth = default(decimal);
            
            // Act
            var minWidth = await _calculator.CalculateBinMinWidth(null);

            // Assert
            Assert.Equal(expectedWidth, minWidth);
        }
    }
}
