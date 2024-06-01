using Domain.Constants;
using Domain.DTOs.DishDTOs;
using Domain.DTOs.OrderDTOs;
using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.OrderService;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PaymentController(IPaymentService service) : ControllerBase
{

    [HttpGet("payments")]
    [PermissionAuthorize(Permissions.Payments.View)]
    public async Task<Response<List<GetPaymentDto>>>GetPaymentsAsync (PaymentFilter filter)
    {
       return await service.GetPaymentsAsync(filter);
    }

    [HttpGet("{paymentId:int}")]
    [PermissionAuthorize(Permissions.Payments.View)]
    public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int id)
        => await service.GetPaymentByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Payments.Create)]
    public async Task<Response<string>> CreatePaymentAsync(CreatePaymentDto create)
        => await service.CreatePaymentAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Payments.Edit)]
    public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto update)
        => await service.UpdatePaymentAsync(update);

    [HttpDelete("{paymentId:int}")]
    [PermissionAuthorize(Permissions.Payments.Delete)]
    public async Task<Response<bool>> DeletePaymentAsync(int id)
        => await service.DeletePaymentAsync(id);
}