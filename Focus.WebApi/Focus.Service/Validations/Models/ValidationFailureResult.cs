using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service.Validations
{
    public class ValidationFailureResult
    {
        public static implicit operator ValidationFailureResult(ValidationFailure model)
        {
            return new ValidationFailureResult
            {
                PropertyName = model.PropertyName,
                ErrorMessage = model.ErrorMessage,
            };
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string? PropertyName { get; set; }
        /// <summary>
        /// 属性描述
        /// </summary>
        public string? PropertyDescription { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

    }

    public static class ValidationFailureResultExtensions
    {
        public static List<ValidationFailureResult> ToValidationFailureResultList(this List<ValidationFailure> list)
        {
            List<ValidationFailureResult> listResult = new List<ValidationFailureResult>();
            foreach (ValidationFailure item in list)
            {
                listResult.Add(item);
            }
            return listResult;
        }
        public static List<ValidationFailureResult> ToValidationFailureResultList(this List<ValidationFailure> list, Type type)
        {
            List<ValidationFailureResult> listResult = new List<ValidationFailureResult>();
            foreach (ValidationFailure item in list)
            {
                ValidationFailureResult validationFailureResult = item;
                validationFailureResult.PropertyDescription = GetDescription(type, item.PropertyName);
                listResult.Add(validationFailureResult);
            }
            return listResult;
        }
        private static string GetDescription(Type type, string PropertyName)
        {
            if (!string.IsNullOrEmpty(PropertyName))
            {
                PropertyInfo property = type.GetProperty(PropertyName);
                object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), false);  //获取描述属性
                if (objs == null || objs.Length == 0)  //当描述属性没有时，直接返回名称
                    return PropertyName;
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
                return descriptionAttribute.Description;
            }
            return PropertyName;
        }
    }

}
