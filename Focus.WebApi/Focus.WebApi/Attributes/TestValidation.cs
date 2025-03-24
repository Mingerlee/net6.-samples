using System.ComponentModel.DataAnnotations;

namespace Focus.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TestValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                //create errorMessage
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);

                //and immediately exit loop returning Validation result
                //because 1 requirement is not met, which is enough for error
                return new ValidationResult(errorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
            return null;
        }
    }
}
