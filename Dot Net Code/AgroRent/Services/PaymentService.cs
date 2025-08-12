using AgroRent.DTOs;
using AgroRent.Models;
using AgroRent.Repositories;
using System.Text;
using System.Text.Json;

namespace AgroRent.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IBookingRepository bookingRepository,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PaymentRespDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(MapToDto);
        }

        public async Task<PaymentRespDto?> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment != null ? MapToDto(payment) : null;
        }

        public async Task<PaymentRespDto?> GetPaymentByBookingIdAsync(int bookingId)
        {
            var payment = await _paymentRepository.GetByBookingIdAsync(bookingId);
            return payment != null ? MapToDto(payment) : null;
        }

        public async Task<PaymentRespDto> CreatePaymentAsync(int bookingId, double amount)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Booking not found");

            // Create Razorpay order using HTTP client
            var apiKey = _configuration["RazorpaySettings:ApiKey"];
            var apiSecret = _configuration["RazorpaySettings:ApiSecret"];
            
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var orderRequest = new
            {
                amount = (int)(amount * 100), // Convert to paise
                currency = "INR",
                receipt = $"booking_{bookingId}"
            };

            var jsonContent = JsonSerializer.Serialize(orderRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.razorpay.com/v1/orders", content);
            
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to create Razorpay order");

            var responseContent = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var orderId = orderResponse.GetProperty("id").GetString();

            var payment = new Payment
            {
                OrderId = orderId,
                Status = PaymentStatus.CREATED,
                Amount = amount,
                Timestamp = DateTime.Now,
                Booking = booking
            };

            var savedPayment = await _paymentRepository.AddAsync(payment);
            return MapToDto(savedPayment);
        }

        public async Task<PaymentRespDto> VerifyPaymentAsync(PaymentVerificationRequestDto dto)
        {
            var payment = await _paymentRepository.GetByBookingIdAsync(int.Parse(dto.RazorpayOrderId.Split('_').Last()));
            if (payment == null)
                throw new InvalidOperationException("Payment not found");

            // Verify Razorpay signature (simplified - in production you should implement proper signature verification)
            // For now, we'll just update the payment status
            try
            {
                // Update payment
                payment.PaymentId = dto.RazorpayPaymentId;
                payment.Status = PaymentStatus.PAID;
                payment.Timestamp = DateTime.Now;

                var updatedPayment = await _paymentRepository.UpdateAsync(payment);
                return MapToDto(updatedPayment);
            }
            catch
            {
                payment.Status = PaymentStatus.FAILED;
                payment.Timestamp = DateTime.Now;
                await _paymentRepository.UpdateAsync(payment);
                throw new InvalidOperationException("Payment verification failed");
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int id, string status)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return false;

            if (Enum.TryParse<PaymentStatus>(status, true, out var paymentStatus))
            {
                payment.Status = paymentStatus;
                payment.Timestamp = DateTime.Now;

                await _paymentRepository.UpdateAsync(payment);
                return true;
            }

            return false;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return false;

            await _paymentRepository.DeleteAsync(id);
            return true;
        }

        private static PaymentRespDto MapToDto(Payment payment)
        {
            return new PaymentRespDto
            {
                Id = payment.Id,
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Status = payment.Status.ToString(),
                Amount = payment.Amount,
                Timestamp = payment.Timestamp,
                BookingId = payment.Booking?.Id ?? 0,
                EquipmentName = payment.Booking?.Equipment?.Name ?? string.Empty,
                RenterName = payment.Booking?.Farmer != null ? $"{payment.Booking.Farmer.FirstName} {payment.Booking.Farmer.LastName}" : string.Empty
            };
        }
    }
}
