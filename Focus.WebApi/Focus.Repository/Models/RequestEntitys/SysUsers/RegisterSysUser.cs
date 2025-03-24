using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class RegisterSysUser
    {
        [Description("登录名")]
        public string LoginName { get; set; }
        [Description("登录密码")]
        public string LoginPwd { get; set; }
        [Description("确认登录密码")]
        public string ConfirmLoginPwd { get; set; }
        [Description("用户名称")]
        public string UserName { get; set; }
        [Description("手机号码")]
        public string? PhoneNumber { get; set; }
        [Description("用户住址")]
        public string? Address { get; set; }
        [Description("用户邮箱")]
        [IsEmail(ErrorMessage ="邮箱格式不正确")]
        public string? Email { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsTelephone : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            if (!Regex.IsMatch(value.ToString() ?? "", @"^(\d{3,4}-)?\d{6,8}$")) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsMobile : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            if (!Regex.IsMatch(value.ToString() ?? "", @"^1[3456789]\d{9}$")) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RegexMatch : ValidationAttribute
    {
        /// <summary>
        ///     正则字符串
        /// </summary>
        public string RegexStr { get; set; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            if (!Regex.IsMatch(value.ToString() ?? "", @$"{RegexStr}")) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsIDcard : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            try
            {
                if (DateTime.IsLeapYear(Convert.ToInt32(value.ToString().Substring(6, 4))))
                {
                    string regexStr = @"(^[1-9]\d{5}(19|20)\d{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2]\d|3[0-1])|(04|06|09|11)(0[1-9]|[1-2]\d|30)|02(0[1-9]|[1-2]\d))\d{3}[\dXx]$)";
                    if (!Regex.IsMatch(value.ToString() ?? "", regexStr)) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                else
                {
                    string regexStr = @"(^[1-9]\d{5}(19|20)\d{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2]\d|3[0-1])|(04|06|09|11)(0[1-9]|[1-2]\d|30)|02(0[1-9]|1\d|2[0-8]))\d{3}[\dXx]$)";
                    if (!Regex.IsMatch(value.ToString() ?? "", regexStr)) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsEmail : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            if (!Regex.IsMatch(value.ToString() ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

}
