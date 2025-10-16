using FluentValidation;
using OrdersApi.Common;
using OrdersApi.Dtos.ProductConfigurations;

namespace OrdersApi.Validators
{
    /// <summary>
    /// Create product configuration dto validator.
    /// </summary>
    public class CreateProductConfigurationDtoValidator : AbstractValidator<CreateProductConfigurationDto>
    {       
        public CreateProductConfigurationDtoValidator()
        {
            RuleFor(p => p.ProductType)
               .NotEmpty().WithMessage(Constants.EmptyProductTypeErrorMessage);

            RuleFor(p => p.Width)
                .GreaterThan(0).WithMessage(Constants.InvalidWidthErrorMessage);

            RuleFor(p => p.NumberOfItemsInStack)
                .GreaterThan(0).WithMessage(Constants.InvalidNumberOfItemsInStackErrorMessage);

        }
    }
}
