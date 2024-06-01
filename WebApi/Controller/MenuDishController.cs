using Domain.Constants;
using Domain.DTOs.MenuDishDTOs;
using Domain.DTOs.MenuDrinksDTOs;
using Domain.DTOs.MenuDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MenuDishService;
using Infrastructure.Services.MenuDrinksService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuDishController(IMenuDishService service) : ControllerBase
{

    [HttpGet("menuDishes")]
    [PermissionAuthorize(Permissions.MenuDishes.View)]
    public async Task<Response<List<GetMenuDishDto>>>GetMenuDishesAsync ([FromQuery]MenuDishFilter filter)
    {
       return await service.GetMenuDishsAsync(filter);
    }

    [HttpGet("{menuDishId:int}")]
    [PermissionAuthorize(Permissions.MenuDishes.View)]
    public async Task<Response<GetMenuDishDto>> GetMenuDishByIdAsync([FromQuery]int id)
        => await service.GetMenuDishByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.MenuDishes.Create)]
    public async Task<Response<string>> CreateMenuDishAsync([FromBody]CreateMenuDishDto create)
        => await service.CreateMenuDishAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.MenuDishes.Edit)]
    public async Task<Response<string>> UpdateMenuDishAsync([FromBody]UpdateMenuDishDto update)
        => await service.UpdateMenuDishAsync(update);

    [HttpDelete("{menuId:int}")]
    [PermissionAuthorize(Permissions.MenuDishes.Delete)]
    public async Task<Response<bool>> DeleteMenuDishAsync(int id)
        => await service.DeleteMenuDishAsync(id);
}