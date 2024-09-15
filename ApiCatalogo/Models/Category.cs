using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models;

[Table("Category")]
public class Category
{
    public Category()
    {
        Product = new Collection<Product>();
    }
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(300)]
    public string? imgUrl { get; set; }

    public ICollection<Product>? Product { get; set; }
}
