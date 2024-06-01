using Domain.Enums;

namespace Domain.Filters;

public class TableFilter:PaginationFilter
{
    public int SizePlace { get; set; }
    public TableStatus Status { get; set; }
}
