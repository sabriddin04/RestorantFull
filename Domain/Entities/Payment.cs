using Domain.Enums;

namespace Domain.Entities;

public class Payment:BaseEntity
{
    public int UserId { get; set; }
    public int ReservationId { get; set; }
    public decimal Sum { get; set; }  
    public PaymentStatus Status { get; set; }



    public User? User { get; set; }
    public Reservation? Reservation { get; set; }

}
