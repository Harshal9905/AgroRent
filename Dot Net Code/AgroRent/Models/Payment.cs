using System.ComponentModel.DataAnnotations;

namespace AgroRent.Models
{
    public class Payment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string? PaymentId { get; set; }      // Razorpay payment ID

        public string? OrderId { get; set; }        // Razorpay order ID

        public PaymentStatus Status { get; set; }         // CREATED, PAID, FAILED

        [Required]
        public double Amount { get; set; }

        public DateTime Timestamp { get; set; }

        // Navigation properties
        public virtual Booking? Booking { get; set; }
    }
}
