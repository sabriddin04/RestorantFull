using Domain.Constants;
using Domain.DTOs.MenuDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MenuService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController(IMenuService service) : ControllerBase
{

    [HttpGet("menus")]
    [PermissionAuthorize(Permissions.Menus.View)]
    public async Task<Response<List<GetMenuDto>>>GetMenusAsync ([FromQuery]MenuFilter filter)
    {
       return await service.GetMenusAsync(filter);
    }

    [HttpGet("{menuId:int}")]
    [PermissionAuthorize(Permissions.Menus.View)]
    public async Task<Response<GetMenuDto>> GetMenuByIdAsync(int id)
        => await service.GetMenuByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Menus.Create)]
    public async Task<Response<string>> CreateMenuAsync([FromBody]CreateMenuDto create)
        => await service.CreateMenuAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Menus.Edit)]
    public async Task<Response<string>> UpdateMenuAsync([FromBody]UpdateMenuDto update)
        => await service.UpdateMenuAsync(update);

    [HttpDelete("{menuId:int}")]
    [PermissionAuthorize(Permissions.Menus.Delete)]
    public async Task<Response<bool>> DeleteMenuAsync(int id)
        => await service.DeleteMenuAsync(id);
}