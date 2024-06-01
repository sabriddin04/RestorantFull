namespace Domain.DTOs.OrderDrinksDTOs;

public class UpdateOrderDrinksDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DrinkId { get; set; }
    public int Quantity { get; set; }
    
}
