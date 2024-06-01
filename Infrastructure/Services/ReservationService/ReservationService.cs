using System.Net;
using Domain.DTOs.ReservationDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ReservationService;


public class ReservationService(ILogger<ReservationService> logger, DataContext context) : IReservationService
{

    #region GetReservationsAsync

    public async Task<PagedResponse<List<GetReservation>>> GetReservationsAsync(ReservationFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetReservationsAsync} in time:{DateTime} ", "GetReservationsAsync",
                DateTimeOffset.UtcNow);

            var reservations = context.Reservations.AsQueryable();
            if (filter.UserId!=0)
                reservations = reservations.Where(x => x.UserId==filter.UserId);
            if (filter.TableId!=0)
                reservations = reservations.Where(x => x.TableId==filter.TableId);

            var response = await reservations.Select(x => new GetReservation()
            {
                UserId = x.UserId,
                Status = x.Status,
                Id = x.Id,
                Duration=x.Duration,
                TableId = x.TableId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await reservations.CountAsync();

            logger.LogInformation("Finished method {GetReservationsAsync} in time:{DateTime} ", "GetReservationsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetReservation>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetReservation>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetReservationByIdAsync

    public async Task<Response<GetReservation>> GetReservationByIdAsync(int reservationId)
    {
        try
        {
            logger.LogInformation("Starting method {GetReservationByIdAsync} in time:{DateTime} ", "GetReservationByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Reservations.Where(x => x.Id == reservationId).Select(x => new GetReservation()
            {
                UserId = x.UserId,
                Status = x.Status,
                Id = x.Id,
                Duration=x.Duration,
                TableId = x.TableId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();

            if (existing is null)
            {
                logger.LogWarning("Not found Reservation with id={Id},time={DateTimeNow}", reservationId, DateTime.UtcNow);
                return new Response<GetReservation>(HttpStatusCode.BadRequest, "Product not found");
            }

            logger.LogInformation("Finished method {GetReservationByIdAsync} in time:{DateTime} ", "GetReservationByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetReservation>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetReservation>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateReservationAsync

    public async Task<Response<string>> CreateReservationAsync(CreateReservationDto createReservation)
    {
        try
        {
            logger.LogInformation("Starting method {CreateReservationAsync} in time:{DateTime} ", "CreateReservationAsync",
                DateTimeOffset.UtcNow);
            var newReservation = new Reservation()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Duration = createReservation.Duration,
                UserId = createReservation.UserId,
                TableId = createReservation.TableId,
                Status=createReservation.Status,
            };
            await context.Reservations.AddAsync(newReservation);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateReservationAsync} in time:{DateTime} ", "CreateReservationAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Reservation by id:{newReservation.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateReservationAsync

    public async Task<Response<string>> UpdateReservationAsync(UpdateReservationDto updateReservation)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateReservationAsync} in time:{DateTime} ", "UpdateReservationAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Reservations.FirstOrDefaultAsync(x => x.Id == updateReservation.Id);
            if (existing is null)
            {
                logger.LogWarning("Reservation not found by id:{Id},time:{DateTimeNow} ", updateReservation.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Reservation not found");
            }

            existing.Duration = updateReservation.Duration;
            existing.Status = updateReservation.Status!;
            existing.UserId = existing.UserId;
            existing.TableId = existing.TableId;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateReservationAsync} in time:{DateTime} ", "UpdateReservationAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Reservation by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteReservationAsync

    public async Task<Response<bool>> DeleteReservationAsync(int reservationId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteReservationAsync} in time:{DateTime} ", "DeleteReservationAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Reservation not found by id:{reservationId}");

            context.Reservations.Remove(existing);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {DeleteReservationAsync} in time:{DateTime} ", "DeleteReservationAsync",
                DateTimeOffset.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion


}

