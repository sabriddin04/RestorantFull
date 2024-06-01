using Domain.Enums;

namespace Domain.DTOs.OrderDTOs;

public class UpdateOrderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int TableId { get; set; }
    public OrderStatus Status { get; set; }
}
