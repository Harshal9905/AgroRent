namespace AgroRent.DTOs
{
    public class PaymentDetailsDto
    {
        public int Id { get; set; }
        public string? PaymentId { get; set; }
        public string? OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int BookingId { get; set; }
    }
}
