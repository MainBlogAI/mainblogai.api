using System.ComponentModel.DataAnnotations;

namespace MainBlog.Validation
{
    public class CapitalizedFirstLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("The first letter of the product name must be uppercase.");
            }

            return ValidationResult.Success;
        }
    }

}
