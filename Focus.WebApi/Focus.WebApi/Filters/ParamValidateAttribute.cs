using FluentValidation.Results;
using Focus.Service.Validations;
using Infrastructure.Enums;
using Infrastructure.Models;
using Infrastructure.ValidationManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Focus.WebApi.Filters
{
    public class ParamValidateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证类名
        /// </summary>
        private readonly string _validateName;
        public ParamValidateAttribute(Type validateName = null)
        {
            _validateName = validateName.FullName;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validatorService = context.HttpContext.RequestServices.GetService(typeof(IValidatorService)) as IValidatorService;
            List<ValidationFailure> errors = new List<ValidationFailure>();

            //依次对参数进行校验
            foreach (var argument in context.ActionArguments)
            {
                if (!validatorService.Valid(argument.Value, _validateName, ref errors))
                {
                    //Tips:验证不通过，输出验证信息
                    ResultModel result = new ResultModel
                    {
                        Status = 0,
                        Data = errors.ToValidationFailureResultList(argument.Value.GetType()),
                        ErrorCode = ResponseCode.sys_verify_failed
                    };
                    context.Result = new OkObjectResult(result);
                    break;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
