using FluentValidation.TestHelper;
using Moq;
using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Validators;
using System.Linq.Expressions;

namespace OrdersApi.UnitTests.Validators
{
    public class CreateOrderItemDtoValidatorTests
    {
        private const string ProductType = "Mug";
        private readonly Mock<IRepository<ProductConfiguration>> _mockProductConfigRepository;
        private readonly CreateOrderItemDtoValidator _validator;

        public CreateOrderItemDtoValidatorTests()
        {
            _mockProductConfigRepository = new Mock<IRepository<ProductConfiguration>>();            
            _mockProductConfigRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                     .ReturnsAsync(new ProductConfiguration { ProductType = ProductType });
            _validator = new CreateOrderItemDtoValidator(_mockProductConfigRepository.Object);
        }

        [Fact]
        public async Task TestValidateAsync_WhenQuantityIsNotValid_ShouldReturnValidationError()
        {
            // Arrange
            var expectedError = "Item quantity should be greater than 0.";
            var model = new CreateOrderItemDto { Quantity = 0, ProductType = ProductType };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenItemProductTypeIsNotValid_ShouldReturnValidationError()
        {
            // Arrange
            var expectedError = "Unknown product type.";
            _mockProductConfigRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                    .ReturnsAsync((ProductConfiguration)null);

            var model = new CreateOrderItemDto { Quantity = 1, ProductType = "UnknownType" };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenCreateOrderItemDtoIsValid_ShouldNotReturnValidationError()
        {
            // Arrange
            var model = new CreateOrderItemDto { Quantity = 5, ProductType = ProductType };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Empty(result.Errors);           
        }

    }
}
