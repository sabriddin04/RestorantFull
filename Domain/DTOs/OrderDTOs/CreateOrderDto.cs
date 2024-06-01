using Domain.Enums;

namespace Domain.DTOs.OrderDTOs;

public class CreateOrderDto
{
    public string Name { get; set; }=null!;
    public int TableId { get; set; } 
    public OrderStatus Status { get; set; } 
}
