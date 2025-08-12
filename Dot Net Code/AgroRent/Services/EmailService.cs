using AgroRent.Security;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace AgroRent.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var subject = "Email Verification - AgroRent";
            var body = $@"
                <h2>Welcome to AgroRent!</h2>
                <p>Please click the following link to verify your email address:</p>
                <p><a href='http://localhost:5000/auth/verify?token={token}'>Verify Email</a></p>
                <p>This link will expire in 24 hours.</p>
                <p>If you didn't create an account, please ignore this email.</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendBookingConfirmationAsync(string email, string equipmentName, DateTime startDate, DateTime endDate)
        {
            var subject = "Booking Confirmation - AgroRent";
            var body = $@"
                <h2>Booking Confirmed!</h2>
                <p>Your booking for {equipmentName} has been confirmed.</p>
                <p><strong>Start Date:</strong> {startDate:MM/dd/yyyy}</p>
                <p><strong>End Date:</strong> {endDate:MM/dd/yyyy}</p>
                <p>Thank you for using AgroRent!</p>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPaymentConfirmationAsync(string email, string paymentId, double amount)
        {
            var subject = "Payment Confirmation - AgroRent";
            var body = $@"
                <h2>Payment Successful!</h2>
                <p>Your payment of ${amount:F2} has been processed successfully.</p>
                <p><strong>Payment ID:</strong> {paymentId}</p>
                <p>Thank you for using AgroRent!</p>";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.Username),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error in production
                Console.WriteLine($"Failed to send email to {to}: {ex.Message}");
            }
        }
    }
}
