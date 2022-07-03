using FluentValidation;
using Focus.Repository.Models.DbEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }
    }
}
