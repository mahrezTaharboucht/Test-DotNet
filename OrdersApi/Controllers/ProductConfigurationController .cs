using Microsoft.AspNetCore.Mvc;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Helpers;
using OrdersApi.Interfaces.Services;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductConfigurationController : ControllerBase
    {
        private readonly IProductConfigurationService _productConfigurationService;
        public ProductConfigurationController(IProductConfigurationService productConfigurationService)
        {
            ArgumentNullException.ThrowIfNull(productConfigurationService, nameof(productConfigurationService));
            _productConfigurationService = productConfigurationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productConfigurations = await _productConfigurationService.GetProductConfigurations();            
            return Ok(ApiResponseHelper.Success(string.Empty, data: productConfigurations));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductConfigurationDto createDto)
        {
            var productConfiguration = await _productConfigurationService.CreateProductConfiguration(createDto);
            return Ok(ApiResponseHelper.Success(string.Empty, productConfiguration));
        }
    }
}
