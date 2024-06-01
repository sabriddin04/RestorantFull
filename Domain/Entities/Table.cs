using Domain.Enums;

namespace Domain.Entities;

public class Table : BaseEntity
{
    public string Name { get; set; } = null!;
    public int SizePlace { get; set; } = 0!;
    public TableStatus Status { get; set; }


    public List<Order>? Orders { get; set; }
    public List<Reservation>? Reservations { get; set; }
}
