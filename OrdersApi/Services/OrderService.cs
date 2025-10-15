using OrdersApi.Dtos.Orders;
using OrdersApi.Entities;
using OrdersApi.Interfaces.Mappers;
using OrdersApi.Interfaces.Repositories;
using OrdersApi.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Services
{
    /// <inheritdoc/>
    public class OrderService : IOrderService
    {
        private const string InvalidOrderIdErrorMessage = "The order Id should be greater than 0.";
        private const string ExistingOrderIdErrorMessage = "The order already exist.";
        private readonly IRepository<Order> _orderRepository;
        private readonly IBinWidthCalculator _widthCalculator;
        private readonly IOrderMapper _orderMapper;

        /// <summary>
        /// Ctor.
        /// </summary>        
        public OrderService(IRepository<Order> orderRepository,
            IBinWidthCalculator widthCalculator,
            IOrderMapper orderMapper)
        {
            ArgumentNullException.ThrowIfNull(orderRepository, nameof(orderRepository));
            ArgumentNullException.ThrowIfNull(widthCalculator, nameof(widthCalculator));
            ArgumentNullException.ThrowIfNull(orderMapper, nameof(orderMapper));

            _orderRepository = orderRepository;
            _widthCalculator = widthCalculator;
            _orderMapper = orderMapper;
        }

        /// <inheritdoc/>
        public async Task<CreateOrderResponseDto> CreateOrder(int orderId, CreateOrderDto dto)
        {
            if (orderId <= 0)
            {
                throw new ValidationException(InvalidOrderIdErrorMessage);
            }

            if (await _orderRepository.Exists(orderId))
            {
                throw new ValidationException(ExistingOrderIdErrorMessage);
            }

            var order = _orderMapper.ToOrderEntity(dto);
            order.Id = orderId;
            order.RequiredBinWidth = await _widthCalculator.CalculateBinMinWidth(dto.Items);
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            return _orderMapper.ToCreateOrderResponseDto(order);            
        }

        /// <inheritdoc/>
        public async Task<OrderDetailResponseDto> GetOrder(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return _orderMapper.ToOrderDetailResponseDto(order);
        }
    }
}
