using Microsoft.AspNetCore.Mvc;
using OrdersApi.Common;
using OrdersApi.Dtos.Orders;
using OrdersApi.Helpers;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
            _orderService = orderService;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = await _orderService.GetOrder(orderId);
            if (order == null)
            {                
                return NotFound(ApiResponseHelper.Failure<string>(Constants.MissingOrderError, new List<string> { Constants.MissingOrderErrorMessage }));
            }
            
            return Ok(ApiResponseHelper.Success(string.Empty, data: order));
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> Create(int orderId, CreateOrderDto createOrderDto)
        {           
            var createOrderResponse = await _orderService.CreateOrder(orderId, createOrderDto);            
            return Ok(ApiResponseHelper.Success(string.Empty, data: createOrderResponse));
        }
    }
}
