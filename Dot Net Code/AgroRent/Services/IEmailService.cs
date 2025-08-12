namespace AgroRent.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string token);
        Task SendBookingConfirmationAsync(string email, string equipmentName, DateTime startDate, DateTime endDate);
        Task SendPaymentConfirmationAsync(string email, string paymentId, double amount);
    }
}
