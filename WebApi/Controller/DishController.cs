using Domain.Constants;
using Domain.DTOs.DishDTOs;
using Domain.DTOs.DrinksDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.DishService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController(IDishService service) : ControllerBase
{

    [HttpGet("dishes")]
    [PermissionAuthorize(Permissions.Dishes.View)]
    public async Task<Response<List<GetDishDto>>> GetTables([FromQuery]DishFilter filter)
    {
       return await service.GetDishesAsync(filter);
    }

    [HttpGet("{dishId:int}")]
    [PermissionAuthorize(Permissions.Dishes.View)]
    public async Task<Response<GetDishDto>> GetDishById(int id)
        => await service.GetDishByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Dishes.Create)]
    public async Task<Response<string>> CreateDish([FromForm]CreateDishDto create)
        => await service.CreateDishAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Dishes.Edit)]
    public async Task<Response<string>> UpdateDish([FromForm]UpdateDishDto update)
        => await service.UpdateDishAsync(update);

    [HttpDelete("{dishId:int}")]
    [PermissionAuthorize(Permissions.Dishes.Delete)]
    public async Task<Response<bool>> DeleteDish(int id)
        => await service.DeleteDishAsync(id);
}