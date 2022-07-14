using FluentValidation;
using FluentValidation.Results;

namespace Infrastructure.ValidationManager
{
    public class ValidatorService : IValidatorService
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Valid<T>(T value, string validateName,ref List<ValidationFailure> errors)
        {
            Type type = ValidatorContainer.ValidatorContainers[validateName];
            var context = new ValidationContext<T>(value);

            var validator = Activator.CreateInstance(type) as IValidator;
            ValidationResult result = validator.Validate(context);

            if (result.IsValid) return true;

            errors = result.Errors;

            return false;
        }
    }
}
