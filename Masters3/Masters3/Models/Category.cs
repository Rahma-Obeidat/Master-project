using System.ComponentModel.DataAnnotations;

namespace Masters3.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; } = null!;

        public string ImagePath { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}

