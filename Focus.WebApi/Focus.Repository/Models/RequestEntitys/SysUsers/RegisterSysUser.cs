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
        [BetweenValue(ErrorMessage = "年龄必须在0~150之间", IsEqualMinValue = true,IsEqualMaxValue =true, MinValue = 0, MaxValue = 150)]
        public int Age { get; set; }
        [GreaterThanMinValue(MinValue =0,ErrorMessage ="金额必须大于0")]
        public decimal PayMoney { get; set; }
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
    /// <summary>
    ///     <para>M.Simple.Expand扩展</para>
    ///     <para>必须大于最小值</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GreaterThanMinValue : ValidationAttribute
    {
        /// <summary>
        ///     最小值
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        ///     是否可以等于最小值
        /// </summary>
        public bool IsEqualMinValue { get; set; } = false;
        /// <summary>
        ///     验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //为空返回提示
            if (value == null) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            try
            {
                _ = int.TryParse(value.ToString() ?? "", out int result);
                if (IsEqualMinValue && result < MinValue)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                if (!IsEqualMinValue && result <= MinValue)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
    /// <summary>
    ///     <para>M.Simple.Expand扩展</para>
    ///     <para>必须小于最大值</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LessThanMaxValue : ValidationAttribute
    {
        /// <summary>
        ///     最大值
        /// </summary>
        public int MaxValue { get; set; }
        /// <summary>
        ///     是否可以等于最大值
        /// </summary>
        public bool IsEqualMaxValue { get; set; } = false;
        /// <summary>
        ///     验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //为空返回提示
            if (value == null) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            try
            {
                _ = int.TryParse(value.ToString() ?? "", out int result);

                if (IsEqualMaxValue && result > MaxValue)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                if (!IsEqualMaxValue && result >= MaxValue)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
    /// <summary>
    ///     <para>M.Simple.Expand扩展</para>
    ///     <para>两者之间</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BetweenValue : ValidationAttribute
    {
        /// <summary>
        ///     最小值
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        ///     最大值
        /// </summary>
        public int MaxValue { get; set; }
        /// <summary>
        ///     是否可以等于最小值
        /// </summary>
        public bool IsEqualMinValue { get; set; } = false;
        /// <summary>
        ///     是否可以等于最大值
        /// </summary>
        public bool IsEqualMaxValue { get; set; } = false;
        /// <summary>
        ///     验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //为空返回提示
            if (value == null) return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            try
            {
                _ = int.TryParse(value.ToString() ?? "", out int result);

                if (IsEqualMinValue && !IsEqualMaxValue && !(result >= MinValue && result < MaxValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                if (!IsEqualMinValue && IsEqualMaxValue && !(result > MinValue && result <= MaxValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                if (IsEqualMaxValue && IsEqualMinValue && !(result >= MinValue && result <= MaxValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                if (!IsEqualMaxValue && !IsEqualMinValue && !(result > MinValue && result < MaxValue))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
}
