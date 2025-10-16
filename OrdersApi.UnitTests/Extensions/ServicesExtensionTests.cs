using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrdersApi.Dtos;
using OrdersApi.Dtos.Orders;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Extensions;
using OrdersApi.Infrastructure.Data;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.UnitTests.Extensions
{
    [Trait("Category", "Unit")]
    public class ServicesExtensionTests
    {
        private const string DbName = "TestDb";
        [Fact]
        public void ConfigureAutoValidationBehaviour_InvalidModelStateResponseFactory_ShouldReturnBadRequestWithCustomFormat()
        {
            // Arrange
            var services = CreateServiceCollection();
            services.ConfigureAutoValidationBehaviour();
            var provider = services.BuildServiceProvider();
            var options = provider.GetService<IOptions<ApiBehaviorOptions>>().Value;

            var actionContext = new ActionContext
            {
                ModelState = { }
            };
            actionContext.ModelState.AddModelError("Quantity", "Invalid value");
            actionContext.ModelState.AddModelError("Width", "Wrong value");

            // Act
            var result = options.InvalidModelStateResponseFactory(actionContext);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
        }


        [Fact]
        public void ConfigureMappers_WhenCalled_ShouldRegisterMappers()
        {
            // Arrange
            var services = CreateServiceCollection();

            // Act
            services.ConfigureMappers();
            var provider = services.BuildServiceProvider();

            // Assert
            var orderMapper = provider.GetService<IOrderMapper>();
            var productConfigurationMapper = provider.GetService<IProductConfigurationMapper>();

            Assert.Multiple(
                () => Assert.NotNull(orderMapper),
                () => Assert.NotNull(productConfigurationMapper));          
        }


        [Fact]
        public void ConfigureRepositories_WhenCalled_ShouldRegisterRepositories()
        {
            // Arrange
            var services = CreateServiceCollection();
            services.AddDbContext<OrdersApiDbContext>(options =>
               options.UseInMemoryDatabase(DbName));

            // Act
            services.ConfigureRepositories();
            var provider = services.BuildServiceProvider();

            // Assert
            var orderRepository = provider.GetService<IRepository<Order>>();
            var productConfigurationRepository = provider.GetService<IRepository<ProductConfiguration>>();

            Assert.Multiple(
                () => Assert.NotNull(orderRepository),
                () => Assert.NotNull(productConfigurationRepository));
        }

        [Fact]
        public void ConfigureValidators_WhenCalled_ShouldRegisterValidators()
        {
            // Arrange
            var services = CreateServiceCollection();
            services.AddDbContext<OrdersApiDbContext>(options =>
               options.UseInMemoryDatabase(DbName));
            services.ConfigureRepositories();

            // Act
            services.ConfigureValidators();
            var provider = services.BuildServiceProvider();

            // Assert
            var orderItemValidator = provider.GetService<IValidator<CreateOrderItemDto>>();
            var orderValidator = provider.GetService<IValidator<CreateOrderDto>>();
            var productConfigurationvalidator = provider.GetService<IValidator<CreateProductConfigurationDto>>();

            Assert.Multiple(
                () => Assert.NotNull(orderItemValidator),
                () => Assert.NotNull(orderValidator),
                () => Assert.NotNull(productConfigurationvalidator));
        }

        [Fact]
        public void ConfigureBusinessServices_WhenCalled_ShouldRegisterServices()
        {
            // Arrange
            var services = CreateServiceCollection();
            services.AddDbContext<OrdersApiDbContext>(options =>
                options.UseInMemoryDatabase(DbName));
            services.AddMemoryCache();
            services.ConfigureRepositories();
            services.ConfigureMappers();

            // Act
            services.ConfigureBusinessServices();
            var provider = services.BuildServiceProvider();

            // Assert
            var productConfigurationService = provider.GetService<IProductConfigurationService>();
            var orderService = provider.GetService<IOrderService>();
            var binWidthCalculatorService = provider.GetService<IBinWidthCalculator>();

            Assert.Multiple(
               () => Assert.NotNull(productConfigurationService),
               () => Assert.NotNull(orderService),
               () => Assert.NotNull(binWidthCalculatorService));           
        }

        private IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }        
    }
}
