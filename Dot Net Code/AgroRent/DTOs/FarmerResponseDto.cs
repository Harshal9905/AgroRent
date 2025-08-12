namespace AgroRent.DTOs
{
    public class FarmerResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
