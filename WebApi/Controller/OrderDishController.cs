using Domain.Constants;
using Domain.DTOs.OrderDishDTOs;
using Domain.DTOs.OrderDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.OrderDishService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]

[Route("api/[controller]")]
public class OrderDishController(IOrderDishService service) : ControllerBase
{

    [HttpGet("orderDishes")]
    [PermissionAuthorize(Permissions.OrderDishes.View)]
    public async Task<Response<List<GetOrderDishDto>>>GetOrderDishsAsync ([FromQuery]OrderDishFilter filter)
    {
       return await service.GetOrderDishsAsync(filter);
    }

    [HttpGet("{orderDishId:int}")]
    [PermissionAuthorize(Permissions.OrderDishes.View)]
    public async Task<Response<GetOrderDishDto>> GetOrderDishByIdAsync(int id)
        => await service.GetOrderDishByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.OrderDishes.Create)]
    public async Task<Response<string>> CreateOrderDishAsync([FromBody]CreateOrderDishDto create)
        => await service.CreateOrderDishAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.OrderDishes.Edit)]
    public async Task<Response<string>> UpdateOrderAsync([FromBody]UpdateOrderDishDto update)
        => await service.UpdateOrderDishAsync(update);

    [HttpDelete("{orderDishId:int}")]
    [PermissionAuthorize(Permissions.OrderDishes.Delete)]
    public async Task<Response<bool>> DeleteOrderDishAsync(int id)
        => await service.DeleteOrderDishAsync(id);
}