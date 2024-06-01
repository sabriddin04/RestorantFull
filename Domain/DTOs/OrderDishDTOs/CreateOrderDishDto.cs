namespace Domain.DTOs.OrderDishDTOs;

public class CreateOrderDishDto
{
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }
    
}
