using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Common;
using OrdersApi.Dtos.Orders;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Helpers;
using OrdersApi.Infrastructure.Data;
using OrdersApi.Infrastructure.Repositories;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Interfaces.Services;
using OrdersApi.Mappers;
using OrdersApi.Services;
using OrdersApi.Validators;

namespace OrdersApi.Extensions
{
    /// <summary>
    /// IServiceCollection extension methods.
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// Configure database context.
        /// </summary>        
        public static void ConfigureDatabase(this IServiceCollection services, IConfigurationManager configuration)
        {
            var connectionString = configuration?.GetConnectionString(Constants.ConnectionStringKey);
            services?.AddDbContext<OrdersApiDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        /// <summary>
        /// Catch auto validation errors and use custom error responses.
        /// </summary>        
        public static void ConfigureAutoValidationBehaviour(this IServiceCollection services)
        {
            services?.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    
                    var result = new BadRequestObjectResult(ApiResponseHelper.Failure<string>(Constants.ValidationExceptionMessage, errors.SelectMany(e => e.Value)));
                    return result;
                };
            });
        }

        /// <summary>
        /// Register entities/dtos mappers.
        /// </summary>        
        public static void ConfigureMappers(this IServiceCollection services)
        {
            services?.AddSingleton<IOrderMapper, OrderMapper>();
            services?.AddSingleton<IProductConfigurationMapper, ProductConfigurationMapper>();
        }

        /// <summary>
        /// Register repositories.
        /// </summary>
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services?.AddScoped<IRepository<Order>, OrdersRepository>();
            services?.AddScoped<IRepository<ProductConfiguration>, ProductConfigurationRepository>();
        }

        /// <summary>
        /// Register dto validators.
        /// </summary>        
        public static void ConfigureValidators(this IServiceCollection services)
        {           
            services?.AddScoped<IValidator<CreateOrderItemDto>, CreateOrderItemDtoValidator>();
            services?.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();
            services?.AddScoped<IValidator<CreateProductConfigurationDto>, CreateProductConfigurationDtoValidator>();
        }

        /// <summary>
        /// Register business services.
        /// </summary>       
        public static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services?.AddScoped<IBinWidthCalculator, BinWidthCalculator>();
            services?.AddScoped<IOrderService, OrderService>();
            services?.AddScoped<IProductConfigurationService, ProductConfigurationService>();
        }
    }
}
