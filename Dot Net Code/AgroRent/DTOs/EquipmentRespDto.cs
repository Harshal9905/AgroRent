namespace AgroRent.DTOs
{
    public class EquipmentRespDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public double RentalPrice { get; set; }
        public bool Available { get; set; }
        public string? CloudinaryPublicId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
