using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masters3.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string ImagePath { get; set; } = null!;
        [ForeignKey("Categorys")]
        public int? CategoryId { get; set; }

        public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

        public virtual Category? Categorys { get; set; }

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    
}
}
