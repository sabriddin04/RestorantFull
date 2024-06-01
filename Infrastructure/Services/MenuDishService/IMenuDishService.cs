using Domain.DTOs.MenuDishDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MenuDishService;

public interface IMenuDishService
{
    Task<PagedResponse<List<GetMenuDishDto>>> GetMenuDishsAsync(MenuDishFilter filter);
    Task<Response<GetMenuDishDto>> GetMenuDishByIdAsync(int menuDishId);
    Task<Response<string>> CreateMenuDishAsync(CreateMenuDishDto createMenuDish);
    Task<Response<string>> UpdateMenuDishAsync(UpdateMenuDishDto updateMenuDish);
    Task<Response<bool>> DeleteMenuDishAsync(int menuDishId);
}
