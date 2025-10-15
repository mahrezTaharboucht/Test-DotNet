using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrdersApi.Dtos;

namespace OrdersApi.Filters
{
    /// <summary>
    /// Dto automatic validation filter
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments)
            {
                var argType = argument.Value?.GetType();

                if (argType == null)
                {
                    continue;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(argType);
                var validator = _serviceProvider.GetService(validatorType);

                if (validator is IValidator realValidator)
                {
                    var validationContext = new ValidationContext<object>(argument.Value!);
                    var result = await realValidator.ValidateAsync(validationContext);

                    if (!result.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(
                            new ApiResponse<string>()
                            {
                                Success = false,
                                Message = "Validation error",
                                Errors = result.Errors.Select(e => e.ErrorMessage)
                            });
                        return;
                    }
                }
            }

            await next();
        }
    }
}
