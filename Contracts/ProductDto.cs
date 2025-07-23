using System.ComponentModel.DataAnnotations;

namespace TodoAPICS.Contracts
{
    public class ProductDto
    {

        public int Id { get; set; } // Include for updates

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public bool Available { get; set; }

        [StringLength(50)]
        public string Color { get; set; }

        [StringLength(20)]
        public string Size { get; set; }

        public string Attributes { get; set; } // JSON string
    }
}
