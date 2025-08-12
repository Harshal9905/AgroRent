using System.ComponentModel.DataAnnotations;

namespace AgroRent.Models
{
    public class Equipment : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required]
        public double RentalPrice { get; set; }

        public bool Available { get; set; } = true;

        public string? CloudinaryPublicId { get; set; }

        // Navigation properties
        public virtual Farmer? Owner { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public void AddBooking(Booking booking)
        {
            Bookings.Add(booking);
            booking.Equipment = this;
        }

        public void RemoveBooking(Booking booking)
        {
            Bookings.Remove(booking);
            booking.Equipment = null;
        }
    }
}
