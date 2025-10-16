using FluentValidation;
using OrdersApi.Common;
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
        public CreateProductConfigurationDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.ProductType)
               .NotEmpty().WithMessage(Constants.EmptyProductTypeErrorMessage)
               .MustAsync(async (productType, ct) =>
               {
                   var entity = await productConfigurationRepository
                   .FirstOrDefaultAsync(e => e.ProductType.ToLower() == productType.Trim().ToLower());
                   return (entity == null);
               }).WithMessage(Constants.InvalidItemProductTypeErrorMessage)
               ;

            RuleFor(p => p.Width)
                .GreaterThan(0).WithMessage(Constants.InvalidWidthErrorMessage);

            RuleFor(p => p.NumberOfItemsInStack)
                .GreaterThan(0).WithMessage(Constants.InvalidNumberOfItemsInStackErrorMessage);

        }
    }
}
