using System.Net;
using Domain.DTOs.MenuDishDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MenuDishService;

public class MenuDishService(ILogger<MenuDishService>logger, DataContext context) : IMenuDishService
{

    #region GetMenuDishsAsync

    public async Task<PagedResponse<List<GetMenuDishDto>>> GetMenuDishsAsync(MenuDishFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenuDishsAsync} in time:{DateTime} ", "GetMenuDishsAsync",
                DateTimeOffset.UtcNow);

            var products = context.MenuDishes.AsQueryable();
            if (filter.DishId!=0)
                products = products.Where(x => x.DishId==filter.DishId);
            if (filter.MenuId!=0)
                products = products.Where(x => x.MenuId==filter.MenuId);

            var response = await products.Select(x => new GetMenuDishDto()
            {
                DishId = x.DishId,
                MenuId = x.MenuId,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetMenuDishsAsync} in time:{DateTime} ", "GetMenuDishsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetMenuDishDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMenuDishDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetMenuDishByIdAsync

    public async Task<Response<GetMenuDishDto>> GetMenuDishByIdAsync(int menuDishId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenuDishByIdAsync} in time:{DateTime} ", "GetMenuDishByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.MenuDishes.Where(x => x.Id == menuDishId).Select(x => new GetMenuDishDto()
            {
                DishId = x.DishId,
                MenuId = x.MenuId,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found MenuDish with id={Id},time={DateTimeNow}", menuDishId, DateTime.UtcNow);
                return new Response<GetMenuDishDto>(HttpStatusCode.BadRequest, "MenuDish not found");
            }

            logger.LogInformation("Finished method {GetMenuDishByIdAsync} in time:{DateTime} ", "GetMenuDishByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetMenuDishDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetMenuDishDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateMenuDishAsync

    public async Task<Response<string>> CreateMenuDishAsync(CreateMenuDishDto createMenuDish)
    {
        try
        {
            logger.LogInformation("Starting method {CreateMenuDishAsync} in time:{DateTime} ", "CreateMenuDishAsync",
                DateTimeOffset.UtcNow);
            var newMenuDish = new MenuDish()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                DishId = createMenuDish.DishId,
                MenuId = createMenuDish.MenuId,
            };
            await context.MenuDishes.AddAsync(newMenuDish);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateMenuDishAsync} in time:{DateTime} ", "CreateMenuDishAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new MenuDish by id:{newMenuDish.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateMenuDishAsync

    public async Task<Response<string>> UpdateMenuDishAsync(UpdateMenuDishDto updateMenuDish)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateMenuDishAsync} in time:{DateTime} ", "UpdateMenuDishAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.MenuDishes.FirstOrDefaultAsync(x => x.Id == updateMenuDish.Id);
            if (existing is null)
            {
                logger.LogWarning("MenuDish not found by id:{Id},time:{DateTimeNow} ", updateMenuDish.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "MenuDish not found");
            }


            existing.DishId = updateMenuDish.DishId;
            existing.MenuId = updateMenuDish.MenuId;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateMenuDishAsync} in time:{DateTime} ", "UpdateMenuDishAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated MenuDish by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteMenuDishAsync

    public async Task<Response<bool>> DeleteMenuDishAsync(int menuId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteMenuDishAsync} in time:{DateTime} ", "DeleteMenuDishAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.MenuDishes.FirstOrDefaultAsync(x => x.Id == menuId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"MenuDish not found by id:{menuId}");
            context.MenuDishes.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteMenuDishAsync} in time:{DateTime} ", "DeleteMenuDishAsync",
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
