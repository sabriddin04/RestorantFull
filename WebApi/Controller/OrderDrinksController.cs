using Domain.Constants;
using Domain.DTOs.OrderDishDTOs;
using Domain.DTOs.OrderDrinksDTOs;
using Domain.DTOs.OrderDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.OrderDishService;
using Infrastructure.Services.OrderDrinksService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDrinksController(IOrderDrinksService service) : ControllerBase
{

    [HttpGet("orderDrinkses")]
    [PermissionAuthorize(Permissions.OrderDrinks.View)]
    public async Task<Response<List<GetOrderDrinksDto>>>GetOrderDrinkssAsync ([FromQuery]OrderDrinksFilter filter)
    {
       return await service.GetOrderDrinksesAsync(filter);
    }
    [HttpGet("{orderDrinksId:int}")]
    [PermissionAuthorize(Permissions.OrderDrinks.View)]
    public async Task<Response<GetOrderDrinksDto>> GetOrderDrinksByIdAsync(int id)
        => await service.GetOrderDrinksByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.OrderDrinks.Create)]
    public async Task<Response<string>> CreateOrderDrinksAsync([FromBody]CreateOrderDrinksDto create)
        => await service.CreateOrderDrinksAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.OrderDrinks.Edit)]
    public async Task<Response<string>> UpdateOrderAsync([FromBody]UpdateOrderDrinksDto update)
        => await service.UpdateOrderDrinksAsync(update);

    [HttpDelete("{orderDrinksId:int}")]
    [PermissionAuthorize(Permissions.OrderDrinks.Delete)]
    public async Task<Response<bool>> DeleteOrderDrinksAsync(int id)
        => await service.DeleteOrderDrinksAsync(id);
}