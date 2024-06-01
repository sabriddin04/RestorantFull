using Domain.DTOs.TableDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.TableService;

public interface ITableService
{
    Task<PagedResponse<List<GetTableDto>>> GetTablesAsync(TableFilter filter);
    Task<Response<GetTableDto>> GetTableByIdAsync(int tableId);
    Task<Response<string>> CreateTableAsync(CreateTableDto createTable);
    Task<Response<string>> UpdateTableAsync(UpdateTableDto updateTable);
    Task<Response<bool>> DeleteTableAsync(int tableId);
}
