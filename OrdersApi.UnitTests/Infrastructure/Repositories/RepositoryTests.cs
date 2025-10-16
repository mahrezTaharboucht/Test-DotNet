using Microsoft.EntityFrameworkCore;
using OrdersApi.Entities;
using OrdersApi.Infrastructure.Repositories;

namespace OrdersApi.UnitTests.Infrastructure.Repositories
{
    [Trait("Category", "Unit")]
    public class RepositoryTests
    {
        [Fact]
        public async Task AddAsync_WhenCalled_ShouldReturnOrder()
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
        public async Task GetAllAsync_WhenCalled_ShouldReturnAllOrders()
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

        [Fact]
        public async Task GetAsync_WhenCalled_ShouldReturnOrder()
        {
            // Arrange            
            const int id = 10;
            const decimal binWidth = 14m;
            const string productType = "photo";
            const int productQuantity = 3;
            using var dbContext = InfrastructureTestsHelper.GetDbContext();
            var orderRepository = new Repository<Order>(dbContext);
            await orderRepository.AddAsync(new Order
            {
                Id = id,
                RequiredBinWidth = binWidth,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductType = productType,
                        Quantity = productQuantity
                    }
                }
            });    
            await orderRepository.SaveChangesAsync();

            // Act
            var order = await orderRepository.GetAsync(q => q.Include(e => e.Items).Where(e => e.Id == id));
            var orderItem = order?.Items.FirstOrDefault();

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(order),
                () => Assert.NotNull(orderItem),
                () => Assert.Equal(id, order.Id),
                () => Assert.Equal(binWidth, order.RequiredBinWidth),
                () => Assert.Single(order.Items),
                () => Assert.Equal(productType, orderItem.ProductType),
                () => Assert.Equal(productQuantity, orderItem.Quantity));
        }
    }   
}
