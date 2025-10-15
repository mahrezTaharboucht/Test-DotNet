using OrdersApi.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrdersApi.UnitTests.Helpers
{
    public class ApiResponseHelperTests
    {
        [Fact]
        public void Failure_WhenCalled_ShouldReturnResponseWithExpectedValues()
        {
            // Arrange
            const string message = "Validation error";
            var errors = new List<string> { "Invalid quantity", "Invalid width" };

            // Act
            var result = ApiResponseHelper.Failure<string>(message, errors);

            // Assert
            Assert.Multiple(
                () => Assert.False(result.Success),
                () => Assert.Equal(message, result.Message),
                () => Assert.Equivalent(errors, result.Errors));
        }

        [Fact]
        public void Success_WhenCalled_ShouldReturnResponseWithExpectedValues()
        {
            // Arrange
            var message = "Order created";
            var data = "data id";

            // Act
            var result = ApiResponseHelper.Success(message, data);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.Success),
                () => Assert.Equal(message, result.Message),
                () => Assert.Equal(data, result.Data));
        }
    }
}
