using FluentValidation;
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
        private const string InvalidItemsErrorMessage = "Order items should contain at least one element.";
        public CreateOrderDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.Items)
                .NotEmpty().WithMessage(InvalidItemsErrorMessage);

            RuleForEach(p => p.Items).SetValidator(new CreateOrderItemDtoValidator(productConfigurationRepository));
        }
    }
}
