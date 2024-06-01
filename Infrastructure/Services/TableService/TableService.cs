using System.Net;
using Domain.DTOs.TableDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.TableService;

public class TableService(ILogger<TableService> logger, DataContext context) : ITableService
{

    #region GetTablesAsync

    public async Task<PagedResponse<List<GetTableDto>>> GetTablesAsync(TableFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetTablesAsync} in time:{DateTime} ", "GetTablesAsync",
                DateTimeOffset.UtcNow);

            var tables = context.Tables.AsQueryable();
            if (filter.SizePlace != 0)
                tables = tables.Where(x => x.SizePlace == filter.SizePlace);

            var response = await tables.Select(x => new GetTableDto()
            {
                Name = x.Name,
                SizePlace = x.SizePlace,
                Id = x.Id,
                Status = x.Status,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await tables.CountAsync();

            logger.LogInformation("Finished method {GetTablesAsync} in time:{DateTime} ", "GetTablesAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetTableDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetTableDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetTableByIdAsync

    public async Task<Response<GetTableDto>> GetTableByIdAsync(int tableId)
    {
        try
        {
            logger.LogInformation("Starting method {GetTableByIdAsync} in time:{DateTime} ", "GetTableByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Tables.Where(x => x.Id == tableId).Select(x => new GetTableDto()
            {
                Name = x.Name,
                SizePlace = x.SizePlace,
                Id = x.Id,
                Status = x.Status,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Table with id={Id},time={DateTimeNow}", tableId, DateTime.UtcNow);
                return new Response<GetTableDto>(HttpStatusCode.BadRequest, "Table not found");
            }

            logger.LogInformation("Finished method {GetTableByIdAsync} in time:{DateTime} ", "GetTableByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetTableDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetTableDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateTableAsync

    public async Task<Response<string>> CreateTableAsync(CreateTableDto createTable)
    {
        try
        {
            logger.LogInformation("Starting method {CreateTableAsync} in time:{DateTime} ", "CreateTableAsync",
                DateTimeOffset.UtcNow);
            var newTable = new Table()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Status = createTable.Status,
                Name = createTable.Name,
                SizePlace = createTable.SizePlace,
            };
            await context.Tables.AddAsync(newTable);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateTableAsync} in time:{DateTime} ", "CreateTableAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Table by id:{newTable.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateTableAsync

    public async Task<Response<string>> UpdateTableAsync(UpdateTableDto updateTable)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateTableAsync} in time:{DateTime} ", "UpdateTableAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Tables.FirstOrDefaultAsync(x => x.Id == updateTable.Id);
            if (existing is null)
            {
                logger.LogWarning("Table not found by id:{Id},time:{DateTimeNow} ", updateTable.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Table not found");
            }
            existing.SizePlace = updateTable.SizePlace;
            existing.Name = updateTable.Name!;
            existing.Status = existing.Status;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateTableAsync} in time:{DateTime} ", "UpdateTableAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Table by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteTableAsync

    public async Task<Response<bool>> DeleteTableAsync(int tableId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteTableAsync} in time:{DateTime} ", "DeleteTableAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Tables.FirstOrDefaultAsync(x => x.Id == tableId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Table not found by id:{tableId}");
            context.Tables.Remove(existing);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {DeleteTableAsync} in time:{DateTime} ", "DeleteTableAsync",
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
