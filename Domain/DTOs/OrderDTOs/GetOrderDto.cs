using Domain.Enums;

namespace Domain.DTOs.OrderDTOs;

public class GetOrderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int TableId { get; set; }  
    public decimal SumOfDish { get; set; }
    public decimal SumOfDrinks { get; set; }

    // public decimal Sum { get; set; }

    public OrderStatus Status { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}
