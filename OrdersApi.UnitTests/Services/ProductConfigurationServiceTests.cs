using Microsoft.Extensions.Caching.Memory;
using Moq;
using OrdersApi.Dtos.ProductConfigurations;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Services;

namespace OrdersApi.UnitTests.Services
{
    [Trait("Category", "Unit")]
    public class ProductConfigurationServiceTests : IDisposable
    {
        private const string CacheKey = "ProductConfigurations";
        private readonly Mock<IRepository<ProductConfiguration>> _repositoryMock;
        private readonly Mock<IProductConfigurationMapper> _mapperMock;
        private readonly IMemoryCache _cache;
        private readonly ProductConfigurationService _productionConfigurationService;

        public ProductConfigurationServiceTests()
        {
            _repositoryMock = new Mock<IRepository<ProductConfiguration>>();
            _mapperMock = new Mock<IProductConfigurationMapper>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _productionConfigurationService = new ProductConfigurationService(_repositoryMock.Object, _mapperMock.Object, _cache);
        }

        [Fact]
        public void Ctor_WhenCacheIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new ProductConfigurationService(_repositoryMock.Object, _mapperMock.Object, null);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());           
        }

        [Fact]
        public void Ctor_WhenRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new ProductConfigurationService(null, _mapperMock.Object, _cache);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public void Ctor_WhenMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var act = () => new ProductConfigurationService(_repositoryMock.Object, null, _cache);

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public async Task CreateProductConfiguration_WhenCalled_ShouldCallRequiredServicesToAddEntity()
        {
            // Arrange
            var dto = new CreateProductConfigurationDto();
            var entity = new ProductConfiguration();
            var cachedData = new List<ProductConfigurationDetailResponseDto>();
            _cache.Set(CacheKey, cachedData);
            _mapperMock.Setup(x => x.ToProductConfiguration(dto)).Returns(entity);

            // Act
            await _productionConfigurationService.CreateProductConfiguration(dto);

            // Assert
            _mapperMock.Verify(x => x.ToProductConfiguration(dto), Times.Once);
            _repositoryMock.Verify(x => x.AddAsync(entity), Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            var cacheResult = _cache.Get<List<ProductConfigurationDetailResponseDto>>(CacheKey);
            Assert.Null(cacheResult);            
        }

        [Fact]
        public async Task GetProductConfigurations_WhenCacheDataIsSet_ShouldReturnDataFromCache()
        {
            // Arrange
            var cacheData = new List<ProductConfigurationDetailResponseDto>
            {
                new ProductConfigurationDetailResponseDto(),
                new ProductConfigurationDetailResponseDto()
            };
            _cache.Set(CacheKey, cacheData);           

            // Act
            var result = await _productionConfigurationService.GetProductConfigurations();

            // Assert
            Assert.Equivalent(cacheData, result);            
            _repositoryMock.Verify(x => x.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task GetProductConfigurations_WhenCacheIsEmpty_ShouldCallRequiredServiceToLoadDataFromRepository()
        {
            // Arrange
            var config1 = new ProductConfiguration();
            var config2 = new ProductConfiguration();
            var configs = new List<ProductConfiguration> { config1, config2 };
            _repositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(configs);
            _mapperMock.Setup(x => x.ToProductConfigurationDetailResponseDto(It.IsAny<ProductConfiguration>()))
                .Returns(new ProductConfigurationDetailResponseDto());

            // Act
            var result = await _productionConfigurationService.GetProductConfigurations();
            var cacheResult = _cache.Get<List<ProductConfigurationDetailResponseDto>>(CacheKey);

            // Assert
            Assert.Equivalent(configs.Count(), result.Count()); // Data loaded
            Assert.Equivalent(configs.Count(), cacheResult.Count()); // Cache is up to date
            _repositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
            _mapperMock.Verify(x => x.ToProductConfigurationDetailResponseDto(config1), Times.Once); 
            _mapperMock.Verify(x => x.ToProductConfigurationDetailResponseDto(config2), Times.Once);           
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }
    }
}
