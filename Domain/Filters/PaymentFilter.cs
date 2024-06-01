using Domain.Enums;

namespace Domain.Filters;

public class PaymentFilter:PaginationFilter
{
   public decimal Sum { get; set; }
   public PaymentStatus Status { get; set; }
}
