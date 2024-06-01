using Domain.Enums;

namespace Domain.DTOs.ReservationDTOs;

public class CreateReservationDto
{
    public int UserId { get; set; }
    public int TableId { get; set; }
    public TimeSpan Duration { get; set; }
    public ReservationStatus Status { get; set; }
}
