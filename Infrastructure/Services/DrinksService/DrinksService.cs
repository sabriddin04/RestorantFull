using System.Net;
using Domain.DTOs.DrinksDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.DrinksService;

public class DrinksService(IFileService fileService, ILogger<DrinksService> logger, DataContext context) : IDrinksService
{

    #region GetDrinksesAsync

    public async Task<PagedResponse<List<GetDrinksDto>>> GetDrinksesAsync(DrinksFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetDrinksesAsync} in time:{DateTime} ", "GetDrinksesAsync",
                DateTimeOffset.UtcNow);

            var drinkses = context.Drinkses.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                drinkses = drinkses.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            if (filter.Capacity!=0)
                drinkses = drinkses.Where(x => x.Capacity==filter.Capacity);
            if (filter.Price!=0)
                drinkses = drinkses.Where(x => x.Price==filter.Price);

            var response = await drinkses.Select(x => new GetDrinksDto()
            {
                Name = x.Name,
                Capacity = x.Capacity,
                Id = x.Id,
                Photo = x.Photo,
                Price = x.Price,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await drinkses.CountAsync();

            logger.LogInformation("Finished method {GetDrinksesAsync} in time:{DateTime} ", "GetDrinksesAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetDrinksDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetDrinksDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetDrinksByIdAsync

    public async Task<Response<GetDrinksDto>> GetDrinksByIdAsync(int drinksId)
    {
        try
        {
            logger.LogInformation("Starting method {GetDrinksByIdAsync} in time:{DateTime} ", "GetDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Drinkses.Where(x => x.Id == drinksId).Select(x => new GetDrinksDto()
            {
                Name = x.Name,
                Capacity = x.Capacity,
                Id = x.Id,
                Photo = x.Photo,
                Price = x.Price,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Drinks with id={Id},time={DateTimeNow}", drinksId, DateTime.UtcNow);
                return new Response<GetDrinksDto>(HttpStatusCode.BadRequest, "Drinks not found");
            }

            logger.LogInformation("Finished method {GetDrinksByIdAsync} in time:{DateTime} ", "GetDrinksByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetDrinksDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetDrinksDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateDrinksAsync

    public async Task<Response<string>> CreateDrinksAsync(CreateDrinksDto createDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {CreateDrinksAsync} in time:{DateTime} ", "CreateDrinksAsync",
                DateTimeOffset.UtcNow);
            var newDrinks = new Drinks()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Capacity = createDrinks.Capacity,
                Name = createDrinks.Name,
                Price = createDrinks.Price,
                Photo = createDrinks.Photo == null ? "null" : await fileService.CreateFile(createDrinks.Photo)
            };
            await context.Drinkses.AddAsync(newDrinks);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateDrinksAsync} in time:{DateTime} ", "CreateDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Drinks by id:{newDrinks.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateDrinksAsync

    public async Task<Response<string>> UpdateDrinksAsync(UpdateDrinksDto updateDrinks)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateDrinksAsync} in time:{DateTime} ", "UpdateDrinksAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Drinkses.FirstOrDefaultAsync(x => x.Id == updateDrinks.Id);
            if (existing is null)
            {
                logger.LogWarning("Drinks not found by id:{Id},time:{DateTimeNow} ", updateDrinks.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Drinks not found");
            }

            if (updateDrinks.Photo != null)
            {
                if (existing.Photo != null) fileService.DeleteFile(existing.Photo);
                existing.Photo = await fileService.CreateFile(updateDrinks.Photo);
            }

            existing.Capacity = updateDrinks.Capacity;
            existing.Name = updateDrinks.Name!;
            existing.Price = updateDrinks.Price;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateDrinksAsync} in time:{DateTime} ", "UpdateDrinksAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Drinks by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteDrinksAsync


    public async Task<Response<bool>> DeleteDrinksAsync(int drinksId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteDrinksAsync} in time:{DateTime} ", "DeleteDrinksAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Drinkses.FirstOrDefaultAsync(x => x.Id == drinksId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Drinks not found by id:{drinksId}");
            if (existing.Photo != null)
                fileService.DeleteFile(existing.Photo);
            context.Drinkses.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteDrinksAsync} in time:{DateTime} ", "DeleteDrinksAsync",
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
