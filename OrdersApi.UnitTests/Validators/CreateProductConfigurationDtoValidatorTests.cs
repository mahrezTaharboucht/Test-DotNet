using FluentValidation.TestHelper;
using Moq;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Validators;
using System.Linq.Expressions;

namespace OrdersApi.UnitTests.Validators
{
    [Trait("Category", "Unit")]
    public class CreateProductConfigurationDtoValidatorTests
    {
        private readonly Mock<IRepository<ProductConfiguration>> _mockProductConfigRepository;
        private readonly CreateProductConfigurationDtoValidator _validator;
        public CreateProductConfigurationDtoValidatorTests()
        {
            _mockProductConfigRepository = new Mock<IRepository<ProductConfiguration>>();
            _mockProductConfigRepository
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                .ReturnsAsync((ProductConfiguration)null);
            _validator = new CreateProductConfigurationDtoValidator(_mockProductConfigRepository.Object);
        }

        [Fact]
        public async Task TestValidateAsync_WhenProductTypeAlreadyExist_ShouldReturnValidationError()
        {
            // Arrange
            const string productType = "Mug";
            const string expectedError = "The given product type value already exists.";
            _mockProductConfigRepository
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<ProductConfiguration, bool>>>()))
                .ReturnsAsync(new ProductConfiguration { ProductType = productType });

            var model = new CreateProductConfigurationDto
            {
                ProductType = productType,
                Width = 10,
                NumberOfItemsInStack = 5
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenProductTypeIsEmpty_ShouldReturnValidationError()
        {
            // Arrange
            const string expectedError = "Product type should be provided.";
            var model = new CreateProductConfigurationDto
            {
                ProductType = "",
                Width = 1,
                NumberOfItemsInStack = 5
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenWidthIsNotValid_ShouldReturnValidationError()
        {
            // Arrange
            const string expectedError = "Width should be greater than 0.";
            var model = new CreateProductConfigurationDto
            {
                ProductType = "cards",
                Width = 0,
                NumberOfItemsInStack = 5
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenNumberOfItemsInStackIsNotValid_ShouldReturnValidationError()
        {
            // Arrange
            const string expectedError = "The number of items in stack should be greater than 0.";
            var model = new CreateProductConfigurationDto
            {
                ProductType = "mug",
                Width = 10,
                NumberOfItemsInStack = 0
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Contains(expectedError, result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TestValidateAsync_WhenCreateProductConfigurationDtoIsValid_ShouldNotReturnValidationError()
        {
            // Arrange
            var model = new CreateProductConfigurationDto
            {
                ProductType = "photo",
                Width = 10,
                NumberOfItemsInStack = 5
            };

            // Act
            var result = await _validator.TestValidateAsync(model);

            // Assert
            Assert.Empty(result.Errors);
        }
    }
}
