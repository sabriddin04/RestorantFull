using Domain.DTOs.DrinksDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.DrinksService;

public interface IDrinksService
{
    Task<PagedResponse<List<GetDrinksDto>>> GetDrinksesAsync(DrinksFilter filter);
    Task<Response<GetDrinksDto>> GetDrinksByIdAsync(int drinksId);
    Task<Response<string>> CreateDrinksAsync(CreateDrinksDto createDrinks);
    Task<Response<string>> UpdateDrinksAsync(UpdateDrinksDto updateDrinks);
    Task<Response<bool>> DeleteDrinksAsync(int drinksId);
}
