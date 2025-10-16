using Microsoft.Extensions.Caching.Memory;
using OrdersApi.Common;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Exceptions;
using OrdersApi.Infrastructure.Repositories;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Interfaces.Services;
using System.Xml;

namespace OrdersApi.Services
{
    /// <inheritdoc/>
    public class ProductConfigurationService : IProductConfigurationService
    {
        private const string CacheKey = "ProductConfigurations";
        private const int CacheExpirationInHours = 2;
        private readonly IRepository<ProductConfiguration> _productConfigurationRepository;
        private readonly IProductConfigurationMapper _productConfigurationMapper;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Ctor.
        /// </summary>        
        public ProductConfigurationService(
            IRepository<ProductConfiguration> productConfigurationRepository,
            IProductConfigurationMapper productConfigurationMapper,
            IMemoryCache cache)
        {
            ArgumentNullException.ThrowIfNull(cache, nameof(cache));
            ArgumentNullException.ThrowIfNull(productConfigurationRepository, nameof(productConfigurationRepository));
            ArgumentNullException.ThrowIfNull(productConfigurationMapper, nameof(productConfigurationMapper));

            _productConfigurationRepository = productConfigurationRepository;
            _productConfigurationMapper = productConfigurationMapper;
            _cache = cache;
        }

        /// <inheritdoc/>
        public async Task<ProductConfigurationDetailResponseDto> CreateProductConfiguration(CreateProductConfigurationDto productConfiguration)
        {
            var entityFromDb = await _productConfigurationRepository
                   .FirstOrDefaultAsync(e => e.ProductType.ToLower() == productConfiguration.ProductType.Trim().ToLower());
           
            if (entityFromDb != null)
            {
                throw new ConflictException(Constants.InvalidItemProductTypeErrorMessage);
            }

            var entity = _productConfigurationMapper.ToProductConfiguration(productConfiguration);
            await _productConfigurationRepository.AddAsync(entity);
            await _productConfigurationRepository.SaveChangesAsync();
            _cache.Remove(CacheKey);
            return _productConfigurationMapper.ToProductConfigurationDetailResponseDto(entity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductConfigurationDetailResponseDto>> GetProductConfigurations()
        {
            if (!_cache.TryGetValue(CacheKey, out List<ProductConfigurationDetailResponseDto> productConfigurationsDtos))
            {
                var entities = await _productConfigurationRepository.GetAllAsync();
                productConfigurationsDtos = entities.Select(x => _productConfigurationMapper.ToProductConfigurationDetailResponseDto(x)).ToList();
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(CacheExpirationInHours));
                _cache.Set(CacheKey, productConfigurationsDtos, options);
            }

            return productConfigurationsDtos;
        }
    }
}
