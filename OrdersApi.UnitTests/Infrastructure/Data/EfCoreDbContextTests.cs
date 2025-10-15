namespace OrdersApi.UnitTests.Infrastructure.Data
{
    public class EfCoreDbContextTests
    {

        [Fact]
        public void DbContext_WhenCalled_ShouldCreateModel()
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
