using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masters3.Models
{
    public class checkout
    {
        [Key]
        public int Id { get; set; }

        public string Fname { get; set; } = null!;

        public string Lname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string City { get; set; } = null!;
        [ForeignKey("User")]
        public string? UserId { get; set; }

        public virtual User? User { get; set; }
    
}
}
