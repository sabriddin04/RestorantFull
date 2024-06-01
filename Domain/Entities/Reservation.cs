using Domain.Enums;

namespace Domain.Entities;

public class Reservation:BaseEntity
{
    public int UserId { get; set; }
    public int TableId { get; set; }
    public TimeSpan Duration { get; set; }
    public ReservationStatus Status { get; set; }

    

    


    public User? User { get; set; }
    public Table? Table { get; set; }
    public List<Payment>? Payments { get; set; }

}
