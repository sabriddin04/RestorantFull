using Domain.DTOs.MenuDrinksDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MenuDrinksService;

public interface IMenuDrinksService
{
    Task<PagedResponse<List<GetMenuDrinksDto>>> GetMenuDrinksesAsync(MenuDrinksFilter filter);
    Task<Response<GetMenuDrinksDto>> GetMenuDrinksByIdAsync(int menuDrinksId);
    Task<Response<string>> CreateMenuDrinksAsync(CreateMenuDrinksDto createMenuDrinks);
    Task<Response<string>> UpdateMenuDrinksAsync(UpdateMenuDrinksDto updateMenuDrinks);
    Task<Response<bool>> DeleteMenuDrinksAsync(int menuDrinksId);
}
