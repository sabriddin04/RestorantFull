using System.Net;
using Domain.DTOs.MenuDrinksDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MenuDrinksService;

public class MenuDrinksService(ILogger<MenuDrinksService>logger, DataContext context) : IMenuDrinksService
{

    #region GetMenuDrinksAsync

    public async Task<PagedResponse<List<GetMenuDrinksDto>>> GetMenuDrinksesAsync(MenuDrinksFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenuDrinksesAsync} in time:{DateTime} ", "GetMenuDrinksesAsync",
                DateTimeOffset.UtcNow);

            var products = context.MenuDrinks.AsQueryable();
            if (filter.DrinksId!=0)
                products = products.Where(x => x.DrinkId==filter.DrinksId);
            if (filter.MenuId!=0)
                products = products.Where(x => x.MenuId==filter.MenuId);

            var response = await products.Select(x => new GetMenuDrinksDto()
            {
                DrinkId = x.DrinkId,
                MenuId = x.MenuId,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetMenuDrinksesAsync} in time:{DateTime} ", "GetMenuDrinksesAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetMenuDrinksDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMenuDrinksDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetMenuDrinksByIdAsync

    public async Task<Response<GetMenuDrinksDto>> GetMenuDrinksByIdAsync(int menuDrinksId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenuDrinksByIdAsync} in time:{DateTime} ", "GetMenuDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.MenuDrinks.Where(x => x.Id == menuDrinksId).Select(x => new GetMenuDrinksDto()
            {
                DrinkId = x.DrinkId,
                MenuId = x.MenuId,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found MenuDrinks with id={Id},time={DateTimeNow}", menuDrinksId, DateTime.UtcNow);
                return new Response<GetMenuDrinksDto>(HttpStatusCode.BadRequest, "MenuDrinks not found");
            }

            logger.LogInformation("Finished method {GetMenuDrinksByIdAsync} in time:{DateTime} ", "GetMenuDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetMenuDrinksDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetMenuDrinksDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateMenuDrinksAsync

    public async Task<Response<string>> CreateMenuDrinksAsync(CreateMenuDrinksDto createMenuDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {CreateMenuDrinksAsync} in time:{DateTime} ", "CreateMenuDrinksAsync",
                DateTimeOffset.UtcNow);
            var newMenuDrinks = new MenuDrinks()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                DrinkId = createMenuDrinks.DrinkId,
                MenuId = createMenuDrinks.MenuId,
            };
            await context.MenuDrinks.AddAsync(newMenuDrinks);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateMenuDrinksAsync} in time:{DateTime} ", "CreateMenuDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new MenuDrinks by id:{newMenuDrinks.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateMenuDrinksAsync

    public async Task<Response<string>> UpdateMenuDrinksAsync(UpdateMenuDrinksDto updateMenuDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateMenuDrinksAsync} in time:{DateTime} ", "UpdateMenuDrinksAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.MenuDrinks.FirstOrDefaultAsync(x => x.Id == updateMenuDrinks.Id);
            if (existing is null)
            {
                logger.LogWarning("MenuDrinks not found by id:{Id},time:{DateTimeNow} ", updateMenuDrinks.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "MenuDrinks not found");
            }

            existing.DrinkId = updateMenuDrinks.DrinkId;
            existing.MenuId = updateMenuDrinks.MenuId;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateMenuDrinksAsync} in time:{DateTime} ", "UpdateMenuDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated MenuDrinks by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteMenuDrinksAsync

    public async Task<Response<bool>> DeleteMenuDrinksAsync(int menuDrinksId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteMenuDrinksAsync} in time:{DateTime} ", "DeleteMenuDrinksAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.MenuDrinks.FirstOrDefaultAsync(x => x.Id == menuDrinksId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"MenuDrinks not found by id:{menuDrinksId}");
            context.MenuDrinks.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteMenuDrinksAsync} in time:{DateTime} ", "DeleteMenuDrinksAsync",
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

