using FluentValidation;
using OrdersApi.Common;
using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;

namespace OrdersApi.Validators
{
    /// <summary>
    /// Create order dto validator.
    /// </summary>
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {       
        public CreateOrderDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.Items)
                .NotEmpty().WithMessage(Constants.InvalidItemsErrorMessage);

            RuleForEach(p => p.Items).SetValidator(new CreateOrderItemDtoValidator(productConfigurationRepository));
        }
    }
}
