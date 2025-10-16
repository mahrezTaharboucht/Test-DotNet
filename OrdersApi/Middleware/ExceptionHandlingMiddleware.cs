using OrdersApi.Common;
using OrdersApi.Exceptions;
using OrdersApi.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OrdersApi.Middleware
{
    /// <summary>
    /// Middleware used to catch validation and unhandled exceptions.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
       
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await context.Response.WriteAsJsonAsync(ApiResponseHelper.Failure<string>(Constants.ConflicExceptionMessage, new List<string> { ex.Message }));
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;               
                await context.Response.WriteAsJsonAsync(ApiResponseHelper.Failure<string>(Constants.ValidationExceptionMessage, new List<string> { ex.Message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Constants.ExceptionMiddleware);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(ApiResponseHelper.Failure<string>(Constants.GlobalExceptionMessage, new List<string>()));
            }
        }
    }
}
