using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masters3.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string CommentUser { get; set; } = null!;

        public string Status { get; set; } = null!;
        [ForeignKey("Product")]
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
