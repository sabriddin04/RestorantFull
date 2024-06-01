using Domain.DTOs.MenuDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MenuService;

public interface IMenuService
{
    Task<PagedResponse<List<GetMenuDto>>> GetMenusAsync(MenuFilter filter);
    Task<Response<GetMenuDto>> GetMenuByIdAsync(int menuId);
    Task<Response<string>> CreateMenuAsync(CreateMenuDto createMenu);
    Task<Response<string>> UpdateMenuAsync(UpdateMenuDto updateMenu);
    Task<Response<bool>> DeleteMenuAsync(int menuId);
}
