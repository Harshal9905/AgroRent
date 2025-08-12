using System.ComponentModel.DataAnnotations;

namespace AgroRent.Models
{
    public class VerificationToken
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public virtual Farmer? Farmer { get; set; }

        public DateTime ExpiryDate { get; set; }

        public VerificationToken() { }

        public VerificationToken(string token, Farmer farmer, DateTime expiryDate)
        {
            Token = token;
            Farmer = farmer;
            ExpiryDate = expiryDate;
        }
    }
}
