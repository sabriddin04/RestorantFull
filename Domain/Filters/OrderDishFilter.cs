namespace Domain.Filters;

public class OrderDishFilter:PaginationFilter
{
      public int OrderId { get; set; }
      public int DishId { get; set; }
      public int Quantity { get; set; }
    
}
