using System.Net;
using Domain.DTOs.OrderDrinksDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.OrderDrinksService;

public class OrderDrinksService(ILogger<OrderDrinksService>logger, DataContext context) : IOrderDrinksService
{

      #region GetOrderDrinksesAsync

    public async Task<PagedResponse<List<GetOrderDrinksDto>>> GetOrderDrinksesAsync(OrderDrinksFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrderDrinksesAsync} in time:{DateTime} ", "GetOrderDrinksesAsync",
                DateTimeOffset.UtcNow);

            var products = context.OrderDrinks.AsQueryable();
            if (filter.DrinkId!=0)
                products = products.Where(x => x.DrinkId==filter.DrinkId);
            if (filter.OrderId!=0)
                products = products.Where(x => x.OrderId==filter.OrderId);
            if (filter.Quantity!=0)
                products = products.Where(x => x.Quantity==filter.Quantity);

            var response = await products.Select(x => new GetOrderDrinksDto()
            {
                DrinkId = x.DrinkId,
                OrderId = x.OrderId,
                Id = x.Id,
                Quantity=x.Quantity,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetOrderDrinksesAsync} in time:{DateTime} ", "GetOrderDrinksesAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetOrderDrinksDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetOrderDrinksDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

      #region GetOrderDrinksByIdAsync

    public async Task<Response<GetOrderDrinksDto>> GetOrderDrinksByIdAsync(int orderDrinksId)
    {
        try
        {
            logger.LogInformation("Starting method {GetOrderDrinksByIdAsync} in time:{DateTime} ", "GetOrderDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.OrderDrinks.Where(x => x.Id == orderDrinksId).Select(x => new GetOrderDrinksDto()
            {
                DrinkId = x.DrinkId,
                OrderId = x.OrderId,
                Quantity=x.Quantity,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found OrderDrinks with id={Id},time={DateTimeNow}", orderDrinksId, DateTime.UtcNow);
                return new Response<GetOrderDrinksDto>(HttpStatusCode.BadRequest, "OrderDrinks not found");
            }

            logger.LogInformation("Finished method {GetOrderDrinksByIdAsync} in time:{DateTime} ", "GetOrderDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetOrderDrinksDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetOrderDrinksDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

      #region CreateOrderDrinksAsync

    public async Task<Response<string>> CreateOrderDrinksAsync(CreateOrderDrinksDto createOrderDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {CreateOrderDrinksAsync} in time:{DateTime} ", "CreateOrderDrinksAsync",
                DateTimeOffset.UtcNow);
            var newOrderDrinks = new OrderDrink()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Quantity=createOrderDrinks.Quantity,
                DrinkId = createOrderDrinks.DrinkId,
                OrderId = createOrderDrinks.OrderId,
            };
            await context.OrderDrinks.AddAsync(newOrderDrinks);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateOrderDrinksAsync} in time:{DateTime} ", "CreateOrderDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new OrderDrinks by id:{newOrderDrinks.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
      
      #region UpdateOrderDrinksAsync

    public async Task<Response<string>> UpdateOrderDrinksAsync(UpdateOrderDrinksDto updateOrderDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateOrderDrinksAsync} in time:{DateTime} ", "UpdateOrderDrinksAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.OrderDrinks.FirstOrDefaultAsync(x => x.Id == updateOrderDrinks.Id);
            if (existing is null)
            {
                logger.LogWarning("OrderDrinks not found by id:{Id},time:{DateTimeNow} ", updateOrderDrinks.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "OrderDrinks not found");
            }

            existing.DrinkId = updateOrderDrinks.DrinkId;
            existing.OrderId = updateOrderDrinks.OrderId;
            existing.Quantity = updateOrderDrinks.Quantity;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateOrderDrinksAsync} in time:{DateTime} ", "UpdateOrderDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated OrderDrinks by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

      #region DeleteOrderDrinksAsync

    public async Task<Response<bool>> DeleteOrderDrinksAsync(int orderId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteOrderDrinksAsync} in time:{DateTime} ", "DeleteOrderDrinksAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.OrderDrinks.FirstOrDefaultAsync(x => x.Id == orderId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"OrderDrinks not found by id:{orderId}");
            context.OrderDrinks.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteOrderDrinksAsync} in time:{DateTime} ", "DeleteOrderDrinksAsync",
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