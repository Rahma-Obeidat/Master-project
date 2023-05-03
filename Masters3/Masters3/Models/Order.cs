using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masters3.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int? Quantity { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }

        public DateTime DateOfOrder { get; set; }

        public virtual Product? Product { get; set; }

        public virtual User? User { get; set; }
    
}
}
