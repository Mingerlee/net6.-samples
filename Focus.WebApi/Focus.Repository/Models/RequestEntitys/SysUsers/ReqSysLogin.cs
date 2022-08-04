using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class ReqSysLogin
    {
        [Description("登录名")]
        public string? LoginName { get; set; }
        [Description("登录密码")]
        public string? LoginPwd { get; set; }
        [Description("验证码")]
        public string? VerifyCode { get; set; }
    }
}
