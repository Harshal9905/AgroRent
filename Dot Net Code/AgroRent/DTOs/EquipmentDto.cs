using System.ComponentModel.DataAnnotations;

namespace AgroRent.DTOs
{
    public class EquipmentDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public double RentalPrice { get; set; }
    }
}
