using System.ComponentModel.DataAnnotations;

namespace AgroRent.Models
{
    public class Booking : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public BookingStatus Status { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        // Navigation properties
        public virtual Equipment? Equipment { get; set; }

        public virtual Farmer? Farmer { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
