using Domain.DTOs.OrderDishDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.OrderDishService;

public interface IOrderDishService
{
    Task<PagedResponse<List<GetOrderDishDto>>> GetOrderDishsAsync(OrderDishFilter filter);
    Task<Response<GetOrderDishDto>> GetOrderDishByIdAsync(int orderDishId);
    Task<Response<string>> CreateOrderDishAsync(CreateOrderDishDto createOrderDish);
    Task<Response<string>> UpdateOrderDishAsync(UpdateOrderDishDto updateOrderDish);
    Task<Response<bool>> DeleteOrderDishAsync(int orderDishId);
}
