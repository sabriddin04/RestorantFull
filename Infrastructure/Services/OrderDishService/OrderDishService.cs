using System.Net;
using Domain.DTOs.OrderDishDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.OrderDishService;

public class OrderDishService(ILogger<OrderDishService>logger, DataContext context) : IOrderDishService
{

    #region GetOrderDishsAsync

    public async Task<PagedResponse<List<GetOrderDishDto>>> GetOrderDishsAsync(OrderDishFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrderDishsAsync} in time:{DateTime} ", "GetOrderDishsAsync",
                DateTimeOffset.UtcNow);

            var products = context.OrderDishes.AsQueryable();
            if (filter.DishId!=0)
                products = products.Where(x => x.DishId==filter.DishId);
            if (filter.OrderId!=0)
                products = products.Where(x => x.OrderId==filter.OrderId);
            if (filter.Quantity!=0)
                products = products.Where(x => x.Quantity==filter.Quantity);

            var response = await products.Select(x => new GetOrderDishDto()
            {
                DishId = x.DishId,
                OrderId = x.OrderId,
                Id = x.Id,
                Quantity=x.Quantity,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetOrderDishsAsync} in time:{DateTime} ", "GetOrderDishsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetOrderDishDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetOrderDishDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetOrderDishByIdAsync

    public async Task<Response<GetOrderDishDto>> GetOrderDishByIdAsync(int orderDishId)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrderDishByIdAsync} in time:{DateTime} ", "GetOrderDishByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.OrderDishes.Where(x => x.Id == orderDishId).Select(x => new GetOrderDishDto()
            {
                DishId = x.DishId,
                OrderId = x.OrderId,
                Quantity=x.Quantity,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found OrderDish with id={Id},time={DateTimeNow}", orderDishId, DateTime.UtcNow);
                return new Response<GetOrderDishDto>(HttpStatusCode.BadRequest, "OrderDish not found");
            }

            logger.LogInformation("Finished method {GetOrderDishByIdAsync} in time:{DateTime} ", "GetOrderDishByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetOrderDishDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetOrderDishDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateOrderDishAsync

    public async Task<Response<string>> CreateOrderDishAsync(CreateOrderDishDto createOrderDish)
    {
        try
        {
            logger.LogInformation("Starting method {CreateOrderDishAsync} in time:{DateTime} ", "CreateOrderDishAsync",
                DateTimeOffset.UtcNow);
            var newOrderDish = new OrderDish()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Quantity=createOrderDish.Quantity,
                DishId = createOrderDish.DishId,
                OrderId = createOrderDish.OrderId,
            };
            await context.OrderDishes.AddAsync(newOrderDish);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateOrderDishAsync} in time:{DateTime} ", "CreateOrderDishAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new OrderDish by id:{newOrderDish.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateOrderDishAsync

    public async Task<Response<string>> UpdateOrderDishAsync(UpdateOrderDishDto updateOrderDish)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateOrderDishAsync} in time:{DateTime} ", "UpdateOrderDishAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.OrderDishes.FirstOrDefaultAsync(x => x.Id == updateOrderDish.Id);
            if (existing is null)
            {
                logger.LogWarning("OrderDish not found by id:{Id},time:{DateTimeNow} ", updateOrderDish.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "OrderDish not found");
            }

            existing.DishId = updateOrderDish.DishId;
            existing.OrderId = updateOrderDish.OrderId;
            existing.Quantity = updateOrderDish.Quantity;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateOrderDishAsync} in time:{DateTime} ", "UpdateOrderDishAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated OrderDish by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteOrderDishAsync

    public async Task<Response<bool>> DeleteOrderDishAsync(int orderId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteOrderDishAsync} in time:{DateTime} ", "DeleteOrderDishAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.OrderDishes.FirstOrDefaultAsync(x => x.Id == orderId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"OrderDish not found by id:{orderId}");
            context.OrderDishes.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteOrderDishAsync} in time:{DateTime} ", "DeleteOrderDishAsync",
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

