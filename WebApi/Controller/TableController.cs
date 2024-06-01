using Domain.Constants;
using Domain.DTOs.TableDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.TableService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TableController(ITableService tableService) : ControllerBase
{

    [HttpGet("tables")]
    [PermissionAuthorize(Permissions.Tables.View)]
    public async Task<Response<List<GetTableDto>>> GetTables([FromQuery]TableFilter tableFilter)
    {
       return await tableService.GetTablesAsync(tableFilter);
    }

    [HttpGet("{tableId:int}")]
    [PermissionAuthorize(Permissions.Tables.View)]
    public async Task<Response<GetTableDto>> GetTableById(int tableId)
        => await tableService.GetTableByIdAsync(tableId);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Tables.Create)]
    public async Task<Response<string>> CreateTable([FromBody]CreateTableDto create)
        => await tableService.CreateTableAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Tables.Edit)]
    public async Task<Response<string>> UpdateTable([FromBody]UpdateTableDto updateTableDto)
        => await tableService.UpdateTableAsync(updateTableDto);

    [HttpDelete("{tableId:int}")]
    [PermissionAuthorize(Permissions.Tables.Delete)]
    public async Task<Response<bool>> DeleteTable(int tableId)
        => await tableService.DeleteTableAsync(tableId);
}