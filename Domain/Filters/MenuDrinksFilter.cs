namespace Domain.Filters;

public class MenuDrinksFilter:PaginationFilter
{
    public int MenuId { get; set; }
    public int DrinksId { get; set; }
}
