using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AgroRent.Models
{
    public class Farmer : BaseEntity, IUser
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(30)]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public Role Role { get; set; }

        public bool Active { get; set; } = true;

        // Razorpay identifiers
        public string? RazorpayContactId { get; set; }

        public string? RazorpayFundAccountId { get; set; }

        // Navigation properties
        public virtual ICollection<Equipment> EquipmentList { get; set; } = new List<Equipment>();

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public string UserName => Email;

        public string? NormalizedUserName { get; set; }

        public string? NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public void AddEquipment(Equipment equipment)
        {
            EquipmentList.Add(equipment);
            equipment.Owner = this;
        }

        public void RemoveEquipment(Equipment equipment)
        {
            EquipmentList.Remove(equipment);
            equipment.Owner = null;
        }

        public void AddBooking(Booking booking)
        {
            Bookings.Add(booking);
            booking.Farmer = this;
        }

        public void RemoveBooking(Booking booking)
        {
            Bookings.Remove(booking);
            booking.Farmer = null;
        }
    }

    public interface IUser
    {
        string UserName { get; }
        string? NormalizedUserName { get; set; }
        string? NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string? PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool TwoFactorEnabled { get; set; }
        DateTimeOffset? LockoutEnd { get; set; }
        bool LockoutEnabled { get; set; }
        int AccessFailedCount { get; set; }
    }
}
