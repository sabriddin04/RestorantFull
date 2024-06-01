using Domain.Constants;
using Domain.DTOs.MenuDrinksDTOs;
using Domain.DTOs.MenuDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MenuDrinksService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuDrinksController(IMenuDrinksService service) : ControllerBase
{

    [HttpGet("menuDrinkses")]
    [PermissionAuthorize(Permissions.MenuDrinks.View)]
    public async Task<Response<List<GetMenuDrinksDto>>>GetMenuDrinksesAsync ([FromQuery]MenuDrinksFilter filter)
    {
       return await service.GetMenuDrinksesAsync(filter);
    }

    [HttpGet("{menudrinksId:int}")]
    [PermissionAuthorize(Permissions.MenuDrinks.View)]
    public async Task<Response<GetMenuDrinksDto>> GetMenuDrinksByIdAsync([FromQuery]int id)
        => await service.GetMenuDrinksByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.MenuDrinks.Create)]
    public async Task<Response<string>> CreateMenuDrinksAsync([FromBody]CreateMenuDrinksDto create)
        => await service.CreateMenuDrinksAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.MenuDrinks.Edit)]
    public async Task<Response<string>> UpdateMenuDrinksAsync([FromBody]UpdateMenuDrinksDto update)
        => await service.UpdateMenuDrinksAsync(update);

    [HttpDelete("{menuId:int}")]
    [PermissionAuthorize(Permissions.MenuDrinks.Delete)]
    public async Task<Response<bool>> DeleteMenuDrinksAsync(int id)
        => await service.DeleteMenuDrinksAsync(id);
}