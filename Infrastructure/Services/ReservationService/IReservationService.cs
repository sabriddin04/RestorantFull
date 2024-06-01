using Domain.DTOs.ReservationDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.ReservationService;

public interface IReservationService
{
    Task<PagedResponse<List<GetReservation>>> GetReservationsAsync(ReservationFilter filter);
    Task<Response<GetReservation>> GetReservationByIdAsync(int ReservationId);
    Task<Response<string>> CreateReservationAsync(CreateReservationDto createReservation);
    Task<Response<string>> UpdateReservationAsync(UpdateReservationDto updateReservation);
    Task<Response<bool>> DeleteReservationAsync(int ReservationId);
}
