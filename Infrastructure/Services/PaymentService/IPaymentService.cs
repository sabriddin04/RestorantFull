using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PaymentService;

public interface IPaymentService
{
    Task<PagedResponse<List<GetPaymentDto>>> GetPaymentsAsync(PaymentFilter filter);
    Task<Response<GetPaymentDto>> GetPaymentByIdAsync(int paymentId);
    Task<Response<string>> CreatePaymentAsync(CreatePaymentDto createPayment);
    Task<Response<string>> UpdatePaymentAsync(UpdatePaymentDto updatePayment);
    Task<Response<bool>> DeletePaymentAsync(int paymentId);
}
