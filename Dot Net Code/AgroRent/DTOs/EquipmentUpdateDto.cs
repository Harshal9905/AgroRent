using System.ComponentModel.DataAnnotations;

namespace AgroRent.DTOs
{
    public class EquipmentUpdateDto
    {
        [StringLength(30)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? RentalPrice { get; set; }

        public bool? Available { get; set; }
    }
}
