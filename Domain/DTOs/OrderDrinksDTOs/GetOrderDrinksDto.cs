namespace Domain.DTOs.OrderDrinksDTOs;

public class GetOrderDrinksDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int DrinkId { get; set; }
    public int Quantity { get; set; } 
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
    
}
