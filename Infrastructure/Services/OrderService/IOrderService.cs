using Domain.DTOs.OrderDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.OrderService;

public interface IOrderService
{
    Task<PagedResponse<List<GetOrderDto>>> GetOrdersAsync(OrderFilter filter);
    Task<Response<GetOrderDto>> GetOrderByIdAsync(int orderId);
    Task<Response<string>> CreateOrderAsync(CreateOrderDto createOrder);
    Task<Response<string>> UpdateOrderAsync(UpdateOrderDto updateOrder);
    Task<Response<bool>> DeleteOrderAsync(int orderId);
}
