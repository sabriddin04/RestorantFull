using Domain.DTOs.DishDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.DishService;

public interface IDishService
{
    Task<PagedResponse<List<GetDishDto>>> GetDishesAsync(DishFilter filter);
    Task<Response<GetDishDto>> GetDishByIdAsync(int dishId);
    Task<Response<string>> CreateDishAsync(CreateDishDto createDish);
    Task<Response<string>> UpdateDishAsync(UpdateDishDto updateDish);
    Task<Response<bool>> DeleteDishAsync(int dishId);
}
