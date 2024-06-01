using Domain.Constants;
using Domain.DTOs.DishDTOs;
using Domain.DTOs.OrderDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService service) : ControllerBase
{

    [HttpGet("orders")]
    [PermissionAuthorize(Permissions.Orders.View)]
    public async Task<Response<List<GetOrderDto>>>GetOrdersAsync ([FromQuery]OrderFilter filter)
    {
       return await service.GetOrdersAsync(filter);
    }

    [HttpGet("{orderId:int}")]
    [PermissionAuthorize(Permissions.Orders.View)]
    public async Task<Response<GetOrderDto>> GetOrderByIdAsync(int id)
        => await service.GetOrderByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Orders.Create)]
    public async Task<Response<string>> CreateOrderAsync([FromBody]CreateOrderDto create)
        => await service.CreateOrderAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Orders.Edit)]
    public async Task<Response<string>> UpdateOrderAsync([FromBody]UpdateOrderDto update)
        => await service.UpdateOrderAsync(update);

    [HttpDelete("{orderId:int}")]
    [PermissionAuthorize(Permissions.Orders.Delete)]
    public async Task<Response<bool>> DeleteOrderAsync(int id)
        => await service.DeleteOrderAsync(id);
}