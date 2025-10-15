using OrdersApi.Entities;
using OrdersApi.Infrastructure.Repositories;

namespace OrdersApi.UnitTests.Infrastructure.Repositories
{
    public class RepositoryTests
    {
        [Fact]
        public async Task AddAsync_Order_Should_Succeed()
        {
            // Arrange
            const int orderId = 1;
            const decimal binWidth = 23m;
            const string productType = "mug";
            const int productQuantity = 2;

            using var dbContext = InfrastructureTestsHelper.GetDbContext();
            var orderRepository = new Repository<Order>(dbContext);

            var order = new Order { 
                Id = orderId,  
                RequiredBinWidth = binWidth, 
                Items = new List<OrderItem> 
                { 
                    new OrderItem
                    {
                         ProductType = productType,
                         Quantity = productQuantity
                    }
                }
            };

            // Act
            await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();

            // Assert
            var savedOrder = await orderRepository.GetByIdAsync(orderId);
            var savedOrderItem = savedOrder?.Items.FirstOrDefault();

            Assert.Multiple(
                () => Assert.NotNull(savedOrder),
                () => Assert.NotNull(savedOrderItem),
                () => Assert.Equal(orderId, savedOrder.Id),
                () => Assert.Equal(binWidth, savedOrder.RequiredBinWidth),
                () => Assert.Single(savedOrder.Items),
                () => Assert.Equal(productType, savedOrderItem.ProductType),
                () => Assert.Equal(productQuantity, savedOrderItem.Quantity)); 
        }

        [Fact]
        public async Task GetAllAsync_Should_Succeed()
        {
            // Arrange
            const int expectedCount = 2;     
            using var dbContext = InfrastructureTestsHelper.GetDbContext();
            var orderRepository = new Repository<Order>(dbContext);
            await orderRepository.AddAsync(new Order { 
                Id = 1,  
                RequiredBinWidth = 14m, 
                Items = new List<OrderItem>
                { 
                    new OrderItem 
                    { 
                        ProductType = "cards",
                        Quantity = 1
                    } 
                } 
            });
            await orderRepository.AddAsync(new Order { Id = 2, RequiredBinWidth = 10m });
            await orderRepository.SaveChangesAsync();

            // Act
            var allOrders = await orderRepository.GetAllAsync();

            // Assert
            Assert.Equal(expectedCount, allOrders.Count());            
        }
    }   
}
