namespace Domain.Filters;

public class ReservationFilter:PaginationFilter
{
    public int UserId { get; set; }
    public int TableId { get; set; }
    public ReservationFilter? Status { get; set; }


}
