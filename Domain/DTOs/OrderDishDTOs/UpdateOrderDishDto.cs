namespace Domain.DTOs.OrderDishDTOs;

public class UpdateOrderDishDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DishId { get; set; }
    public int Quantity { get; set; }
}
