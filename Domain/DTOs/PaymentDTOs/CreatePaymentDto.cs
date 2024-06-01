using Domain.Enums;

namespace Domain.DTOs.PaymentDTOs;

public class CreatePaymentDto
{
    public int UserId { get; set; }
    public int ReservationId { get; set; }
    public decimal Sum { get; set; }  
    public PaymentStatus Status { get; set; }
}
