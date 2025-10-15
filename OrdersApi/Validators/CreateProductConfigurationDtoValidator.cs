using FluentValidation;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;

namespace OrdersApi.Validators
{
    /// <summary>
    /// Create product configuration dto validator.
    /// </summary>
    public class CreateProductConfigurationDtoValidator : AbstractValidator<CreateProductConfigurationDto>
    {
        private const string InvalidProductTypeErrorMessage = "The given product type value already exists.";
        private const string InvalidWidthErrorMessage = "Width should be greater than 0.";
        private const string InvalidNumberOfItemsInStackErrorMessage = "The number of items in stack should be greater than 0.";
        public CreateProductConfigurationDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.ProductType)
               .MustAsync(async (productType, ct) =>
               {
                   var entity = await productConfigurationRepository
                   .FirstOrDefaultAsync(e => e.ProductType.ToLower() == productType.Trim().ToLower());
                   return (entity == null);
               }).WithMessage(InvalidProductTypeErrorMessage);

            RuleFor(p => p.Width)
                .GreaterThan(0).WithMessage(InvalidWidthErrorMessage);

            RuleFor(p => p.NumberOfItemsInStack)
                .GreaterThan(0).WithMessage(InvalidNumberOfItemsInStackErrorMessage);

        }
    }
}
