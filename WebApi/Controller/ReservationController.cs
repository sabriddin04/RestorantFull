using Domain.Constants;
using Domain.DTOs.MenuDTOs;
using Domain.DTOs.ReservationDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.ReservationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController(IReservationService service) : ControllerBase
{

    [HttpGet("reservations")]
    [PermissionAuthorize(Permissions.Reservations.View)]
    public async Task<Response<List<GetReservation>>>GetReservationsAsync (ReservationFilter filter)
    {
       return await service.GetReservationsAsync(filter);
    }

    [HttpGet("{reservationId:int}")]
    [PermissionAuthorize(Permissions.Reservations.View)]
    public async Task<Response<GetReservation>> GetReservationByIdAsync(int id)
        => await service.GetReservationByIdAsync(id);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Reservations.Create)]
    public async Task<Response<string>> CreateReservationAsync(CreateReservationDto create)
        => await service.CreateReservationAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Reservations.Edit)]
    public async Task<Response<string>> UpdateReservationAsync(UpdateReservationDto update)
        => await service.UpdateReservationAsync(update);

    [HttpDelete("{reservationId:int}")]
    [PermissionAuthorize(Permissions.Reservations.Delete)]
    public async Task<Response<bool>> DeleteReservationAsync(int id)
        => await service.DeleteReservationAsync(id);
}