namespace Domain.Filters;

public class DrinksFilter:PaginationFilter
{
    public string? Name { get; set; }
    public double Capacity { get; set; }
    public decimal Price { get; set; }
    
}
