using AgroRent.DTOs;

namespace AgroRent.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentRespDto>> GetAllPaymentsAsync();
        Task<PaymentRespDto?> GetPaymentByIdAsync(int id);
        Task<PaymentRespDto?> GetPaymentByBookingIdAsync(int bookingId);
        Task<PaymentRespDto> CreatePaymentAsync(int bookingId, double amount);
        Task<PaymentRespDto> VerifyPaymentAsync(PaymentVerificationRequestDto dto);
        Task<bool> UpdatePaymentStatusAsync(int id, string status);
        Task<bool> DeletePaymentAsync(int id);
    }
}
