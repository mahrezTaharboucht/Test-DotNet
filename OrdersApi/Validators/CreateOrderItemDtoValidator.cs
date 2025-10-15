using FluentValidation;
using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Repositories;
namespace OrdersApi.Validators
{
    /// <summary>
    /// Create order item validator.
    /// </summary>
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        private const string InvalidQuantityErrorMessage = "Item quantity should be greater than 0.";
        private const string InvalidProductTypeErrorMessage = "Unknown product type.";
        public CreateOrderItemDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage(InvalidQuantityErrorMessage);

            RuleFor(p => p.ProductType)
                .MustAsync(async (productType, ct) =>
                {
                    var entity = await productConfigurationRepository
                    .FirstOrDefaultAsync(e => e.ProductType.ToLower() == productType.ToLower());
                    return (entity != null);
                }).WithMessage(InvalidProductTypeErrorMessage);
        }
    }
}
