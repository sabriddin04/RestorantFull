namespace Domain.Filters;

public class MenuDishFilter:PaginationFilter
{
    public int MenuId { get; set; }
    public int DishId { get; set; }
    
}
