namespace OrdersApi.UnitTests.Infrastructure.Data
{
    public class EfCoreDbContextTests
    {

        [Fact]
        public void Should_Create_Model_Successfully()
        {
            // Arrange
            using var context = InfrastructureTestsHelper.GetDbContext();
            
            // Act
            var model = context.Model;

            // Assert
            Assert.NotNull(model);
        }     
    }
}
