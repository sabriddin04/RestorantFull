using System.Net;
using Domain.DTOs.OrderDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.OrderService;

public class OrderService(ILogger<OrderService> logger, DataContext context) : IOrderService
{

    #region GetOrdersAsync

    public async Task<PagedResponse<List<GetOrderDto>>> GetOrdersAsync(OrderFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrdersAsync} in time:{DateTime} ", "GetOrdersAsync",
                DateTimeOffset.UtcNow);

            var orders = context.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                orders = orders.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            if (filter.Status!=null)
                orders = orders.Where(x => x.Status==filter.Status);
            if (filter.TableId!=0)
                orders = orders.Where(x => x.TableId==filter.TableId);

            var response = await orders.Select(x => new GetOrderDto()
            {
                Name = x.Name,
                Status = x.Status,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                SumOfDish=x.Dishes!.Sum(k=>k.Price),
                SumOfDrinks=x.Drinks!.Sum(k=>k.Price),
                
                
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await orders.CountAsync();

            logger.LogInformation("Finished method {GetOrdersAsync} in time:{DateTime} ", "GetOrdersAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetOrderDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetOrderDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetOrderByIdAsync

    public async Task<Response<GetOrderDto>> GetOrderByIdAsync(int orderId)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrderByIdAsync} in time:{DateTime} ", "GetOrderByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Orders.Where(x => x.Id == orderId).Select(x => new GetOrderDto()
            {
                Name = x.Name,
                Status = x.Status,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                SumOfDish=x.Dishes!.Sum(k=>k.Price),
                SumOfDrinks=x.Drinks!.Sum(k=>k.Price),
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Order with id={Id},time={DateTimeNow}", orderId, DateTime.UtcNow);
                return new Response<GetOrderDto>(HttpStatusCode.BadRequest, "Order not found");
            }

            logger.LogInformation("Finished method {GetOrderByIdAsync} in time:{DateTime} ", "GetOrderByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetOrderDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetOrderDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateOrderAsync

    public async Task<Response<string>> CreateOrderAsync(CreateOrderDto createOrder)
    {
        try
        {
            logger.LogInformation("Starting method {CreateOrderAsync} in time:{DateTime} ", "CreateOrderAsync",
                DateTimeOffset.UtcNow);
            var newOrder = new Domain.Entities.Order()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                TableId = createOrder.TableId,
                Name = createOrder.Name,
                Status = createOrder.Status,
            };
            await context.Orders.AddAsync(newOrder);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateOrderAsync} in time:{DateTime} ", "CreateOrderAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Order by id:{newOrder.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateOrderAsync

    public async Task<Response<string>> UpdateOrderAsync(UpdateOrderDto updateOrder)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateOrderAsync} in time:{DateTime} ", "UpdateOrderAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Orders.FirstOrDefaultAsync(x => x.Id == updateOrder.Id);
            if (existing is null)
            {
                logger.LogWarning("Order not found by id:{Id},time:{DateTimeNow} ", updateOrder.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Order not found");
            }

            existing.TableId = updateOrder.TableId;
            existing.Name = updateOrder.Name!;
            existing.Status = existing.Status;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateOrderAsync} in time:{DateTime} ", "UpdateOrderAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Order by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteOrderAsync

    public async Task<Response<bool>> DeleteOrderAsync(int orderId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteOrderAsync} in time:{DateTime} ", "DeleteOrderAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Order not found by id:{orderId}");
            context.Orders.Remove(existing);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {DeleteOrderAsync} in time:{DateTime} ", "DeleteOrderAsync",
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
