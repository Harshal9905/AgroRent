using System.ComponentModel.DataAnnotations;

namespace AgroRent.Models
{
    public abstract class BaseEntity
    {
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
