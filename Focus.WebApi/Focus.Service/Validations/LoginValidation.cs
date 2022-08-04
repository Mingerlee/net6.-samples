using FluentValidation;
using Focus.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service.Validations
{
    public class LoginValidation : AbstractValidator<ReqSysLogin>
    {
        public LoginValidation()
        {
            //CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.LoginName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.LoginPwd).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.VerifyCode).NotEmpty().WithMessage("验证码不能为空");
        }
    }
}
