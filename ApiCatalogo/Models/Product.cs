using MainBlog.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainBlog.Models;

[Table("Product")]
public class Product : IValidatableObject
{
    [Key]
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

    public DateTime CreateDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
       if(!string.IsNullOrEmpty(this.Name))
        {
           var fistLetter = this.Name[0].ToString();
           if (fistLetter != fistLetter.ToUpper())
            {
               yield return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula", new string[] { nameof(Name) });
           }
       }
       if(this.Inventory <= 0)
       {
           yield return new ValidationResult("O estoque deve ser maior que zero", new string[] { nameof(Inventory) });
       }
    }
}
