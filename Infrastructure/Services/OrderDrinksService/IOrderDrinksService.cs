using Domain.DTOs.OrderDrinksDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.OrderDrinksService;

public interface IOrderDrinksService
{
    Task<PagedResponse<List<GetOrderDrinksDto>>> GetOrderDrinksesAsync(OrderDrinksFilter filter);
    Task<Response<GetOrderDrinksDto>> GetOrderDrinksByIdAsync(int orderDrinksId);
    Task<Response<string>> CreateOrderDrinksAsync(CreateOrderDrinksDto createOrderDrinks);
    Task<Response<string>> UpdateOrderDrinksAsync(UpdateOrderDrinksDto updateOrderDrinks);
    Task<Response<bool>> DeleteOrderDrinksAsync(int orderDrinksId);
}
