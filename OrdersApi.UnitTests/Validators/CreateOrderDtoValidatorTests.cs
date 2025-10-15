using FluentValidation.TestHelper;
using Moq;
using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Validators;
using System.Linq.Expressions;
namespace OrdersApi.UnitTests.Validators
{
    public class CreateOrderDtoValidatorTests
    {
        private const string ProductType = "Cards";
        private readonly Mock<IRepository<ProductConfiguration>> _mockProductConfigRepository;
        private readonly CreateOrderDtoValidator _validator;

        public CreateOrderDtoValidatorTests()
        {
            _mockProductConfigRepository = new Mock<IRepository<ProductConfiguration>>();
           
            _mockProductConfigRepository
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                .ReturnsAsync(new ProductConfiguration { ProductType = ProductType });
            _validator = new CreateOrderDtoValidator(_mockProductConfigRepository.Object);
        }

        [Fact]
        public async Task TestValidateAsync_WithEmptyItemsList_ReturnsValidationError()
        {
            // Arrange
            var expectedError = "Order items should contain at least one element.";
            var model = new CreateOrderDto { Items = new List<CreateOrderItemDto>() };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);           
        }

        [Fact]
        public async Task TestValidateAsync_WithInvalidItemProductType_ReturnsValidationError()
        {    
            // Arrange
            _mockProductConfigRepository
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                .ReturnsAsync((ProductConfiguration)null);

            var model = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new() { Quantity = 1, ProductType = "InvalidType" }
                }
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Single(result.Errors);
        }

        [Fact]
        public async Task TestValidateAsync_WithValidCreateOrderDto_ReturnsNoValidationError()
        {
            // Assert
            var model = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                    {
                        new() { Quantity = 2, ProductType = ProductType }
                    }
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Empty(result.Errors);
        }
    }
}
