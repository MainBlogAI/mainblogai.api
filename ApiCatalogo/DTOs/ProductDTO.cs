using MainBlog.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MainBlog.Models;

namespace MainBlog.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(80)]
        [CapitalizedFirstLetter]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImgUrl { get; set; }

        public float Inventory { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
