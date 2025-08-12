namespace AgroRent.DTOs
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public double TotalAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public int FarmerId { get; set; }
        public string FarmerName { get; set; } = string.Empty;
    }
}
