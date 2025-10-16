using FluentValidation;
using OrdersApi.Common;
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
        public CreateOrderItemDtoValidator(IRepository<ProductConfiguration> productConfigurationRepository)
        {
            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage(Constants.InvalidQuantityErrorMessage);

            RuleFor(p => p.ProductType)
                .MustAsync(async (productType, ct) =>
                {
                    var entity = await productConfigurationRepository
                    .FirstOrDefaultAsync(e => e.ProductType.ToLower() == productType.ToLower());
                    return (entity != null);
                }).WithMessage(Constants.InvalidProductTypeErrorMessage);
        }
    }
}
