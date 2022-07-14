using FluentValidation.Results;

namespace Infrastructure.ValidationManager
{
    public interface IValidatorService
    {
        /// <summary>
        /// 默认校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Valid<T>(T value, string validateName, ref List<ValidationFailure> errors);
    }
}
