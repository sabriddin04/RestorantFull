namespace Domain.Filters;

public class OrderDrinksFilter : PaginationFilter
{
    public int OrderId { get; set; }
    public int DrinkId { get; set; }
    public int Quantity { get; set; }

}
