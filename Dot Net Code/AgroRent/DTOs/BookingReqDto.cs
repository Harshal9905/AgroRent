using System.ComponentModel.DataAnnotations;

namespace AgroRent.DTOs
{
    public class BookingReqDto
    {
        [Required]
        public int EquipmentId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
