using System.ComponentModel.DataAnnotations;

namespace Masters3.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string NameOnCard { get; set; } = null!;

        public int ExpYear { get; set; }

        public int ExpMonth { get; set; }

        public string CardNumber { get; set; } = null!;

        public string Cvv { get; set; } = null!;

        public double Ammount { get; set; }
    }
}
