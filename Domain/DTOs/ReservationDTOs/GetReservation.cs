using Domain.Enums;

namespace Domain.DTOs.ReservationDTOs;

public class GetReservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TableId { get; set; }
    public TimeSpan Duration { get; set; }
    public ReservationStatus Status { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}
