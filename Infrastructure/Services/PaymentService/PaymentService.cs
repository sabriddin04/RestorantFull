using System.Net;
using Domain.DTOs.PaymentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.PaymentService;

public class PaymentService(ILogger<PaymentService>logger, DataContext context) : IPaymentService
{

    #region GetPaymentsAsync

    public async Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaymentFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetPaymentsAsync} in time:{DateTime} ", "GetPaymentsAsync",
                DateTimeOffset.UtcNow);

            var products = context.Payments.AsQueryable();
            if (filter.Status!=0)
                products = products.Where(x => x.Status==filter.Status);
            if (filter.Sum!=0)
                products = products.Where(x => x.Sum==filter.Sum);
                
            var response = await products.Select(x => new GetPaymentDto()
            {
                Status = x.Status,
                Sum = x.Sum,
                Id = x.Id,
                UserId=x.UserId,
                ReservationId=x.ReservationId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetPaymentsAsync} in time:{DateTime} ", "GetPaymentsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetPaymentDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetPaymentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetPaymentByIdAsync

    public async Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int PaymentId)
    {
        try
        {
            logger.LogInformation("Starting method {GetPaymentByIdAsync} in time:{DateTime} ", "GetPaymentByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Payments.Where(x => x.Id == PaymentId).Select(x => new GetPaymentDto()
            {
                Status = x.Status,
                Sum = x.Sum,
                Id = x.Id,
                UserId=x.UserId,
                ReservationId=x.ReservationId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Payment with id={Id},time={DateTimeNow}", PaymentId, DateTime.UtcNow);
                return new Response<GetPaymentDto>(HttpStatusCode.BadRequest, "Payment not found");
            }

            logger.LogInformation("Finished method {GetPaymentByIdAsync} in time:{DateTime} ", "GetPaymentByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetPaymentDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetPaymentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
    
    #region CreatePaymentAsync

    public async Task<Response<string>> CreatePaymentAsync(CreatePaymentDto createPayment)
    {
        try
        {
            logger.LogInformation("Starting method {CreatePaymentAsync} in time:{DateTime} ", "CreatePaymentAsync",
                DateTimeOffset.UtcNow);
            var newPayment = new Payment()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                UserId=createPayment.UserId,
                ReservationId = createPayment.ReservationId,
                Sum = createPayment.Sum,
                Status=createPayment.Status,
            };
            await context.Payments.AddAsync(newPayment);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreatePaymentAsync} in time:{DateTime} ", "CreatePaymentAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Payment by id:{newPayment.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
    
    #region UpdatePaymentAsync

    public async Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePayment)
    {
        try
        {
            logger.LogInformation("Starting method {UpdatePaymentAsync} in time:{DateTime} ", "UpdatePaymentAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == updatePayment.Id);
            if (existing is null)
            {
                logger.LogWarning("Payment not found by id:{Id},time:{DateTimeNow} ", updatePayment.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Payment not found");
            }

            existing.UserId = updatePayment.UserId;
            existing.ReservationId = updatePayment.ReservationId;
            existing.Sum = updatePayment.Sum;
            existing.Status = updatePayment.Status;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdatePaymentAsync} in time:{DateTime} ", "UpdatePaymentAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Payment by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
     
    #region DeletePaymentAsync

    public async Task<Response<bool>> DeletePaymentAsync(int paymentId)
    {
        try
        {
            logger.LogInformation("Starting method {DeletePaymentAsync} in time:{DateTime} ", "DeletePaymentAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Payments.FirstOrDefaultAsync(x => x.Id == paymentId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Payment not found by id:{paymentId}");
            context.Payments.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeletePaymentAsync} in time:{DateTime} ", "DeletePaymentAsync",
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