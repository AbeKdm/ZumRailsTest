using System.ComponentModel.DataAnnotations;

namespace ZumRailsPosts.API.Attributes.Validations
{
    public class DirectionAttribute : ValidationAttribute
    {
        public static readonly HashSet<string> validValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "asc", "desc" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string direction = value as string;

            if (string.IsNullOrWhiteSpace(direction) || !validValues.Contains(direction))
            {
                return new ValidationResult("Invalid value for 'direction' parameter. Valid values are: asc, desc.");
            }

            return ValidationResult.Success;
        }
    }
}
