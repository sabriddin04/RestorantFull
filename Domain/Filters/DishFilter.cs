namespace Domain.Filters;

public class DishFilter:PaginationFilter
{
    public string? Name { get; set; }
    public decimal Price { get; set; }

}
