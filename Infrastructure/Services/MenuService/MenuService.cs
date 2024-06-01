
using System.Net;
using Domain.DTOs.MenuDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MenuService;

public class MenuService (ILogger<MenuService> logger, DataContext context) : IMenuService
{

    #region GetMenusAsync

    public async Task<PagedResponse<List<GetMenuDto>>> GetMenusAsync(MenuFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenusAsync} in time:{DateTime} ", "GetMenusAsync",
                DateTimeOffset.UtcNow);

            var products = context.Menus.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                products = products.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            var response = await products.Select(x => new GetMenuDto()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetProductsAsync} in time:{DateTime} ", "GetProductsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetMenuDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMenuDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetMenuByIdAsync

    public async Task<Response<GetMenuDto>> GetMenuByIdAsync(int menuId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMenuByIdAsync} in time:{DateTime} ", "GetMenuByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Menus.Where(x => x.Id == menuId).Select(x => new GetMenuDto()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Menu with id={Id},time={DateTimeNow}", menuId, DateTime.UtcNow);
                return new Response<GetMenuDto>(HttpStatusCode.BadRequest, "Menu not found");
            }

            logger.LogInformation("Finished method {GetMenuByIdAsync} in time:{DateTime} ", "GetMenuByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetMenuDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetMenuDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateMenuAsync

    public async Task<Response<string>> CreateMenuAsync(CreateMenuDto createMenu)
    {
        try
        {
            logger.LogInformation("Starting method {CreateMenuAsync} in time:{DateTime} ", "CreateMenuAsync",
                DateTimeOffset.UtcNow);
            var newMenu = new Menu()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Description = createMenu.Description,
                Name = createMenu.Name
            };
            await context.Menus.AddAsync(newMenu);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateMenuAsync} in time:{DateTime} ", "CreateMenuAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Menu by id:{newMenu.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateMenuAsync

    public async Task<Response<string>> UpdateMenuAsync(UpdateMenuDto updateMenu)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateMenuAsync} in time:{DateTime} ", "UpdateMenuAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Menus.FirstOrDefaultAsync(x => x.Id == updateMenu.Id);
            if (existing is null)
            {
                logger.LogWarning("Menu not found by id:{Id},time:{DateTimeNow} ", updateMenu.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Menu not found");
            }
            existing.Description = updateMenu.Description;
            existing.Name = updateMenu.Name!;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateMenuAsync} in time:{DateTime} ", "UpdateMenuAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Menu by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteMenuAsync

    public async Task<Response<bool>> DeleteMenuAsync(int menuId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteMenuAsync} in time:{DateTime} ", "DeleteMenuAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Menus.FirstOrDefaultAsync(x => x.Id == menuId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Menu not found by id:{menuId}");
            context.Menus.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteMenuAsync} in time:{DateTime} ", "DeleteMenuAsync",
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