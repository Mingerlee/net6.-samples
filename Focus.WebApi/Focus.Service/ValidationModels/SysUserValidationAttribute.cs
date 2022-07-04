using FluentValidation;
using Focus.Repository.Models.DbEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Focus.Service.ValidationModels
{
    public class SysUserValidationAttribute: AbstractValidator<SysUser>
    {
        public SysUserValidationAttribute()
        {
            //如果设置为Stop，则检测到失败的验证，则立即终止，不会继续执行剩余属性的验证。 
            //默认值为 Continue 
            //CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空").Length(2, 12).WithMessage("用户名至少2个字符，最多12个字符");
            RuleFor(x => x.UserPwd).NotEmpty().WithMessage("密码不能为空")
                                                  .Length(6, 16).WithMessage("密码长度至少6个字符，最多16个字符")
                                                  .Must(EncryptionPassword).WithMessage("密码不符合规则,必须包含数字、小写或大写字母、特殊符号");
            //RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("确认密码不能为空").Must(ComparePassword).WithMessage("确认密码必须跟密码一样");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("手机号不能为空").Must(IsMobile).WithMessage("手机号格式不正确");
        }
        private bool EncryptionPassword(string password)
        {
            //正则
            var regex = new Regex(@"
                               (?=.*[0-9])                     #必须包含数字
                               (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                               (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                               .{6,16}                         #至少6个字符，最多16个字符 ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            return regex.IsMatch(password);
        }
        private bool ComparePassword(SysUser model, string confirmpwd)
        {
            //return string.Equals(model.UserPwd, confirmpwd, StringComparison.OrdinalIgnoreCase);  //比较字符串并忽略大小写
            return string.Equals(model.UserPwd, confirmpwd);
        }
        private bool IsMobile(string arg)
        {
            return Regex.IsMatch(arg, @"^[1][3-8]\d{9}$");
        }
    }
}
