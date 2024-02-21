using System.ComponentModel.DataAnnotations;

namespace ZumRailsPosts.API.Attributes.Validations
{
    public class PostsSortAttribute : ValidationAttribute
    {
        public static readonly HashSet<string> validValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "id", "reads", "likes", "popularity" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string sortBy = value as string;

            if (string.IsNullOrWhiteSpace(sortBy) || !validValues.Contains(sortBy))
            {
                return new ValidationResult("Invalid value for 'sortBy' parameter. Valid values are: id, reads, likes, popularity.");
            }

            return ValidationResult.Success;
        }
    }
}
