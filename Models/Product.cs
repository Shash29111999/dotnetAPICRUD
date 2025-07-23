using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TodoAPICS.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Ensure correct precision for currency
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

        // Using a string for attributes to store JSON, similar to your previous payload
        // In EF Core 6+, you could use JSON columns directly if your database supports it.
        [Column(TypeName = "nvarchar(max)")]
        public string Attributes { get; set; } // Stores JSON string like {"weight":"500g","brand":"Brand X"}

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
