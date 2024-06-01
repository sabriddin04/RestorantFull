using Domain.Enums;

namespace Domain.Filters;

public class OrderFilter:PaginationFilter
{
    public int TableId { get; set; }
    public string? Name { get; set; }
    public OrderStatus? Status { get; set; }

}
